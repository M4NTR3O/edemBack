using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IDeleteFunctions: IConnection
    {
        //Delete Object
        public static async Task DeleteObject(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Object` WHERE id_object= {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
        //Delete Worker
        public static async Task DeleteWorker(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Worker` WHERE id_worker= {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
        //Delete Office
        public static async Task DeleteOffice(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Office` WHERE id_office= {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
        //Delete Booking
        public static async Task DeleteBooking(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Object_Booking` WHERE id_booking= {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
        //Delete Role
        public static async Task DeleteRole(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Role` WHERE id_role= {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
        //Delete Request
        public static async Task DeleteRequest(int id)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand($"DELETE FROM `Request` WHERE id_request = {id};", database.connection))
            {
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
    }
}
