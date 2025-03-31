using edemBack.IFunction;
using edemBack.Models;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
using System.Text.Json;

namespace edemBack
{
    public class Program: IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
    {
        public static void Main(string[] args)
        {
            List<Models.Object> objects = new List<Models.Object>();
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

            var app = builder.Build();

            app.UseCors("AllowAll");
            daoClass dao = new daoClass();

            //GetAllObject and with filter
            app.MapGet("/Objects", (HttpRequest request) =>
            {
                //filter parametrs
                bool kitchen = false;
                int office = 0;
                bool balcony = false;
                int rooms = 0;
                //list item
                var objects = IGetFunction.getAllObject();
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
            app.MapGet("/Object/{id:int}", (int id) =>
            {
                var objects = IGetFunction.getObject(id);
                if (objects.Count == 0)
                {
                    return Results.NotFound("Not found object with that id");
                }
                else
                {
                    return Results.Ok(objects[0]);
                }
            });

            //Get All Workers
            app.MapGet("/Workers", (HttpRequest request) =>
            {
                //parametrs filter
                int id_office = 0;
                int id_role = 0;
                //list item
                var workers = IGetFunction.getAllWorker();
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
            app.MapGet("/Worker/{id:int}", (int id) =>
            {
                var workers = IGetFunction.getWorker(id);
                if (workers.Count == 0)
                {
                    return Results.NotFound("Not found worker");
                }
                else
                {
                    return Results.Ok(workers);
                }
            });

            //Get All Requests
            app.MapGet("/Requests", (HttpRequest request) =>
            {
                //parametrs filter (подумать над office_id)
                bool urgency = false;
                string? status = null;
                //list item
                var requests = IGetFunction.getAllRequest();
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

            //Get All Booking
            app.MapGet("/Boocking", (HttpRequest request) =>
            {
                //filter
                int office_id = -1;
                //list item
                var bookingList = IGetFunction.getAllBooking();
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
                        bookingQuery = bookingQuery.Where(
                            x => IGetFunction.getObject(x.Object_id)[0].Office_Id == office_id);
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

            //Create Object
            app.MapPost("/Object", (Models.Object _object) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createObject(_object);
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
            app.MapPost("/Worker", (Models.Worker worker) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createWorker(worker);
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
            app.MapPost("/Office", (Models.Office office) =>
            {
                var validationContext = new ValidationContext(office);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(office, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createOffice(office);
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
            app.MapPost("/Role", (Models.Role role) =>
            {
                var validationContext = new ValidationContext(role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createRole(role);
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
            app.MapPost("/Request", (Models.Request request) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createRequest(request);
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
            app.MapPost("/Booking", (Models.Booking booking) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        ICreateFunction.createBooking(booking);
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

            //Update Worker
            app.MapPut("/UpdateWorker", (Models.Worker worker) =>
            {
                var validationContext = new ValidationContext(worker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(worker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        IUpdateFunction.UpdateWorker(worker);
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
            app.MapPut("/UpdateRequest", (Models.Request request) =>
            {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        IUpdateFunction.UpdateRequest(request);
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
            app.MapPut("/UpdateBooking", (Models.Booking booking) =>
            {
                var validationContext = new ValidationContext(booking);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(booking, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        IUpdateFunction.UpdateBooking(booking);
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
            app.MapPut("/UpdateObject", (Models.Object _object) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(_object, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        IUpdateFunction.UpdateObject(_object);
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
            app.MapPatch("/UpdatePassword/{id: int}", (int id, string password) =>
            {
                IUpdateFunction.UpdatePassword(id, password);
                return Results.NoContent();
            });

            //Auth
            app.MapPost("/Auth", (AuthRequest request) =>
             {
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(request, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        int id = IGetFunction.getAuth(request.Login, request.Password);
                        if (id == -2)
                        {
                            return Results.BadRequest("Нет пользователя");
                        }
                        else if (id == -1)
                        {
                            return Results.BadRequest("Неверный пароль");
                        }
                        else
                        {
                            var workers = IGetFunction.getWorker(id);
                            return Results.Accepted(null, workers);
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


            app.Run();
        }
    }
}
