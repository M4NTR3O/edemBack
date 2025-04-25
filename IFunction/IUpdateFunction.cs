using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace edemBack.IFunction
{
    public interface IUpdateFunction
    {
        //Переписать как CREATE методы
        //Update Request
        public static async Task UpdateRequest(Models.Request request, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE `Request` SET role_id = @Role_Id, object_id = @Object_Id, worker_id = @WorkerId, status = @Status, urgency = @Urgency WHERE id_request = @RequestId;", connection))
                {
                    command.Parameters.AddWithValue("@Role_Id", request.Role_Id);
                    command.Parameters.AddWithValue("@Object_Id", request.Object_Id);
                    command.Parameters.AddWithValue("@WorkerId", request.Worker_Id);
                    command.Parameters.AddWithValue("@Status", request.Status);
                    command.Parameters.AddWithValue("@Urgency", request.Urgency);
                    command.Parameters.AddWithValue("@RequestId", request.Request_Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Update Worker
        public static async Task UpdateWorker(Models.Worker worker, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE `Worker` SET surname = @Surname, name = @Name, fatherName = @Fathername, phoneNumber = @PhoneNumber, email = @Email, birthday = @Birthday, role_id = @Role_Id, office_id = @Office_Id, password = @Password WHERE id_worker = @Id;", connection))
                {
                    command.Parameters.AddWithValue("@Surname", worker.Surname);
                    command.Parameters.AddWithValue("@Name", worker.Name);
                    command.Parameters.AddWithValue("@Fathername", worker.Fathername);
                    command.Parameters.AddWithValue("@PhoneNumber", worker.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", worker.Email);
                    command.Parameters.Add("@Birthday", MySqlDbType.Date).Value = worker.Birthday.Date;
                    command.Parameters.AddWithValue("@Role_Id", worker.Id_Role);
                    command.Parameters.AddWithValue("@Office_Id", worker.Id_Office);
                    //Сделать проверку на пароль, чтобы не двоить хэширование (возможно запретить изменение пароля в update)
                    command.Parameters.AddWithValue("@Password", worker.Password);
                    command.Parameters.AddWithValue("@Id", worker.Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update Worker password не проверено
        public static async Task UpdatePassword(int id, string password, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                string hashPass = PasswordHasher.Hash(password);
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE `Worker` SET password = @Password WHERE id_worker = @Id_worker;", connection))
                {
                    command.Parameters.AddWithValue("@Password", hashPass);
                    command.Parameters.AddWithValue("@Id_worker", id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Update Booking
        public static async Task UpdateBooking(Models.Booking booking, IDatabaseFactory factory)
        {
            var database = factory.Create();
            int id_status = 0;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command1 = new MySqlCommand(@"SELECT `id_status` FROM `Status` WHERE `name` = @Status", connection))
                {
                    command1.Parameters.AddWithValue("@Status", booking.Status);
                    var reader = await command1.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    id_status = int.Parse(reader[0].ToString()!);
                }
                await connection.CloseAsync();
            }
            
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE Object_Booking SET date_start = @Date_start, date_end = @Date_end, status = @Status WHERE id_booking = @Id_Booking", connection))
                {
                    command.Parameters.AddWithValue("@Date_start", booking.Date_Start.Date);
                    command.Parameters.AddWithValue("@Date_end", booking.Date_End.Date);
                    command.Parameters.AddWithValue("@Status", id_status);
                    command.Parameters.AddWithValue("@Id_Booking", booking.Id_Booking);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Update Object
        public static async Task UpdateObject(Models.Object _object, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using(var command = new MySqlCommand(@"UPDATE `Object` SET balcony = @Balcony, status = @Status, kitchen = @Kitchen WHERE id_object = @Id_object;", connection))
                {
                    command.Parameters.AddWithValue("@Balcony", _object.Balcony);
                    command.Parameters.AddWithValue("@Status", _object.Status);
                    command.Parameters.AddWithValue("@Kitchen", _object.Kitchen);
                    command.Parameters.AddWithValue("@Id_object", _object.Id);
                    await command.ExecuteReaderAsync();
                }
                await connection.CloseAsync();
            }
        }
    }
}
