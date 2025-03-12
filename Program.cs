using edemBack.IFunction;
using Org.BouncyCastle.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;

namespace edemBack
{
    public class Program: IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
    {
        public static void Main(string[] args)
        {
            List<Models.Object> objects = new List<Models.Object>();
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
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

            app.MapPost("/Object", (Models.Object _object) =>
            {
                var validationContext = new ValidationContext(_object);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(
                _object, validationContext, validationResults, true);
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
            
            app.Run();
        }
    }
}
