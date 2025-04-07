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
            using (var command = new MySqlCommand(@"UPDATE `Worker` SET password = @Password WHERE id_worker = @Id_worker;", database.connection))
            {
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Id_worker", id);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
        //Update Booking
        public static async Task UpdateBooking(Models.Booking booking)
        {
            await database.openAsyncConnection();
            using (var command = new MySqlCommand(@"UPDATE Object_Booking SET date_start = @Date_start, date_end = @Date_end WHERE id_booking = @Id_Booking", database.connection))
            {
                command.Parameters.AddWithValue("@Date_start", booking.Date_Start.Date);
                command.Parameters.AddWithValue("@Date_end", booking.Date_End.Date);
                command.Parameters.AddWithValue("@Id_Booking", booking.Id_Booking);
            }
            await database.closeAsyncConnection();
        }
        //Update Object
        public static async Task UpdateObject(Models.Object _object)
        {
            await database.openAsyncConnection();
            using(var command = new MySqlCommand(@"UPDATE `Object` SET balcony = @Balcony, status = @Status, kitchen = @Kitchen WHERE id_object = @Id_object;", database.connection))
            {
                command.Parameters.AddWithValue("@Balcony", _object.Balcony);
                command.Parameters.AddWithValue("@Status", _object.Status);
                command.Parameters.AddWithValue("@Kitchen", _object.Kitchen);
                command.Parameters.AddWithValue("@Id_object", _object.Id);
                await command.ExecuteReaderAsync();
            }
            await database.closeAsyncConnection();
        }
    }
}
