using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IDeleteFunctions
    {
        //Delete Object
        public static async Task DeleteObject(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        public static async Task DeleteWorker(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        public static async Task DeleteOffice(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        public static async Task DeleteBooking(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        public static async Task DeleteRole(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        public static async Task DeleteRequest(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
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
        //Delete Status
        public static async Task DeleteStatus(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Status` WHERE `id_status`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Status_Request
        public static async Task DeleteStatusRequest(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Status_request` WHERE `id`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Type_Work
        public static async Task DeleteTypeWork(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using(var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Type_Work` WHERE `id_work`=@id;";
                using(var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
