using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IUpdateFunction: IConnection
    {
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
        //Delete Worker

        //Delete Booking

        //Delete Object
    }
}
