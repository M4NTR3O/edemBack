using edemBack.IFunction;
using Microsoft.AspNetCore.Authorization;

namespace edemBack.Endpoints
{
    public class DeleteEndpoint: IFunction.IEndpoint, IDeleteFunctions
    {
        public void MapEndpoints(IEndpointRouteBuilder app)
        {
            //Delete Worker
            app.MapDelete("/Worker/{id:int}", [Authorize(Policy = "level2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Object/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Booking/{id:int}", [Authorize(Policy = "level3")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Office/{id:int}", [Authorize(Policy = "level1")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Request/{id:int}", [Authorize(Policy = "level3")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Role/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Status/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/Status_Request/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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
            app.MapDelete("/TypeWork/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
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

            //Delete AddParametrRole
            app.MapDelete("/Add_Parametr_Role/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteAddParametrRole(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteAddParametrRole")
                .WithTags("Role", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить дополнительный параметр роли";
                    op.Description = "Удаляет запись дополнительного параметра роли";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete Work_Role
            app.MapDelete("/Work_Role/{id:int}", [Authorize(Policy = "level1to2")] async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteWorkRole(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteWorkRole")
                .WithTags("TypeWork", "Role", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить взаимосвязь типа работ и роли";
                    op.Description = "Удаляет запись взаимосвязи типа работ и роли из базы данных";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);

            //Delete RequestWorker
            app.MapDelete("RequestWorker/{id:int}", [Authorize(Policy = "level3")] async (int id, IDatabaseFactory factory) =>
            {
                if (id < 1)
                {
                    return Results.BadRequest("Not natural integer ID");
                }
                await IDeleteFunctions.DeleteRequestWorker(id, factory);
                return Results.NoContent();
            })
                .WithName("DeleteRequestWorker")
                .WithTags("Request", "DELETE")
                .WithOpenApi(op =>
                {
                    op.Summary = "Удалить Сотрудника из заявки";
                    op.Description = "Удаляет идентификатор сотрудника из таблицы связи с заявкой";
                    return op;
                })
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
