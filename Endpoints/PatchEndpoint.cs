using edemBack.IFunction;

namespace edemBack.Endpoints
{
    public class PatchEndpoint: IFunction.IEndpoint, IUpdateFunction
    {
        public void MapEndpoints(WebApplication app)
        {
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
            app.MapPatch("/ChangeStatusRequest/{id_request:int}", async (int id_request, string status, IDatabaseFactory factory) =>
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
            app.MapPatch("/ChangeStatusBooking/{id_booking:int}", async (int id_booking, string status, IDatabaseFactory factory) =>
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
            app.MapPatch("/ChangeStatusObject/{id_object:int}", async (int id_object, string status, IDatabaseFactory factory) =>
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
            /*app.MapPatch("/UpdateWorkerRequest/{id_request:int}", async (int id_request, int id_worker, IDatabaseFactory factory) =>
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
                .Produces(StatusCodes.Status400BadRequest);*/
        }
    }
}
