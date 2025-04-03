using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface IUpdateFunction: IConnection
    {
        //Переписать как CREATE методы
        //Update Request
        public static async Task UpdateRequest(Models.Request request)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand(@"UPDATE `Request` SET worker_id = @WorkerId, status = @Status, urgency = @Urgency WHERE id_request = @RequestId;", database.connection))
            {
                command.Parameters.AddWithValue("@WorkerId", request.Worker_Id);
                command.Parameters.AddWithValue("@Status", request.Status);
                command.Parameters.AddWithValue("@Urgency", request.Urgency);
                command.Parameters.AddWithValue("@RequestId", request.Request_Id);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
        //Update Worker
        public static async Task UpdateWorker(Models.Worker worker)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Worker` SET surname = {worker.Surname}, name = {worker.Name}, fatherName = {worker.Fathername}," +
                $"phoneNumber = {worker.PhoneNumber}, email = {worker.Email}, role_id = {worker.Id_Role}, office_id = {worker.Id_Office}, password = {worker.Password} WHERE id_worker = {worker.Id};";
            using (var command = new MySqlCommand(@"UPDATE `Worker` SET surname = @Surname, name = @Name, fatherName = @Fathername, phoneNumber = @PhoneNumber, email = @Email, role_id = @Role_Id, office_id = @Office_Id, password = @Password WHERE id_worker = @Id;", database.connection))
            {
                command.Parameters.AddWithValue("@Surname", worker.Surname);
                command.Parameters.AddWithValue("@Name", worker.Name);
                command.Parameters.AddWithValue("@Fathername", worker.Fathername);
                command.Parameters.AddWithValue("@PhoneNumber", worker.PhoneNumber);
                command.Parameters.AddWithValue("@Email", worker.Email);
                command.Parameters.AddWithValue("@Role_Id", worker.Id_Role);
                command.Parameters.AddWithValue("@Office_Id", worker.Id_Office);
                command.Parameters.AddWithValue("@Password", worker.Password);
                command.Parameters.AddWithValue("@Id", worker.Id);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }

        //Update Worker password
        public static async Task UpdatePassword(int id, string password)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Worker` SET password = {password} WHERE id_request = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            await database.closeAsyncConnection();
        }
        //Update Booking
        public static async Task UpdateBooking(Models.Booking booking)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Object_Booking` SET date_start = {booking.Date_Start}, date_start = {booking.Date_End} WHERE id_booking = {booking.Id_Booking};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            await database.closeAsyncConnection();
        }
        //Update Object
        public static async Task UpdateObject(Models.Object _object)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"UPDATE `Object` SET balcony = {_object.Balcony}, status = {_object.Status}, kitchen = {_object.Kitchen},  WHERE id_object = {_object.Id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            await database.closeAsyncConnection();
        }
    }
}
