using edemBack.IFunction;
using edemBack.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Microsoft.AspNetCore.OpenApi;

namespace edemBack
{
    public class Program: IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.AddJsonFile("params.json", optional: true, reloadOnChange: true);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("https://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
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
            });

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseMiddleware<RequestLoggingMiddleware>();


            //GetAllObject and with filter
            app.MapGet("/Objects", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //filter parametrs
                bool kitchen = false;
                int office = 0;
                bool balcony = false;
                int rooms = 0;
                //list item
                var objects = await IGetFunction.getAllObject(factory);
                var objectsQuery = objects.AsQueryable();
                if (objects.Count == 0)
                {
                    Results.NotFound("Not found objects");
                }
                //filters
                //office_id
                if (request.Query.ContainsKey("Office"))
                {
                    var parsedOfficeId = int.TryParse(request.Query["Office"], out office);
                    if (parsedOfficeId)
                    {
                        objectsQuery = objectsQuery.Where(
                            x => x.Office_Id == office);
                    }
                }
                //kitchen
                if (request.Query.ContainsKey("Kitchen"))
                {
                    var parsedKitchen = bool.TryParse(request.Query["Kitchen"], out kitchen);
                    if (parsedKitchen)
                    {
                        objectsQuery = objectsQuery.Where(
                            x => x.Kitchen == kitchen);
                    }
                }
                //balcony
                if (request.Query.ContainsKey("Balcony"))
                {
                    var parsedBalcony = bool.TryParse(request.Query["Balcony"], out balcony);
                    if (parsedBalcony)
                    {
                        objectsQuery = objectsQuery.Where(
                            x => x.Balcony == balcony);
                    }
                }
                //rooms
                if (request.Query.ContainsKey("Rooms"))
                {
                    var parsedRooms = int.TryParse(request.Query["Rooms"], out rooms);
                    if (parsedRooms)
                    {
                        objectsQuery = objectsQuery.Where(
                            x => x.Rooms == rooms);
                    }
                }
                var result = objectsQuery.ToList();
                if (result.Count == 0)
                {
                    return Results.NotFound("Not found objects with that parametrs");
                }
                return Results.Ok(result);
            })
                .WithName("GetAllObjects")
                .WithTags("Object", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все объекты";
                    op.Description = "Возвращает все объекты, с возможностью фильтрации по офису, количеству комнат, наличию кухни и балкона.";

                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                        {
                            new Microsoft.OpenApi.Models.OpenApiParameter
                            {
                                Name = "Office",
                                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                                Required = false,
                                Description = "ID офиса для фильтрации объектов по офису",
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                            },
                            new Microsoft.OpenApi.Models.OpenApiParameter
                            {
                                Name = "Rooms",
                                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                                Required = false,
                                Description = "Количество комнат в объекте для фильтрации объектов по количеству комнат",
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                            },
                            new Microsoft.OpenApi.Models.OpenApiParameter
                            {
                                Name = "Balcony",
                                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                                Required = false,
                                Description = "Наличие балкона для фильтрации объектов по раличию балкона",
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "bool" }
                            },
                            new Microsoft.OpenApi.Models.OpenApiParameter
                            {
                                Name = "Kitchen",
                                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                                Required = false,
                                Description = "Наличие кухни для фильтрации объектов по наличию кухни",
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "bool" }
                            }
                        };

                    return op;
                })
                .Produces<List<Models.Object>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get one object with ID
            app.MapGet("/Object/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var _object = await IGetFunction.getObject(id, factory);
                if (_object == null)
                {
                    return Results.NotFound("Not found object with that id");
                }
                else
                {
                    return Results.Ok(_object);
                }
            })
                .WithName("GetObjectByID")
                .WithTags("Object", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить объект по его ID";
                    op.Description = "Получение объекта по его идентификатору";
                    op.Parameters[0].Description = "Идентификатор объекта (Натуральное число)";
                    return op;
                })
                .Produces<Models.Object>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get All Workers
            app.MapGet("/Workers", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter
                int id_office = 0;
                int id_role = 0;
                //list item
                var workers = await IGetFunction.getAllWorker(factory);
                if (workers.Count == 0)
                {
                    return Results.NotFound("Not found workers");
                }
                var workersQuery = workers.AsQueryable();
                //filters
                //office_id
                if (request.Query.ContainsKey("Office"))
                {
                    var parsedOfficeId = int.TryParse(request.Query["Office"], out id_office);
                    if (parsedOfficeId)
                    {
                        workersQuery = workersQuery.Where(
                            x => x.Id_Office == id_office);
                    }
                }
                //role_id
                if (request.Query.ContainsKey("Role"))
                {
                    var parsedRoleId = int.TryParse(request.Query["Role"], out id_role);
                    if (parsedRoleId)
                    {
                        workersQuery = workersQuery.Where(
                            x => x.Id_Role== id_role);
                    }
                }
                //result list
                var result = workersQuery.ToList();
                //check empty list
                if(result.Count == 0)
                {
                    return Results.NotFound("Not found workers with that parametrs");
                }
                return Results.Ok(result);
            })
                .WithName("GetAllWorkers")
                .WithTags("Worker", "GET")
                .WithOpenApi(op =>
            {
                op.Summary = "Получить список работников";
                op.Description = "Возвращает всех работников, с возможностью фильтрации по офису и роли.";

                op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                {
                    new Microsoft.OpenApi.Models.OpenApiParameter
                    {
                        Name = "Office",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                        Required = false,
                        Description = "ID офиса для фильтрации работников по офису",
                        Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                    },
                    new Microsoft.OpenApi.Models.OpenApiParameter
                    {
                        Name = "Role",
                        In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                        Required = false,
                        Description = "ID роли для фильтрации работников по роли",
                        Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                    }
                };

                return op;
            })
                .Produces<List<Models.Worker>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Worker with ID
            app.MapGet("/Worker/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var worker = await IGetFunction.getWorker(id, factory);
                if (worker == null)
                {
                    return Results.NotFound("Not found worker");
                }
                else
                {
                    return Results.Ok(worker);
                }
            })
                .WithName("GetWorkerByID")
                .WithTags("Worker", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить работника по ID";
                    op.Description = "Получение работника по его целочисленному идентификатору";
                    op.Parameters[0].Description = "Идентификатор работника (Натуральное число)";
                    return op;
                })
                .Produces<Models.Worker>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get All Requests
            app.MapGet("/Requests", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter (подумать над office_id)
                bool urgency = false;
                string? status = null;
                //list item
                List<Models.Request> requests = await IGetFunction.getAllRequest(factory);
                
                if (requests.Count == 0)
                {
                    return Results.NotFound("Not found requests");
                }
                var requestsQuery = requests.AsQueryable();
                //filters
                //status
                if (request.Query.ContainsKey("Status"))
                {
                    status = request.Query["Status"];
                    if (status != null)
                    {
                        requestsQuery = requestsQuery.Where(
                            x => x.Status == status);
                    }
                }
                if (request.Query.ContainsKey("Urgency"))
                {
                    var parsedUrgency = bool.TryParse(request.Query["Urgency"], out urgency);
                    if (parsedUrgency)
                    {
                        requestsQuery = requestsQuery.Where(
                            x => x.Urgency == urgency);
                    }
                }
                //result list
                var result = requestsQuery.ToList();
                //check empty list
                if (result.Count == 0)
                {
                    return Results.NotFound("Not found requests with that parametrs");
                }
                return Results.Ok(result);
            })
                .WithName("GetAllRequest")
                .WithTags("Request", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить список заявок";
                    op.Description = "Возвращает все заявки, с возможностью фильтрации по срочности и статусу.";

                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                    {
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Urgency",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Срочность заявки для фильтрации заявок по их срочности",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "bool" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Status",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Статус заявки для фильтрации заявок по их статусу",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" }
                        }
                    };

                    return op;
                })
                .Produces<List<Models.Request>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Request
            app.MapGet("/Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var request = await IGetFunction.getRequest(id, factory);
                if (request == null)
                {
                    return Results.NotFound("Not found request");
                }
                else
                {
                    return Results.Ok(request);
                }
            })
                .WithName("GetRequestByID")
                .WithTags("Request", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить заявку по ID";
                    op.Description = "Получение заявки по его идентификатору";
                    op.Parameters[0].Description = "Идентификатор заявки (Натуральное число)";
                    return op;
                })
                .Produces<Models.Request>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get All BookingObject
            app.MapGet("/Bookings", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //filter
                int office_id = -1;
                DateTime date = DateTime.Now;
                //list item
                var bookingList = await IGetFunction.getAllBooking(factory);
                if (bookingList.Count == 0)
                {
                    return Results.NotFound("Not found booking");
                }
                var bookingQuery = bookingList.AsQueryable();
                //filters
                //office_id
                if (request.Query.ContainsKey("Office_Id"))
                {
                    var parsedOffice_id = int.TryParse(request.Query["Office_Id"], out office_id);
                    if (parsedOffice_id)
                    {
                        var bookings = await bookingQuery.ToListAsync();
                        var objects = await Task.WhenAll(bookings.Select(async x => await IGetFunction.getObject(x.Object_id, factory)));
                        bookingQuery = bookingQuery.Where(
                            x => objects.First(o => o.Office_Id == office_id) != null);
                    }
                }
                //Date
                if (request.Query.ContainsKey("Date"))
                {
                    var parsedDate = DateTime.TryParse(request.Query["Date"], out date);
                    if (parsedDate)
                    {
                        bookingQuery = bookingQuery.Where(
                            x => x.Date_Start <= date && x.Date_End >= date);
                    }
                }
                //result list
                var result = bookingQuery.ToList();
                //check empty list
                if (result.Count == 0)
                {
                    return Results.NotFound("Not found bookings with that parametrs");
                }
                return Results.Ok(result);
            })
                .WithName("GetAllBookings")
                .WithTags("Booking", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить список бронирований";
                    op.Description = "Возвращает все бронирования, с возможностью фильтрации по офису и по дате.";

                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                    {
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Office",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "ID офиса для фильтрации бронирований по офису",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Role",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Дата бронирования, при котором оно активно, для фильтрации бронирований по дате",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "Date" }
                        }
                    };

                    return op;
                })
                .Produces<List<Models.Booking>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Booking
            app.MapGet("/Booking/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var booking = await IGetFunction.getBooking(id, factory);
                if (booking == null)
                {
                    return Results.NotFound("Not found booking");
                }
                else
                {
                    return Results.Ok(booking);
                }
            })
                .WithName("GetBookingByID")
                .WithTags("Booking", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить бронирование по ID";
                    op.Description = "Получение объекта по его идентификатору";
                    op.Parameters[0].Description = "Идентификатор бронирования (Натуральное число)";
                    return op;
                })
                .Produces<Models.Booking>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Booking date
            app.MapGet("/BookingDate/{date}", async (DateTime date, IDatabaseFactory factory) =>
            {
                var booking = await IGetFunction.getBookingForDate(date, factory);
                if (booking == null)
                {
                    return Results.NotFound("Not found booking");
                }
                else
                {
                    return Results.Ok(booking);
                }    
            })
                .WithName("GetBookingByDate")
                .WithTags("Booking", "GET")
                .WithOpenApi(op => 
                {
                    op.Summary = "Получить бронирования по дате";
                    op.Description = "Получение списка бронирований, активных на текущую дату";
                    op.Parameters[0].Description = "Дата, на которую необходимо бронирование (Формат YYYY-MM-DD)";
                    return op;
                })
                .Produces<List<Models.Booking>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get All Roles 
            app.MapGet("/Roles", async (IDatabaseFactory factory) =>
            {
                var roles = await IGetFunction.getAllRoles(factory);
                if (roles.Count == 0)
                {
                    return Results.NotFound("Not found roles");
                }
                else
                {
                    return Results.Ok(roles);
                }
            })
                .WithName("GetAllRoles")
                .WithTags("Role", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все роли";
                    op.Description = "Получить список доступных ролей";
                    return op;
                })
                .Produces<List<Models.Role>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Role
            app.MapGet("/Role/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not Natural integer id");
                }
                var role = await IGetFunction.getRole(id, factory);
                if (role == null)
                {
                    return Results.NotFound("Not found role");
                }
                return Results.Ok(role);
            })
                .WithName("GetRoleByID")
                .WithTags("Role", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить роль по ID";
                    op.Description = "Получить роль по её идентификатору";
                    op.Parameters[0].Description = "Идентификатор роли (Натуральное число)";
                    return op;
                })
                .Produces<Models.Role>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get Not Importnat Roles
            app.MapGet("/Roles/NoImportant", async (IDatabaseFactory factory) =>
            {
                var roles = await IGetFunction.getNotImportnantRoles(factory);
                if (roles.Count == 0)
                {
                    return Results.NotFound("Not found roles");
                }
                else
                {
                    return Results.Ok(roles);
                }
            })
                .WithName("GetNoImportantRoles")
                .WithTags("Role", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить роли исполнителей";
                    op.Description = "Получить список ролей, являющихся исполнительскими";
                    return op;
                })
                .Produces<List<Models.Role>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get All Offices
            app.MapGet("/Offices", async (IDatabaseFactory factory) =>
            {
                var offices = await IGetFunction.getAllOffice(factory);
                if (offices.Count == 0)
                {
                    return Results.NotFound("Not found offices");
                }
                else
                {
                    return Results.Ok(offices);
                }
            })
                .WithName("GetAllOffices")
                .WithTags("Office", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все офисы";
                    op.Description = "Получить список всех офисов";
                    return op;
                })
                .Produces<List<Models.Office>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Get One Status
            app.MapGet("/Status/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natura integers id");
                }
                var status = await IGetFunction.getOneStatus(id, factory);
                if (status == null)
                {
                    return Results.NotFound($"Not found sttus with id {id}");
                }
                else
                {
                    return Results.Ok(status);
                }
            })
                .WithName("GetStatusByID")
                .WithTags("Status", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить статус по id";
                    op.Description = "Получить статус по его идентификатору";
                    op.Parameters[0].Description = "Идентификатор статуса (Натуральное число)";
                    return op;
                })
                .Produces<Models.Status>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status404NotFound);

            //Get All Status
            app.MapGet("/Status", async (IDatabaseFactory factory) =>
            {
                var status = await IGetFunction.getAllStatus(factory);
                if (status.Count == 0)
                {
                    return Results.NotFound("Not found statuses");
                }
                else
                {
                    return Results.Ok(status);
                }
            })
                .WithName("GetAllStatus")
                .WithTags("Status", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все статусы";
                    op.Description = "Получить список доступных статусов";
                    return op;
                })
                .Produces<List<Models.Status>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            //Create Object
            app.MapPost("/Object", async (Models.Object _object, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createObject(_object, factory);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateObject")
                .WithTags("Object", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать объект";
                    op.Description = "Добавляет объект в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Worker
            app.MapPost("/Worker", async (Models.Worker worker, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createWorker(worker, factory);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateWorker")
                .WithTags("Worker", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать работника";
                    op.Description = "Добавляет работника в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Office
            app.MapPost("/Office", async (Models.Office office, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(office);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(office, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createOffice(office, factory);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateOffice")
                .WithTags("Office", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать офис";
                    op.Description = "Добавляет офис в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Role
            app.MapPost("/Role", async (Models.Role role, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createRole(role, factory);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateRole")
                .WithTags("Role", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать роль";
                    op.Description = "Добавляет роль в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Request
            app.MapPost("/Request", async (Models.Request request, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createRequest(request, factory);
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateRequest")
                .WithTags("Request", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать заявку";
                    op.Description = "Добавляет заявку в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Booking
            app.MapPost("/Booking", async (Models.Booking booking, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        bool flag = await ICreateFunction.createBooking(booking, factory);
                        if (flag)
                        {
                            return Results.Created();
                        }
                        return Results.BadRequest("Наложение дат");
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateBooking")
                .WithTags("Booking", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать бронирование";
                    op.Description = "Добавляет бронирование в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
            //Create Additional
            app.MapPost("/Additionally", async (List<Models.Additionally> additionally, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(additionally);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(additionally, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        foreach(var additional in additionally)
                        {
                            await ICreateFunction.createAdditional(additional, factory);
                        }
                        return Results.Created();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateAdditionally")
                .WithTags("Additionally", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать дополнительные параметры объекта";
                    op.Description = "Добавляет список дополнительных параметров объекта в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Worker
            app.MapPut("/UpdateWorker", async (Models.Worker worker, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateWorker(worker, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("UpdateWorker")
                .WithTags("Worker", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить работника";
                    op.Description = "Обновляет запись работника в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Request
            app.MapPut("/UpdateRequest", async (Models.Request request, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateRequest(request, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("UpdateRequest")
                .WithTags("Request", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить заявку";
                    op.Description = "Обновляет запись заявки в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Booking
            app.MapPut("/UpdateBooking", async (Models.Booking booking, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateBooking(booking, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("UpdateBooking")
                .WithTags("Booking", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить бронирование";
                    op.Description = "Обновляет запись бронирования в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Object
            app.MapPut("/UpdateObject", async (Models.Object _object, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateObject(_object, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("UpdateObject")
                .WithTags("Object", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить объект";
                    op.Description = "Обновляет запись объекта в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Worker password
            /*app.MapPatch("/UpdatePassword/{id: int}", (int id, string password) =>
            {
                IUpdateFunction.UpdatePassword(id, password);
                return Results.NoContent();
            });*/

            //Delete Worker
            app.MapDelete("/Worker/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteWorker(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteWorker")
                .WithTags("Worker", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить работника";
                    op.Description = "Удаляет запись работника в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Delete Object
            app.MapDelete("/Object/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteObject(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteObject")
                .WithTags("Object", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить объект";
                    op.Description = "Удаляет запись объекта из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Delete Booking
            app.MapDelete("/Booking/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteBooking(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteBooking")
                .WithTags("Booking", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить бронирование";
                    op.Description = "Удаляет запись бронирования из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Delete Office
            app.MapDelete("/Office/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteOffice(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteOffice")
                .WithTags("Office", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить офис";
                    op.Description = "Удаляет запись офиса из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Delete Request
            app.MapDelete("/Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteRequest(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteRequest")
                .WithTags("Request", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить заявку";
                    op.Description = "Удаляет запись заявки из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Delete Role
            app.MapDelete("/Role/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteRole(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteRole")
                .WithTags("Role", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить роль";
                    op.Description = "Удаляет запись роли из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent);

            //Auth
            app.MapPost("/Auth", async (AuthRequest request, IDatabaseFactory factory) =>
            {
                List<string> importnantRoles = ["Директор","Администратор", "Оператор", "Менеджер"];
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        //string password = PasswordHasher.Hash(request.Password);
                        int id = await IGetFunction.getAuth(request.Login, request.Password, factory);
                        if (id == 0)
                        {
                            return Results.BadRequest("Нет пользователя");
                        }
                        else if (id == -1)
                        {
                            return Results.BadRequest("Неверный пароль");
                        }
                        else
                        {
                            var role = await IGetFunction.getRole(id, factory);
                            Dictionary<string, object> json = new Dictionary<string, object>();
                            var worker = (await IGetFunction.getWorker(id, factory));
                            json.Add("Worker", worker);
                            if (importnantRoles.Contains(role.Name))
                            {
                                var roles = await IGetFunction.getAllRoles(factory);
                                json.Add("Roles", await IGetFunction.getAllRoles(factory));
                                return Results.Accepted(null, json);
                            }
                            return Results.Accepted(null, json);
                        }
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
