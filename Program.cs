using edemBack.IFunction;
using edemBack.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.OpenApi.Models;

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
                    return Results.NoContent();
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
            });

            //Get one object with ID
            /// <summary>
            /// Получение объекта по ID
            /// </summary>
            /// <param name="model">Объект</param>
            /// <returns></returns>
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
            });

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
            });

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
            });

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
            });

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
            });

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
            });
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
            });

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
            });

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
            });

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
            });

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
            });

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
            });

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
            });
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
                    catch(Exception ex)
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            });

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
            });

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
            });

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
            });

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
            });

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
            });
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
            });

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
            });

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
            });

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
            });

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
            });

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
            });

            //Delete Object
            app.MapDelete("/Object/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteObject(id, factory);
                return Results.NoContent();
            });

            //Delete Booking
            app.MapDelete("/Booking/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteBooking(id, factory);
                return Results.NoContent();
            });

            //Delete Office
            app.MapDelete("/Office/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteOffice(id, factory);
                return Results.NoContent();
            });

            //Delete Request
            app.MapDelete("/Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteRequest(id, factory);
                return Results.NoContent();
            });
            //Delete Role
            app.MapDelete("/Role/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                await IDeleteFunctions.DeleteRole(id, factory);
                return Results.NoContent();
            });

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
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(); // по умолчанию будет на /swagger
            }

            app.Run();
        }
    }
}
