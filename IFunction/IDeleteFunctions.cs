using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IDeleteFunctions: IConnection
    {
        //Delete Object
        public static async Task DeleteObject(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Object` WHERE id_object= {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Worker
        public static async Task DeleteWorker(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Worker` WHERE id_worker= {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Office
        public static async Task DeleteOffice(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Office` WHERE id_office= {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Booking
        public static async Task DeleteBooking(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Object_Booking` WHERE id_booking= {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Role
        public static async Task DeleteRole(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Role` WHERE id_role= {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Request
        public static async Task DeleteRequest(int id)
        {
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand($"DELETE FROM `Request` WHERE id_request = {id};", connection))
                {
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
