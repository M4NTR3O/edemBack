using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IDeleteFunctions: IConnection
    {
        //Delete Object
        public static async Task DeleteObject(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Object` WHERE id_object = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            await database.closeAsyncConnection();
        }
        //Delete Worker
        public static async Task DeleteWorker(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Worker` WHERE id_worker = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            await database.closeAsyncConnection();
        }
        //Delete Office
        public static async Task DeleteOffice(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Office` WHERE id_office = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            await database.closeAsyncConnection();
        }
        //Delete Booking
        public static async Task DeleteBooking(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Object_Booking` WHERE id_booking = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            await database.closeAsyncConnection();
        }
        //Delete Request
        public static async Task DeleteRequest(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Request` WHERE id_request = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            await database.closeAsyncConnection();
        }
    }
}
