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

namespace edemBack
{
    public class Program: IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.WithOrigins("https://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseCors("AllowAll");
            app.UseMiddleware<RequestLoggingMiddleware>();
            daoClass dao = new daoClass();

            //GetAllObject and with filter
            app.MapGet("/Objects", async (HttpRequest request) => 
            {
                //filter parametrs
                bool kitchen = false;
                int office = 0;
                bool balcony = false;
                int rooms = 0;
                //list item
                var objects = await IGetFunction.getAllObject();
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
            app.MapGet("/Object/{id:int}", async (int id) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var _object = await IGetFunction.getObject(id);
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
            app.MapGet("/Workers", async (HttpRequest request) =>
            {
                //parametrs filter
                int id_office = 0;
                int id_role = 0;
                //list item
                var workers = await IGetFunction.getAllWorker();
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
            app.MapGet("/Worker/{id:int}", async (int id) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var worker = await IGetFunction.getWorker(id);
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
            app.MapGet("/Requests", async (HttpRequest request) =>
            {
                //parametrs filter (подумать над office_id)
                bool urgency = false;
                string? status = null;
                //list item
                List<Models.Request> requests = await IGetFunction.getAllRequest();
                
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
            app.MapGet("/Bookings", async (HttpRequest request) =>
            {
                //filter
                int office_id = -1;
                DateTime date = DateTime.Now;
                //list item
                var bookingList = await IGetFunction.getAllBooking();
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
                        var objects = await Task.WhenAll(bookings.Select(async x => await IGetFunction.getObject(x.Object_id)));
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
            app.MapGet("/Booking/{id:int}", async (int id) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var booking = await IGetFunction.getBooking(id);
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
            app.MapGet("/BookingDate/{date}", async (DateTime date) =>
            {
                var booking = await IGetFunction.getBookingForDate(date);
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
            app.MapGet("/Roles", async () =>
            {
                var roles = await IGetFunction.getAllRoles();
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
            app.MapGet("/Role/{id:int}", async (int id) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not Natural integer id");
                }
                var role = await IGetFunction.getRole(id);
                if (role == null)
                {
                    return Results.NotFound("Not found role");
                }
                return Results.Ok(role);
            });

            //Get All Offices
            app.MapGet("/Offices", async () =>
            {
                var offices = await IGetFunction.getAllOffice();
                if (offices.Count == 0)
                {
                    return Results.NotFound("Not found offices");
                }
                else
                {
                    return Results.Ok(offices);
                }
            });

            //Create Object
            app.MapPost("/Object", async (Models.Object _object) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createObject(_object);
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
            app.MapPost("/Worker", async (Models.Worker worker) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createWorker(worker);
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
            app.MapPost("/Office", async (Models.Office office) =>
            {
                var validationContext = new ValidationContext(office);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(office, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createOffice(office);
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
            app.MapPost("/Role", async (Models.Role role) =>
            {
                var validationContext = new ValidationContext(role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createRole(role);
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
            app.MapPost("/Request", async (Models.Request request) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createRequest(request);
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
            app.MapPost("/Booking", async (Models.Booking booking) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        bool flag = await ICreateFunction.createBooking(booking);
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
            app.MapPost("/Additionally", async (List<Models.Additionally> additionally) =>
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
                            await ICreateFunction.createAdditional(additional);
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
            app.MapPut("/UpdateWorker", async (Models.Worker worker) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateWorker(worker);
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
            app.MapPut("/UpdateRequest", async (Models.Request request) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateRequest(request);
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
            app.MapPut("/UpdateBooking", async (Models.Booking booking) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateBooking(booking);
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
            app.MapPut("/UpdateObject", async (Models.Object _object) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.UpdateObject(_object);
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
            app.MapDelete("/Worker/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteWorker(id);
                return Results.NoContent();
            });

            //Delete Object
            app.MapDelete("/Object/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteObject(id);
                return Results.NoContent();
            });

            //Delete Booking
            app.MapDelete("/Booking/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteBooking(id);
                return Results.NoContent();
            });

            //Delete Office
            app.MapDelete("/Office/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteOffice(id);
                return Results.NoContent();
            });

            //Delete Request
            app.MapDelete("/Request/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteRequest(id);
                return Results.NoContent();
            });
            //Delete Role
            app.MapDelete("/Role/{id:int}", async (int id) =>
            {
                await IDeleteFunctions.DeleteRole(id);
                return Results.NoContent();
            });

            //Auth
            app.MapPost("/Auth", async (AuthRequest request) =>
            {
                List<string> importnantRoles = ["Директор","Администратор", "Оператор", "Менеджер"];
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        int id = await IGetFunction.getAuth(request.Login, request.Password);
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
                            var role = await IGetFunction.getRole(id);
                            Dictionary<string, object> json = new Dictionary<string, object>();
                            var worker = (await IGetFunction.getWorker(id));
                            json.Add("Worker", worker);
                            if (importnantRoles.Contains(role.Name))
                            {
                                var roles = await IGetFunction.getAllRoles();
                                json.Add("Roles", await IGetFunction.getAllRoles());
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
