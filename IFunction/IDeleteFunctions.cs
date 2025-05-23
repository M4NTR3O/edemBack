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

        //Delete AddParametrRole
        public static async Task DeleteAddParametrRole(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Add_Parametrs_Role` WHERE `id`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Delete Work_Role
        public static async Task DeleteWorkRole(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Work_Role` WHERE `id_work`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Delete Request_Worker
        public static async Task DeleteRequestWorker(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            int request = -1;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"SELECT `request_id` FROM `Request_Worker` WHERE `id_request_worker`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    request = int.Parse(reader[0].ToString()!);
                }
                await connection.CloseAsync();
            }
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"DELETE FROM `Request_Worker` WHERE `id_request_worker`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
            bool worker = false;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"SELECT COUNT(worker_id) FROM Request_Worker WHERE request_id = @Request;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Request", request);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    worker = int.Parse(reader[0].ToString()!) > 0;
                }
                await connection.CloseAsync();
            }
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Request
                                SET worker = @Worker WHERE id_request = @Request;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Request", request);
                    command.Parameters.AddWithValue("@Worker", worker);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
