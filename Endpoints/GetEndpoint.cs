using edemBack.IFunction;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace edemBack.Endpoints
{
    public class GetEndpoint: IFunction.IEndpoint, IGetFunction
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            //GetAllObject and with filter
            app.MapGet("/Objects", [Authorize(Policy = "level1to2")] async (HttpRequest request, IDatabaseFactory factory) =>
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
                    op.Description = "Возвращает все объекты, с возможностью фильтрации по офису, количеству комнат, наличию кухни и балкона. Уровень доступа не выше 2";

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

            //GetAllObject and with filter
            app.MapGet("/Objects/Worker", [Authorize(Policy = "level2to3")] async (HttpContext ctx, HttpRequest request, IDatabaseFactory factory) =>
            {
                //filter parametrs
                bool kitchen = false;
                int office = Convert.ToInt32(ctx.User.FindFirst("Office").Value?.ToString());
                bool balcony = false;
                int rooms = 0;
                //list item
                var objects = await IGetFunction.getObjectsWorker(office, factory);
                var objectsQuery = objects.AsQueryable();
                if (objects.Count == 0)
                {
                    return Results.NoContent();
                }
                //filters
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
                .WithName("GetObjectsWorker")
                .WithTags("Object", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить все объекты для работника";
                    op.Description = "Возвращает все объекты, с возможностью фильтрации по количеству комнат, наличию кухни и балкона. Уровень доступа не выше 2";

                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                        {
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
            app.MapGet("/Object/{id:int}",[Authorize(Policy = "levelAll")] async (int id, IDatabaseFactory factory) =>
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
                    op.Description = "Получение объекта по его идентификатору. Доступно при авторизации";
                    op.Parameters[0].Description = "Идентификатор объекта (Натуральное число)";
                    return op;
                })
                .Produces<Models.Object>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status204NoContent);

            //Get All Workers
            app.MapGet("/Workers", [Authorize(Policy = "level1to2")] async (HttpRequest request, IDatabaseFactory factory) =>
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
                        foreach (var id in request.Query["Role"])
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
                    op.Description = "Возвращает всех работников, с возможностью фильтрации по офису и роли. Уровень доступа не выше 2";

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

            //Get All Workers for worker
            app.MapGet("/Workers/Worker", [Authorize(Policy = "level1to2")] async (HttpContext ctx, HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter
                int id_office = Convert.ToInt32(ctx.User.FindFirst("Office").Value?.ToString()); ;
                List<int> id_roles = new List<int>();
                int id_role = 0;
                //list item
                var workers = await IGetFunction.getWorkersWorker(id_office, factory);
                if (workers.Count == 0)
                {
                    return Results.NoContent();
                }
                var workersQuery = workers.AsQueryable();
                //filters
                //role_id
                if (request.Query.ContainsKey("Role"))
                {
                    Console.WriteLine(request.Query["Role"].Count);
                    if (request.Query["Role"].Count > 1)
                    {
                        foreach (var id in request.Query["Role"])
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
                .WithName("GetWorkersForWorker")
                .WithTags("Worker", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить список работников для сотрудника";
                    op.Description = "Возвращает всех работников, с возможностью фильтрации по офису и роли. Уровень доступа не выше 2";

                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter>
                {
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
            app.MapGet("/Worker/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapGet("/Requests",[Authorize(Policy = "level3")] async (HttpContext ctx, HttpRequest request, IDatabaseFactory factory) =>
            {
                //parametrs filter (подумать над office_id)
                int object_id = -1;
                bool urgency = false;
                string status = "-1";
                bool text_type = false;
                bool text_status = false;
                int office_id = Convert.ToInt32(ctx.User.FindFirst("Office")?.Value);
                int worker = -1;
                //list item
                List<Models.Request> requests = await IGetFunction.getAllRequest(office_id, factory);

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
                        foreach (var row in requestsQuery)
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
                //Worker
                if (request.Query.ContainsKey("Worker"))
                {
                    var parsedWorker = int.TryParse(request.Query["Worker"], out worker);
                    if (parsedWorker)
                    {
                        var requestWorker = await IGetFunction.getAllRequestWorker(worker, factory);
                        if(requestWorker != null || requestWorker.Count != 0)
                        {
                            requestsQuery = requestsQuery.Where(
                            x => requestWorker.Contains(x.Request_Id));
                        }
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
                        },
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Worker",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Идентификатор работника, для которого выводятся заявки",
                            Schema = new Microsoft.OpenApi.Models.OpenApiSchema { Type = "integer" }
                        }
                    };

                    return op;
                })
                .Produces<List<Models.Request>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

            //Get One Request
            app.MapGet("/Request/{id:int}", [Authorize(Policy = "level3to4")] async (int id, IDatabaseFactory factory) =>
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

            app.MapGet("/Requests/Worker", [Authorize(Policy = "level4")] async (HttpContext ctx, IDatabaseFactory factory) =>
            {
                int id = Convert.ToInt32(ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var requests = await IGetFunction.getRequestsWorker(id, factory);
                if (requests.Count == 0)
                {
                    return Results.NoContent();
                }
                return Results.Ok(requests);
            })
                .WithName("GetRequestForWorker")
                .WithTags("Request", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить заявку по ID";
                    op.Description = "Получение заявки для работника (данные из куки)";
                    return op;
                })
                .Produces<Models.Request>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status204NoContent);

            //Get All BookingObject
            app.MapGet("/Bookings", [Authorize(Policy = "level1to2")] async (HttpRequest request, IDatabaseFactory factory) =>
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
                
                if (request.Query.ContainsKey("Office"))
                {
                    var parsedOffice_id = int.TryParse(request.Query["Office"], out office_id);
                    if (parsedOffice_id)
                    {

                        var objects = (await IGetFunction.getAllObject(factory)).AsQueryable().Where(
                            x => x.Office_Id == office_id).Select(x => x.Id).ToList();
                        bookingQuery = bookingQuery.Where(
                            x => objects.Contains(x.Object_id));
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
                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter> {
                        new Microsoft.OpenApi.Models.OpenApiParameter
                        {
                            Name = "Office",
                            In = Microsoft.OpenApi.Models.ParameterLocation.Query,
                            Required = false,
                            Description = "Офис объекта, на которое назначено бронирование",
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
            app.MapGet("/Booking/{id:int}", [Authorize(Policy = "level3")] async (int id, IDatabaseFactory factory) =>
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


            //ИЗМЕНИТЬ
            //Get One Booking date
            app.MapGet("/BookingDate/{date}", [Authorize(Policy = "level3")] async (DateTime date, IDatabaseFactory factory) =>
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

            //Get BookingObject For Worker
            app.MapGet("/Bookings/Worker", [Authorize(Policy = "level3")] async (HttpContext ctx, HttpRequest request, IDatabaseFactory factory) =>
            {
                //filter
                int office_id = Convert.ToInt32(ctx.User.FindFirst("Office").Value?.ToString());
                DateTime date = DateTime.Now;
                //list item
                var bookingList = await IGetFunction.getBookingOffice(office_id, factory);
                if (bookingList.Count == 0)
                {
                    return Results.NoContent();
                }
                var bookingQuery = bookingList.AsQueryable();
                //filters
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
                .WithName("GetBookingsWorker")
                .WithTags("Booking", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить список бронирований для заданного работника";
                    op.Description = "Возвращает все бронирования, с возможностью фильтрации по дате.";
                    op.Parameters = new List<Microsoft.OpenApi.Models.OpenApiParameter> {
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

            //Get All Roles 
            app.MapGet("/Roles", [Authorize(Policy = "level1to2")] async (IDatabaseFactory factory) =>
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
            app.MapGet("/Role/{id:int}", [Authorize(Policy = "levelAll")] async (int id, IDatabaseFactory factory) =>
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
            app.MapGet("/Roles/NoImportant", [Authorize(Policy = "level3")] async (IDatabaseFactory factory) =>
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
            app.MapGet("/Offices", [Authorize(Policy = "level1to2")] async (IDatabaseFactory factory) =>

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
            app.MapGet("/Office/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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

            //Get Office Worker
            app.MapGet("/Office/Worker", [Authorize(Policy = "levelAll")] async (HttpContext ctx, IDatabaseFactory factory) =>
            {
                int id_office = Convert.ToInt32(ctx.User.FindFirst("Office")?.Value);
                var office = await IGetFunction.getOneOffice(id_office, factory);
                if (office == null)
                {
                    return Results.NoContent();
                }
                return Results.Ok(office);
            })
                .WithName("GetOfficeForWorker")
                .WithTags("Office", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить офис работника";
                    op.Description = "Получить офис из данных работника";
                    return op;
                })
                .Produces<List<Models.Office>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status204NoContent);

            //Get One Status
            app.MapGet("/Status/{id:int}", [Authorize(Policy = "level1to3")] async (int id, IDatabaseFactory factory) =>
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
            app.MapGet("/Status", [Authorize(Policy = "level1to3")] async (IDatabaseFactory factory) =>
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
            app.MapGet("/Status_Request/{id:int}", [Authorize(Policy = "level1to4")] async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integers id");
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
            app.MapGet("/Status_Request", [Authorize(Policy = "level1to3")] async (IDatabaseFactory factory) =>
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
            app.MapGet("/TypesWork", [Authorize(Policy = "level1to3")] async (IDatabaseFactory factory) =>
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
            app.MapGet("/TypeWork/{id:int}", [Authorize(Policy = "level1to3")] async (int id, IDatabaseFactory factory) =>
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

            app.MapGet("/MyData", [Authorize(Policy = "levelAll")] async (HttpContext ctx, IDatabaseFactory factory) =>
            {
                int id = Convert.ToInt32(ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var worker = await IGetFunction.getWorker(id, factory);
                if (worker == null)
                {
                    return Results.NoContent();
                }
                else
                {
                    worker.Password = null;
                    return Results.Ok(worker);
                }
            })
                .WithName("GetDataWorker")
                .WithTags("Worker", "GET")
                .WithOpenApi(op =>
                {
                    op.Summary = "Получить работника по авторизации";
                    op.Description = "Получение данных работника по данным авторизации";
                    return op;
                })
                .Produces<Models.Worker>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status204NoContent);
        }
    }
}
