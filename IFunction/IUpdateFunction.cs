using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IUpdateFunction: IConnection
    {
        //Update Request
        public void UpdateRequest(Models.Request request)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Request` SET worker_id = {request.Worker_Id}, status = {request.Status}, urgency = {request.Urgency} WHERE id_request = {request.Request_Id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Update Worker
        public void UpdateWorker(Models.Worker worker)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Worker` SET surname = {worker.Surname}, name = {worker.Name}, fatherName = {worker.Fathername}," +
                $"phoneNumber = {worker.PhoneNumber}, email = {worker.Email}, role_id = {worker.Id_Role}, office_id = {worker.Id_Office}, password = {worker.Password} WHERE id_request = {worker.Id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }

        //Update Worker password
        public void UpdatePassword(Models.Worker worker)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Worker` SET password = {worker.Password} WHERE id_request = {worker.Id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Update Booking
        public void UpdateBooking(Models.Booking booking)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Object_Booking` SET date_start = {booking.Date_Start}, date_start = {booking.Date_End} WHERE id_request = {booking.Id_Booking};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
        //Update Object
        public void UpdateObject(Models.Object _object)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Object` SET balcony = {_object.Balcony}, status = {_object.Status}, kitchen = {_object.Kitchen},  WHERE id_request = {_object.Id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            database.closeConnection();
        }
    }
}
