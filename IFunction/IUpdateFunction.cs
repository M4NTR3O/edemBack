using edemBack.Models;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace edemBack.IFunction
{
    public interface IUpdateFunction
    {
        //Переписать как CREATE методы
        //Update Request
        public static async Task updateRequest(Models.Request request, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE `Request` SET object_id = @Object_Id,
                                                                worker_id = @WorkerId, status = @Status,
                                                                urgency = @Urgency, description = @Description,  WHERE id_request = @RequestId;", connection))
                {
                    command.Parameters.AddWithValue("@Object_Id", request.Object_Id);
                    command.Parameters.AddWithValue("@WorkerId", request.Worker_Id);
                    command.Parameters.AddWithValue("@Status", request.Status);
                    command.Parameters.AddWithValue("@Urgency", request.Urgency);
                    command.Parameters.AddWithValue("@Description", request.Description);
                    command.Parameters.AddWithValue("@RequestId", request.Request_Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Update Worker
        public static async Task updateWorker(Models.Worker worker, IDatabaseFactory factory)
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
                    //command.Parameters.AddWithValue("@Password", worker.Password);
                    command.Parameters.AddWithValue("@Id", worker.Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update Worker password 
        public static async Task updatePassword(int id, string password, IDatabaseFactory factory)
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
        public static async Task updateBooking(Models.Booking booking, IDatabaseFactory factory)
        {
            var database = factory.Create();
            
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(@"UPDATE Object_Booking SET date_start = @Date_start, date_end = @Date_end, status = @Status WHERE id_booking = @Id_Booking", connection))
                {
                    command.Parameters.AddWithValue("@Date_start", booking.Date_Start.Date);
                    command.Parameters.AddWithValue("@Date_end", booking.Date_End.Date);
                    command.Parameters.AddWithValue("@Status", booking.Status);
                    command.Parameters.AddWithValue("@Id_Booking", booking.Id_Booking);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Update Object
        public static async Task updateObject(Models.Object _object, IDatabaseFactory factory)
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
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //update Status
        public static async Task updateStatus(Models.Status status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE `Status` SET name = @name WHERE id_status = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", status.Name);
                    command.Parameters.AddWithValue("@id", status.Id);
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update status_request
        public static async Task updateStatusRequest(Models.Status_Request status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE `Status_request` SET name = @name WHERE id = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", status.Name);
                    command.Parameters.AddWithValue("@id", status.Id_Status);
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update Office
        public static async Task updateOffice(Models.Office office, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Office SET street = @Street, house = @House WHERE id_office = @id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Street", office.Street);
                    command.Parameters.AddWithValue("@House", office.House);
                    command.Parameters.AddWithValue("@id", office.Office_Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update Role
        public static async Task updateRole(Models.Role role, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long id;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Role SET name = @Name, salary = @Salary WHERE id_role = @id;";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", role.Name);
                    command.Parameters.AddWithValue("@Salary", role.Salary);
                    command.Parameters.AddWithValue("@id", role.Role_Id);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Update Type_work
        public static async Task updateTypeWork(Models.TypeWork typeWork, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long id;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Type_Work SET name = @Name WHERE id_work = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", typeWork.Name);
                    command.Parameters.AddWithValue("@id", typeWork.Id_Work);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Change Status Booking
        public static async Task changeStatusBooking(int id_booking, int status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Object_Booking SET status = @Status WHERE id_booking = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@id", id_booking);
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
            DateTime start; DateTime finish; int _object;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"SELECT object_id, date_start, date_end FROM Object_Booking WHERE id_booking = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id_booking);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    _object = int.Parse(reader[0].ToString()!);
                    start = DateTime.Parse(reader[1].ToString()!);
                    finish = DateTime.Parse(reader[2].ToString()!);
                }

                await connection.CloseAsync();
            }
            if ((DateTime.Now > start) && (DateTime.Now < finish))
            {
                using (var connection = database.CreateNewConnection())
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE Object SET status = @Status WHERE id_object = @id;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Status", status);
                        command.Parameters.AddWithValue("@id", _object);
                        await command.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
            }
        }

        //Change status for object
        public static async Task changeStatusObject(int id_object, int status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Object SET status = @Status WHERE id_object = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@id", id_object);
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        //Change Status Request
        public static async Task changeStatusRequest(int id_request, int status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Request SET status = @Status WHERE id_request = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@id", id_request);
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        //Add or change worker of request
        public static async Task updateWorkerRequest(int id_request, int id_worker, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"UPDATE Request SET worker_id = @Worker WHERE id_request = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Worker", id_worker);
                    command.Parameters.AddWithValue("@id", id_request);
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }
    }
}
