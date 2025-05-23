using edemBack.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Data;
using System.Net.Http.Headers;

namespace edemBack.IFunction
{
    public interface ICreateFunction
    {
        //Create object +
        public static async Task createObject(Models.Object _object, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long newId;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Object 
                            (office_id, street, house, apartment, rooms, status, area, kitchen, balcony) 
                            VALUES 
                            (@Office, @Street, @House, @Apartment, @Rooms, @Status, @Area, @Kitchen, @Balcony);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Office", _object.Office_Id);
                    command.Parameters.AddWithValue("@Street", (object?)_object.Street ?? DBNull.Value);
                    command.Parameters.AddWithValue("@House", (object?)_object.House ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Apartment", (object?)_object.Apartment ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Rooms", _object.Rooms);
                    command.Parameters.AddWithValue("@Status", (object?)_object.Status ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Area", _object.Area);
                    command.Parameters.AddWithValue("@Kitchen", _object.Kitchen);
                    command.Parameters.AddWithValue("@Balcony", _object.Balcony);

                    await command.ExecuteNonQueryAsync();
                    newId = command.LastInsertedId;
                }
                await connection.CloseAsync();
            }
        }
        //Create Photos
        public static async Task createPhotos(Models.Object _object, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = "";
            }
        }
        //Create worker +
        public static async Task createWorker(Models.Worker worker, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO `Worker` 
                        (surname, name, fatherName, phoneNumber, email, birthday, role_id, office_id, password) 
                        VALUES 
                        (@Surname, @Name, @Fathername, @PhoneNumber, @Email, @Birthday, @Id_Role, @Id_Office, @Password);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Surname", worker.Surname);
                    command.Parameters.AddWithValue("@Name", worker.Name);
                    command.Parameters.AddWithValue("@Fathername", worker.Fathername);
                    command.Parameters.AddWithValue("@PhoneNumber", worker.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", worker.Email);
                    command.Parameters.Add("@Birthday", MySqlDbType.Date).Value = worker.Birthday.Date;
                    command.Parameters.AddWithValue("@Id_Role", worker.Id_Role);
                    command.Parameters.AddWithValue("@Id_Office", worker.Id_Office);
                    command.Parameters.AddWithValue("@Password", PasswordHasher.Hash(worker.Password));

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
            
        }

        //Create Office +
        public static async Task createOffice(Models.Office office, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Office 
                            (street, house) 
                            VALUES 
                            (@Street, @House);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Street", office.Street);
                    command.Parameters.AddWithValue("@House", office.House);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //Create additional info
        public static async Task createAdditional(Models.Additionally additionally, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Additionally 
                            (name_parametr, value, object_id) 
                            VALUES 
                            (@Parametr, @Value, @ID_Object);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID_Object", additionally.ID_object);
                    command.Parameters.AddWithValue("@Parametr", additionally.Parametr);
                    command.Parameters.AddWithValue("@Value", additionally.Value);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }
        //create role +
        public static async Task createRole(Models.Role role, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long id;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Role 
                            (name, salary, level_important) 
                            VALUES 
                            (@Name, @Salary, @Level);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", role.Name);
                    command.Parameters.AddWithValue("@Salary", role.Salary);
                    command.Parameters.AddWithValue("@Level", role.LevelImportant);

                    await command.ExecuteNonQueryAsync();
                    id = command.LastInsertedId;
                }
                await connection.CloseAsync();
            }
            if (role.add_Parametrs != null)
            {
                foreach (var add in role.add_Parametrs)
                {
                    using (var connection = database.CreateNewConnection())
                    {
                        await connection.OpenAsync();
                        string query = @"INSERT INTO Add_Parametrs_Role
                            (role_id, parametr) VALUES
                            (@Role, @Parametr);";
                        using (var command = new MySqlCommand(query, connection))
                        {

                            command.Parameters.AddWithValue("@Role", id);
                            command.Parameters.AddWithValue("@Parametr", add.Parametr);

                            await command.ExecuteNonQueryAsync();
                        }
                        await connection.CloseAsync();
                    }
                }
            }
        }
        //create request
        public static async Task createRequest(Models.Request request, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long request_id = -1;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                
                string query = @"INSERT INTO Request 
                            (type_id, worker, object_id, status, urgency, description) 
                            VALUES 
                            (@Type_Id, @Worker, @Object_Id, @Status, @Urgency, @Description);";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Type_Id", request.Type_Work);
                    command.Parameters.AddWithValue("@Worker", (object?)request.Worker ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Object_Id", request.Object_Id);
                    if (request.Status != "" || request.Status != null)
                    {
                        command.Parameters.AddWithValue("@Status", request.Status);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Status", 1);
                    }
                    command.Parameters.AddWithValue("@Urgency", Convert.ToInt32(request.Urgency));
                    command.Parameters.AddWithValue("@Description", request.Description);

                    await command.ExecuteNonQueryAsync();
                    request_id = command.LastInsertedId;
                }
                await connection.CloseAsync();
            }
            if (request.Workers_id != null || request.Workers_id.Count != 0)
            {
                foreach(var worker in request.Workers_id)
                {
                    using (var connection = database.CreateNewConnection())
                    {
                        await connection.OpenAsync();
                        string query = @"INSERT INTO Request_Worker
                                (request_id, worker_id)
                                VALUES
                                (@Request, @Worker);";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Request", request_id);
                            command.Parameters.AddWithValue(@"Worker", worker);

                            await command.ExecuteNonQueryAsync();
                        }
                        await connection.CloseAsync();
                    }
                }
                using (var connection = database.CreateNewConnection())
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE Request
                                SET worker = true, status = '2' WHERE id_request = @Request;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Request", request_id);

                        await command.ExecuteNonQueryAsync();
                    }
                    await connection.CloseAsync();
                }

            }
        }

        //create booking
        public static async Task<bool> createBooking(Models.Booking booking, IDatabaseFactory factory)
        {
            var database = factory.Create();
            string query = @"INSERT INTO Object_Booking 
                        (object_id, date_start, date_end, status) 
                        VALUES 
                        (@Object_Id, @Date_Start, @Date_End, @Status);";

            string newQuery = @"SELECT id_booking, object_id, date_start, date_end, status FROM Object_Booking INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status` WHERE object_id = @Object_Id ; ";
            List<Models.Booking> bookings = new List<Models.Booking>();
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
                using (var newCommand = new MySqlCommand(newQuery, connection))
                {
                    newCommand.Parameters.AddWithValue("@Object_Id", booking.Object_id);

                    try
                    {
                        using (var reader = await newCommand.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                Models.Booking oneBooking = new Models.Booking
                                {
                                    Id_Booking = reader.IsDBNull(0) ? 0 : reader.GetInt32(0),
                                    Object_id = reader.IsDBNull(1) ? 0 : reader.GetInt32(1),
                                    Date_Start = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                                    Date_End = reader.IsDBNull(3) ? DateTime.MinValue : reader.GetDateTime(3),
                                    Status = reader.IsDBNull(4) ? "" : reader.GetString(4)
                                };
                                bookings.Add(oneBooking);
                            }
                        }
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine("Ошибка MySQL: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Общая ошибка: " + ex.Message);
                    }
                }
                await connection.CloseAsync();
            }
            
            foreach (var oneBooking in bookings)
            {
                bool hasConflict = (booking.Date_Start >= oneBooking.Date_Start && booking.Date_Start < oneBooking.Date_End) ||
                    (booking.Date_End > oneBooking.Date_Start && booking.Date_End <= oneBooking.Date_End) ||
                    (booking.Date_Start <= oneBooking.Date_Start && booking.Date_End >= oneBooking.Date_End);
                
                if (hasConflict){
                    return false;
                }
            }
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Object_Id", booking.Object_id);
                    command.Parameters.Add("@Date_Start", MySqlDbType.Date).Value = booking.Date_Start;
                    command.Parameters.Add("@Date_End", MySqlDbType.Date).Value = booking.Date_End;
                    command.Parameters.AddWithValue("@Status", id_status);
                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
            return true;
        }

        //create type work
        public static async Task createTypeWork(Models.TypeWork typeWork, IDatabaseFactory factory)
        {
            var database = factory.Create();
            long id;
            using(var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Type_Work (name) VALUES (@Name);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", typeWork.Name);

                    await command.ExecuteNonQueryAsync();
                    id = command.LastInsertedId;
                }
                await connection.CloseAsync();
            }
            if (typeWork.Roles_Id != null)
            {
                foreach (int role in typeWork.Roles_Id)
                {
                    using (var connection = database.CreateNewConnection())
                    {
                        await connection.OpenAsync();
                        string query = @"INSERT INTO Work_Role (work_id, role_id) VALUES (@Work, @Role);";
                        using (var command = new MySqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@Work", id);
                            command.Parameters.AddWithValue("@Role", role);

                            await command.ExecuteNonQueryAsync();
                        }
                        await connection.CloseAsync();
                    }
                }
            }
        }

        //create Status
        public static async Task createStatus(Models.Status status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using(var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Status (name) VALUES (@name);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", status.Name);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //create status_request
        public static async Task createStatusRequest(Models.Status_Request status, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Status_request (name) VALUES (@name);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", status.Name);

                    await command.ExecuteNonQueryAsync();
                }
                await connection.CloseAsync();
            }
        }

        //Create Parametr Role
        public static async Task createParametrRole(Models.Add_Parametrs_Role parametr, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Add_Parametrs_Role (parametr, role_id) VALUES (@Parametr, @Role);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Parametr", parametr.Parametr);
                    command.Parameters.AddWithValue("@Role", parametr.Role_Id);
                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        //create work_role
        public static async Task createWorkRole(Models.Work_Role work_Role, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Work_Role (work_id, role_id) VALUES (@Work, @Role);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Work", work_Role.Work);
                    command.Parameters.AddWithValue("@Role", work_Role.Role);

                    await command.ExecuteNonQueryAsync();
                }

                await connection.CloseAsync();
            }
        }

        //create worker fo request
        public static async Task createWorkerRequest(int id_worker, int id_request, IDatabaseFactory factory)
        {
            //500 ошибка проверить
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"INSERT INTO Request_Worker (request_id, worker_id) VALUES (@Request, @Worker);";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Request", id_request);
                    command.Parameters.AddWithValue("@Worker", id_worker);

                    await command.ExecuteNonQueryAsync();
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
                    command.Parameters.AddWithValue("@Request", id_request);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    worker = int.Parse(reader[0].ToString()!) > 0;
                }
                await connection.CloseAsync();
            }
            if (worker)
            {
                using (var connection = database.CreateNewConnection())
                {
                    await connection.OpenAsync();
                    string query = @"UPDATE Request SET worker = true WHERE id_request = @Request;";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Request", id_request);

                        await command.ExecuteNonQueryAsync();
                    }

                    await connection.CloseAsync();
                }
            }
        }
    }
}
