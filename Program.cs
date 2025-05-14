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

namespace edemBack
{
    public class Program : IGetFunction, ICreateFunction, IUpdateFunction, IDeleteFunctions
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
                    return Results.NoContent();
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
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "boolean" }
                            },
                            new Microsoft.OpenApi.Models.OpenApiParameter
                            {
                                Name = "Kitchen",
                                In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                                Required = false,
                                Description = "Наличие кухни для фильтрации объектов по наличию кухни",
                                Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "boolean" }
                            }
                        };

                    return op;
                })
                .Produces<List<Models.Object>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get All Workers
            app.MapGet("/Workers", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter
                int id_office = 0;
                List<int> id_roles = new List<int>();
                int id_role = 0;
                //list item
                var workers = await IGetFunction.getAllWorker(factory);
                if (workers.Count == 0)
                {
                    return Results.NoContent();
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
                    Console.WriteLine(request.Query["Role"].Count);
                    if (request.Query["Role"].Count > 1)
                    {
                        foreach(var id in request.Query["Role"])
                        {
                            if (int.TryParse(id, out int parsedId))
                            {
                                id_roles.Add(parsedId);
                            }
                            
                        }
                        if (id_roles.Count > 0)
                        {
                            workersQuery = workersQuery.Where(x => id_roles.Contains(x.Id_Role));
                        }
                    }
                    else
                    {
                        var parsedRoleId = int.TryParse(request.Query["Role"], out id_role);
                        if (parsedRoleId)
                        {
                            workersQuery = workersQuery.Where(
                                x => x.Id_Role == id_role);
                        }
                    }   
                }
                //result list
                var result = workersQuery.ToList();
                //check empty list
                if (result.Count == 0)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get One Worker with ID //Подумать о необходимости возвращать нехэшированный пароль
            app.MapGet("/Worker/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                var worker = await IGetFunction.getWorker(id, factory);
                if (worker == null)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get All Requests
            app.MapGet("/Requests", async (HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter (подумать над office_id)
                int object_id = -1;
                bool urgency = false;
                string status = "-1";
                bool text_type = false;
                int office_id = -1;
                bool text_status = false;
                //list item
                List<Models.Request> requests = await IGetFunction.getAllRequest(factory);

                if (requests.Count == 0)
                {
                    return Results.NoContent();
                }
                var requestsQuery = requests.AsQueryable();
                //filters
                //status
                if (request.Query.ContainsKey("Status"))
                {
                    status = request.Query["Status"]!;
                    requestsQuery = requestsQuery.Where(
                        x => x.Status == status);
                }
                //text_status
                if (request.Query.ContainsKey("Text_Status"))
                {
                    var parsedStatus = bool.TryParse(request.Query["Text_Status"], out text_status);
                    if (parsedStatus)
                    {
                        foreach(var row in requestsQuery)
                        {
                            Models.Status status1 = await IGetFunction.getOneStatus(int.Parse(row.Status.ToString()!), factory);
                            row.Status = status1.Name;
                        }
                    }
                }
                //text_type
                if (request.Query.ContainsKey("Text_Type"))
                {
                    var parsedType = bool.TryParse(request.Query["Text_Type"], out text_type);
                    if (parsedType)
                    {
                        foreach (var row in requestsQuery)
                        {
                            Models.TypeWork type = await IGetFunction.getOneTypesWorks(factory, int.Parse(row.Type_Work.ToString()));
                            row.Type_Work = type.Name;
                        }
                    }
                }
                //urgency
                if (request.Query.ContainsKey("Urgency"))
                {
                    var parsedUrgency = bool.TryParse(request.Query["Urgency"], out urgency);
                    if (parsedUrgency)
                    {
                        requestsQuery = requestsQuery.Where(
                            x => x.Urgency == urgency);
                    }
                }
                //object_id
                if (request.Query.ContainsKey("Object"))
                {
                    var parsedObject = int.TryParse(request.Query["Object"], out object_id);
                    if (parsedObject)
                    {
                        requestsQuery = requestsQuery.Where(
                            x => x.Object_Id == object_id);
                    }
                }
                //office_id Подумать, пока недореализовано
                if (request.Query.ContainsKey("Office"))
                {
                    var parsedOffice = int.TryParse(request.Query["Office"], out office_id);
                    if (parsedOffice)
                    {
                        //requestsQuery = requestsQuery.Where
                    }
                }
                //result list
                var result = requestsQuery.ToList();
                //check empty list
                if (result.Count == 0)
                {
                    return Results.NoContent();
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
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "boolean" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Status",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Статус заявки для фильтрации заявок по их статусу",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "string" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Object",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Идентификатор объекта для фильтрации заявок по объектам",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Text_Status",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Флаг для смены идентификатора статуса заявок в текстовый вид",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "boolean" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Text_Type",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Флаг для смены идентификатора типа работы заявок в текстовый вид",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "boolean" }
                        }
                    };

                    return op;
                })
                .Produces<List<Models.Request>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
                }
                var bookingQuery = bookingList.AsQueryable();
                //filters
                //office_id
                /*
                if (request.Query.ContainsKey("Office_Id"))
                {
                    var parsedOffice_id = int.TryParse(request.Query["Office_Id"], out office_id);
                    if (parsedOffice_id)
                    {

                        var objects = await Task.WhenAll(bookingQuery.Select(async x => await IGetFunction.getObject(x.Object_id, factory)));
                        bookingQuery = bookingQuery.Where(
                            x => objects.First(o => o.Office_Id == office_id) != null);
                    }
                }*/
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
                    return Results.NoContent();
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
                            Name = "Office_Id",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "ID офиса для фильтрации бронирований по офису",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Date",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Дата бронирования, при котором оно активно, для фильтрации бронирований по дате",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "Date" }
                        }
                    };

                    return op;
                })
                .Produces<List<Models.Booking>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get One Booking date
            app.MapGet("/BookingDate/{date}", async (DateTime date, IDatabaseFactory factory) =>
            {
                var booking = await IGetFunction.getBookingForDate(date, factory);
                if (booking == null)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get All Roles 
            app.MapGet("/Roles", async (IDatabaseFactory factory) =>
            {
                var roles = await IGetFunction.getAllRoles(factory);
                if (roles.Count == 0)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get Not Importnat Roles
            app.MapGet("/Roles/NoImportant", async (IDatabaseFactory factory) =>
            {
                var roles = await IGetFunction.getNotImportnantRoles(factory);
                if (roles.Count == 0)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get All Offices
            app.MapGet("/Offices", async (IDatabaseFactory factory) =>
            {
                var offices = await IGetFunction.getAllOffice(factory);
                if (offices.Count == 0)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get One Offices
            app.MapGet("/Office/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {

                }
                var office = await IGetFunction.getOneOffice(id, factory);
                if (office == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.Ok(office);
                }
            })
                .WithName("GetOneOffice")
                .WithTags("Office", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить офис";
                    op.Description = "Получить офис по его идентификатору";
                    return op;
                })
                .Produces<Models.Role>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status204NoContent);

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
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get All Status
            app.MapGet("/Status", async (IDatabaseFactory factory) =>
            {
                var status = await IGetFunction.getAllStatus(factory);
                if (status.Count == 0)
                {
                    return Results.NoContent();
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
                .Produces(StatusCodes.Status204NoContent);

            //Get One Status_Request
            app.MapGet("/Status_Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natura integers id");
                }
                var status = await IGetFunction.getOneStatusRequest(id, factory);
                if (status == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.Ok(status);
                }
            })
                .WithName("GetStatusRequestByID")
                .WithTags("Status", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить статус заявки по id";
                    op.Description = "Получить статус заявки по его идентификатору";
                    op.Parameters[0].Description = "Идентификатор статуса (Натуральное число)";
                    return op;
                })
                .Produces<Models.Status>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status204NoContent);

            //Get All Status_Request
            app.MapGet("/Status_Request", async (IDatabaseFactory factory) =>
            {
                var status = await IGetFunction.getAllStatusRequest(factory);
                if (status.Count == 0)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.Ok(status);
                }
            })
                .WithName("getAllStatusRequest")
                .WithTags("Status", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все статусы для заявок";
                    op.Description = "Получить список доступных статусов для заявок";
                    return op;
                })
                .Produces<List<Models.Status>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

            //Get Types Works
            app.MapGet("/TypesWork", async (IDatabaseFactory factory) =>
            {
                var typesWorks = await IGetFunction.getTypesWorks(factory);
                if (typesWorks.Count == 0)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.Ok(typesWorks);
                }
            })
                .WithName("GetAllTypesWorks")
                .WithTags("TypeWork", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все типы работ";
                    op.Description = "Получение всех типов работ с их ID и наименованием";
                    return op;
                })
                .Produces<List<Models.TypeWork>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

            //Get One Types Works
            app.MapGet("/TypeWork/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                var typeWork = await IGetFunction.getOneTypesWorks(factory, id);
                if (typeWork != null)
                {
                    return Results.Ok(typeWork);
                }
                else
                {
                    return Results.NoContent();
                }
            })
                .WithName("GetOnrTypeWork")
                .WithTags("GET", "TypeWork")
                .WithOpenApi(op =>
                {
                    op.Summary = "Вернуть тип работы";
                    op.Description = "Вернуть тип работы по идентификатору со списком назначенных ролей на данное задание";
                    return op;
                })
                .Produces<Models.TypeWork>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

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

            //Create Object With Photos

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
                    op.Description = "Добавляет роль в базу данных со списком дополнительных параметров (все идентификаторы ставить -1)";
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
            //Create Additional (А нужно ли оно? По факту, оно работает при создании объекта)
            app.MapPost("/Additionally", async (List<Models.Additionally> additionally, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(additionally);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(additionally, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        foreach (var additional in additionally)
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

            //Create Type Work
            app.MapPost("/TypeWork", async (Models.TypeWork type, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(type);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(type, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createTypeWork(type, factory);
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
                .WithName("CreateTypeWork")
                .WithTags("TypeWork", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создание краткого описания работы";
                    op.Description = "Создание типа работы и связи его с ролями";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Status
            app.MapPost("/Status", async (Models.Status status, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(status);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(status, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createStatus(status, factory);
                        return Results.Created();
                    }
                    catch
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateStatus")
                .WithTags("POST", "Status")
                .WithOpenApi(op =>
                {
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Status
            app.MapPost("/Status_Request", async (Models.Status_Request status, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(status);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(status, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createStatusRequest(status, factory);
                        return Results.Created();
                    }
                    catch
                    {
                        return Results.Problem();
                    }
                }
                else
                {
                    return Results.BadRequest();
                }
            })
                .WithName("CreateStatusRequest")
                .WithTags("POST", "Status")
                .WithOpenApi(op =>
                {
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
                        await IUpdateFunction.updateWorker(worker, factory);
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
                    op.Description = "Обновляет запись работника в базе данных (без пароля)";
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
                        await IUpdateFunction.updateRequest(request, factory);
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
                    op.Description = "Обновляет запись заявки в базе данных (type_id не обновляется)";
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
                        await IUpdateFunction.updateBooking(booking, factory);
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
                    op.Description = "Обновляет запись бронирования в базе данных (без обновления фотографий)";
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
                        await IUpdateFunction.updateObject(_object, factory);
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
                    op.Description = "Обновляет запись объекта в базе данных (без обновления дополнительных фотографий, дополнительных параметров)";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Status
            app.MapPut("/UpdateStatus", async (Models.Status status, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(status);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(status, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateStatus(status, factory);
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
                .WithName("UpdateStatus")
                .WithTags("Status", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить статус";
                    op.Description = "Обновляет запись статуса в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Status_request
            app.MapPut("/UpdateStatus_Request", async (Models.Status_Request status, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(status);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(status, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateStatusRequest(status, factory);
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
                .WithName("UpdateStatus_Request")
                .WithTags("Status", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить статус заявки";
                    op.Description = "Обновляет запись статуса заявки в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Office
            app.MapPut("/UpdateOffice", async (Models.Office office, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(office);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(office, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateOffice(office, factory);
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
                .WithName("UpdateOffice")
                .WithTags("Office", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить офис";
                    op.Description = "Обновляет запись офиса в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Role
            app.MapPut("/UpdateRole", async (Models.Role role, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateRole(role, factory);
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
                .WithName("UpdateRole")
                .WithTags("Role", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить роль";
                    op.Description = "Обновляет запись роли в базе данных (без дополнительных параметров)";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Type_Work
            app.MapPut("/UpdateTypeWork", async (Models.TypeWork typeWork, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(typeWork);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(typeWork, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateTypeWork(typeWork, factory);
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
                .WithName("UpdateTypeWork")
                .WithTags("TypeWork", "PUT")
                .WithOpenApi(op =>
                {
                    op.Summary = "Обновить тип работ";
                    op.Description = "Обновляет запись типа работ в базе данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Worker password - check on security don't release
            app.MapPatch("/UpdatePassword/{id:int}", async (int id, string password, IDatabaseFactory factory) =>
            {
                await IUpdateFunction.updatePassword(id, password, factory);
                return Results.NoContent();
            })
                .WithName("UpdatePassword")
                .WithTags("Worker", "PATCH")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменение пароля";
                    op.Description = "Изменяет пароль пользователя по его идентификатору";
                    return op;
                });

            //Update Status Request
            app.MapPatch("/ChangeStatusRequest/{id_request:int}", async (int id_request, int status, IDatabaseFactory factory) =>
            {
                {
                    try
                    {
                        await IUpdateFunction.changeStatusRequest(id_request, status, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
            })
                .WithName("ChangeStatusRequest")
                .WithTags("PATCH", "Status", "Request")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменить статус заявки";
                    op.Description = "Изменяет целочисленное значение статуса у заявки";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Status Boocking
            app.MapPatch("/ChangeStatusBooking/{id_booking:int}", async (int id_booking, int status, IDatabaseFactory factory) =>
            {
                {
                    try
                    {
                        await IUpdateFunction.changeStatusBooking(id_booking, status, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
            })
                .WithName("ChangeStatusBooking")
                .WithTags("PATCH", "Status", "Booking")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменить статус бронирования";
                    op.Description = "Изменяет целочисленное значение статуса у бронирования, а также объекта при наступлении бронирования";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Status Object
            app.MapPatch("/ChangeStatusObject/{id_object:int}", async (int id_object, int status, IDatabaseFactory factory) =>
            {
                {
                    try
                    {
                        await IUpdateFunction.changeStatusObject(id_object, status, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
            })
                .WithName("ChangeStatusObject")
                .WithTags("PATCH", "Status", "Object")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменить статус объекта";
                    op.Description = "Изменяет целочисленное значение статуса у объекта";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Worker of Request
            app.MapPatch("/UpdateWorkerRequest/{id_request:int}", async (int id_request, int id_worker, IDatabaseFactory factory) =>
            {
                {
                    try
                    {
                        await IUpdateFunction.updateWorkerRequest(id_request, id_worker, factory);
                        return Results.NoContent();
                    }
                    catch (Exception ex)
                    {
                        return Results.Problem();
                    }
                }
            })
                .WithName("UpdateWorkerRequest")
                .WithTags("PATCH", "Worker", "Request")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменить работника на заявку";
                    op.Description = "Изменяет идентификатор работника, назначенного на заявку";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Worker
            app.MapDelete("/Worker/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Object
            app.MapDelete("/Object/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Booking
            app.MapDelete("/Booking/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Office
            app.MapDelete("/Office/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Request
            app.MapDelete("/Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Role
            app.MapDelete("/Role/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
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
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Status
            app.MapDelete("/Status/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteStatus(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteStatus")
                .WithTags("Status", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить статус";
                    op.Description = "Удаляет запись статуса из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Status_Request
            app.MapDelete("/Status_Request/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteStatusRequest(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteStatus_Request")
                .WithTags("Status", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить статус заявки";
                    op.Description = "Удаляет запись статуса заявки из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Type_Work
            app.MapDelete("/TypeWork/{id:int}", async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteTypeWork(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteTypeWork")
                .WithTags("TypeWork", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить тип работ";
                    op.Description = "Удаляет запись типа работ из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Auth Проверил с хэшированием на одном пользователе
            app.MapPost("/Auth", async (AuthRequest request, IDatabaseFactory factory) =>
            {
                List<string> importnantRoles = ["Директор", "Администратор", "Оператор", "Менеджер"];
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
                        else if (id == -1)
                        {
                            return Results.BadRequest("Неверный пароль");
                        }
                        else
                        {
                            var role = await IGetFunction.getRoleIdWorker(id, factory);
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
