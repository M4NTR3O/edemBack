using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IDeleteFunctions: IConnection
    {
        //Delete Object
        public void DeleteObject(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Object` WHERE id_object = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Delete Worker
        public void DeleteWorker(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Worker` WHERE id_worker = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Delete Office
        public void DeleteOffice(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Office` WHERE id_office = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Delete Booking
        public void DeleteBooking(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Object_Booking` WHERE id_booking = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Delete Request
        public void DeleteRequest(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"DELETE FROM `Request` WHERE id_request = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
    }
}
