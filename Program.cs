using edemBack.IFunction;
using edemBack.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using System.Security.Cryptography;
using Org.BouncyCastle.Ocsp;
using System.Text.Json;
using Microsoft.AspNetCore.HttpOverrides;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace edemBack
{
    public class Program : IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseUrls("http://+:5077");
            builder.Configuration.AddJsonFile("Params.json", optional: true, reloadOnChange: true);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("https://localhost:3000", "http://185.25.60.46")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "SessionUser";
                    options.Cookie.HttpOnly = true;
                    //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Strict;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                    options.LoginPath = "/";
                    options.LogoutPath = "/";
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("level1to2", policy =>
                    policy.RequireClaim("Level", "1", "2"));
            });

            builder.Services.AddSingleton<IDatabaseFactory, DatabaseFactory>();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "EdemBack API",
                    Description = "Документация существующих эндпоинтов API",
                });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
                // 1) Описание схемы cookie-аутентификации
                options.AddSecurityDefinition("cookieAuth", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = "Cookie",
                    Description = "Введите имя вашей аутентификационной куки, например:\nMyAppAuth=eyJhbGciOiJIUzI1NiIsInR5cCI…"
                });

                // 2) Для всех операций требовать наличия этой куки
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "cookieAuth"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();
            app.UseCors("AllowAll");
            app.UseStaticFiles();
            app.UsePathBase("/api");

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<RequestLoggingMiddleware>();

            //Auth Проверил с хэшированием на одном пользователе
            app.MapPost("/Auth", async (HttpContext ctx, AuthRequest request, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        int id = await IGetFunction.getAuth(request.Login, request.Password, factory);
                        if (id == 0)
                        {
                            return Results.BadRequest("Нет пользователя");
                        }
                        if (id == -1)
                        {
                            return Results.BadRequest("Неверный пароль");
                        }
                        var role = await IGetFunction.getRoleIdWorker(id, factory);
                        var worker = (await IGetFunction.getWorker(id, factory));
                        if (worker == null)
                        {
                            return Results.BadRequest("Нет пользователя");
                        }
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, worker.Id.ToString()),
                            new Claim(ClaimTypes.Name, worker.Surname + " " + worker.Name + " " + worker.Fathername),
                            new Claim(ClaimTypes.Role, role.Name.ToString()!),
                            new Claim("Level", role.LevelImportant.ToString()!)
                        };
                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var props = new AuthenticationProperties
                        {
                            IsPersistent = request.RememberMe,  // поле в AuthRequest
                            ExpiresUtc = request.RememberMe
                            ? DateTimeOffset.UtcNow.AddDays(7)
                            : DateTimeOffset.UtcNow.AddMinutes(30)
                        };

                        // 6) Подписываем куку
                        await ctx.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            principal,
                            props);
                        return Results.Ok(worker);
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem(ex.ToString());
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("Authorization")
                .WithTags("Authorization", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Авторизация";
                    op.Description = "Авторизация сотрудника по его логину (номеру телефона) и паролю";
                    return op;
                })
                .Produces<(int, List<Models.Role>?)>(StatusCodes.Status202Accepted)
                .Produces(StatusCodes.Status400BadRequest);

            var endpointTypes = Assembly.GetExecutingAssembly()
                                        .GetTypes()
                                        .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface);

            foreach (var type in endpointTypes)
            {
                var instance = Activator.CreateInstance(type) as IEndpoint;
                instance?.MapEndpoints(app);
            }

            app.MapGet("/debag/claims", (HttpContext ctx) =>
                Results.Ok(ctx.User.Claims.Select(c => (c.Type, c.Value)))
            );

            app.MapGet("/debug/claims", (HttpContext ctx) =>
            {
                if (!ctx.User.Identity.IsAuthenticated)
                    return Results.Unauthorized();
                return Results.Ok(ctx.User.Claims
                    .Select(c => new { c.Type, c.Value }));
            })
                .RequireAuthorization();  // чтобы сюда шли только после SignIn


            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Minimal API V1");
                    options.RoutePrefix = string.Empty; // Стартовая страница = Swagger
                }); // по умолчанию будет на /swagger
            }

            app.Run();
        }
    }
}
