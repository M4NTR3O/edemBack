using edemBack.IFunction;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace edemBack.Endpoints
{
    public class PutEndpoint: IFunction.IEndpoint, IUpdateFunction
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            //Update Worker
            app.MapPut("/UpdateWorker", [Authorize(Policy = "level2")]  async (Models.Worker worker, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateRequest", [Authorize(Policy = "level3")] async (Models.Request request, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateBooking", [Authorize(Policy = "Operator")] async (Models.Booking booking, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateObject", [Authorize(Policy = "level1to2")] async (Models.Object _object, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateStatus", [Authorize(Policy = "level1to2")] async (Models.Status status, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateStatus_Request", [Authorize(Policy = "level1to2")] async (Models.Status_Request status, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateOffice", [Authorize(Policy = "level1")] async (Models.Office office, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateRole", [Authorize(Policy = "level1to2")] async (Models.Role role, IDatabaseFactory factory) =>
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
            app.MapPut("/UpdateTypeWork", [Authorize(Policy = "level1to2")] async (Models.TypeWork typeWork, IDatabaseFactory factory) =>
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

            //Update Work_role
            app.MapPut("UpdateWork_Role", [Authorize(Policy = "level1to2")] async (Models.Work_Role work_role, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(work_role);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(work_role, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateWorkRole(work_role, factory);
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
                .WithName("UpdateWork_Role")
                .WithTags("PUT", "TypeWork", "Role")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменение взаимосвязи типа работы и роли";
                    op.Description = "Изменение взаимосвязи типа работы и роли";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);

            //Update Parametrs Role
            app.MapPut("UpdateParametrRole", [Authorize(Policy = "level1to2")] async (Models.Add_Parametrs_Role parametr, IDatabaseFactory factory) =>
            {
                var validationContext = new ValidationContext(parametr);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(parametr, validationContext, validationResults, true);
                if (isValid)
                {
                    try
                    {
                        await IUpdateFunction.updateParametrRole(parametr, factory);
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
                .WithName("UpdateParametrRole")
                .WithTags("PUT", "Role")
                .WithOpenApi(op =>
                {
                    op.Summary = "Изменение дополнительного параметра роли";
                    op.Description = "Изменение дополнительного параметра роли";
                    return op;
                })
                .Produces(StatusCodes.Status201Created)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
