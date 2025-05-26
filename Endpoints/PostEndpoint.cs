using edemBack.IFunction;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace edemBack.Endpoints
{
    public class PostEndpoint: IFunction.IEndpoint, ICreateFunction
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            //Create Object
            app.MapPost("/Object", [Authorize(Policy = "level1to2")] async (Models.Object _object, IDatabaseFactory factory) =>
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
            app.MapPost("/Worker", [Authorize(Policy = "level2")] async (Models.Worker worker, IDatabaseFactory factory) =>
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
            app.MapPost("/Office", [Authorize(Policy = "level1")] async (Models.Office office, IDatabaseFactory factory) =>
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
            app.MapPost("/Role", [Authorize(Policy = "level1to2")] async (Models.Role role, IDatabaseFactory factory) =>
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
            app.MapPost("/Request", [Authorize(Policy = "level3")] async (Models.Request request, IDatabaseFactory factory) =>
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
            app.MapPost("/Booking", [Authorize(Policy = "Operator")] async (Models.Booking booking, IDatabaseFactory factory) =>
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
            app.MapPost("/Additionally", [Authorize(Policy = "level1to2")] async (List<Models.Additionally> additionally, IDatabaseFactory factory) =>
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
                .WithTags("Object", "POST")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создать дополнительные параметры объекта";
                    op.Description = "Добавляет список дополнительных параметров объекта в базу данных";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Type Work
            app.MapPost("/TypeWork", [Authorize(Policy = "level1to2")] async (Models.TypeWork type, IDatabaseFactory factory) =>
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
            app.MapPost("/Status", [Authorize(Policy = "level1to2")] async (Models.Status status, IDatabaseFactory factory) =>
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
            app.MapPost("/Status_Request", [Authorize(Policy = "level1to2")] async (Models.Status_Request status, IDatabaseFactory factory) =>
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

            //Create Work_role
            app.MapPost("Work_Role", [Authorize(Policy = "level1to2")] async (Models.Work_Role work_role, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(work_role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(work_role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createWorkRole(work_role, factory);
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
                .WithName("CreateWork_Role")
                .WithTags("POST", "TypeWork", "Role")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создание взаимосвязи типа работы и роли";
                    op.Description = "Создание взаимосвязи типа работы и роли";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Add parametr Role
            app.MapPost("Add_Parametrs_Role", [Authorize(Policy = "level1to2")] async (Models.Add_Parametrs_Role parametr, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(parametr);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(parametr, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createParametrRole(parametr, factory);
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
                .WithName("createParametrRole")
                .WithTags("POST", "Role")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создание дополнительного параметра роли";
                    op.Description = "Создание дополнительного параметра роли";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Create Add Worker for Request
            app.MapPost("/RequestWorker", [Authorize(Policy = "level3")] async (Models.RequestWorker requestWorker, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(requestWorker);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(requestWorker, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await ICreateFunction.createWorkerRequest(requestWorker.Worker, requestWorker.Request, factory);
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
                .WithName("createRequestWorker")
                .WithTags("POST", "Request")
                .WithOpenApi(op =>
                {
                    op.Summary = "Создание связи работника и заявки";
                    op.Description = "Прикрепление назначенного работника к заявке";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
