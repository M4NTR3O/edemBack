using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface ICreateFunction: IConnection
    {
        //Create object +
        public static async Task createObject(Models.Object _object)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Object 
                        (office_id, street, house, apartment, rooms, status, area, kitchen, balcony) 
                        VALUES 
                        (@Office, @Street, @House, @Apartment, @Rooms, @Status, @Area, @Kitchen, @Balcony);";

            using (var command = new MySqlCommand(query, database.connection))
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
            }
            await database.closeAsyncConnection();
        }
        //Create worker
        public static async Task createWorker(Models.Worker worker)
        {
            try
            {
                await database.openAsyncConnection();
                if (database.connection.State != System.Data.ConnectionState.Open)
                {
                    throw new Exception("Соединение не открылось!");
                }

                string query = @"INSERT INTO `Worker` 
                         (surname, name, fatherName, phoneNumber, email, birthday, role_id, office_id, password) 
                         VALUES 
                         (@Surname, @Name, @Fathername, @PhoneNumber, @Email, @Birthday, @Id_Role, @Id_Office, @Password);";

                using (var command = new MySqlCommand(query, database.connection))
                {
                    command.Parameters.AddWithValue("@Surname", worker.Surname);
                    command.Parameters.AddWithValue("@Name", worker.Name);
                    command.Parameters.AddWithValue("@Fathername", worker.Fathername);
                    command.Parameters.AddWithValue("@PhoneNumber", worker.PhoneNumber);
                    command.Parameters.AddWithValue("@Email", worker.Email);
                    command.Parameters.Add("@Birthday", MySqlDbType.Date).Value = worker.Birthday.Date;
                    command.Parameters.AddWithValue("@Id_Role", worker.Id_Role);
                    command.Parameters.AddWithValue("@Id_Office", worker.Id_Office);
                    command.Parameters.AddWithValue("@Password", worker.Password);

                    await command.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                // Логгирование ошибки для диагностики
                Console.WriteLine("MySQL Exception: " + ex.Message);
                throw;
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("ObjectDisposedException: " + ex.Message);
                throw;
            }
            finally
            {
                await database.closeAsyncConnection();
            }
        }

        //Create Office +
        public static async Task createOffice(Models.Office office)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Office 
                        (street, house) 
                        VALUES 
                        (@Street, @House);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Street", office.Street);
                command.Parameters.AddWithValue("@House", office.House);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
        //Create additional info
        public static async Task createAdditional(Models.Additionally additionally)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Additionally 
                        (name_parametr, value, object_id) 
                        VALUES 
                        (@Parametr, @Value, @ID_Object);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@ID_Object", additionally.ID_object);
                command.Parameters.AddWithValue("@Parametr", additionally.Parametr);
                command.Parameters.AddWithValue("@Value", additionally.Value);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
        //create role +
        public static async Task createRole(Models.Role role)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Role 
                        (name, salary) 
                        VALUES 
                        (@Name, @Salary);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Name", role.Name);
                command.Parameters.AddWithValue("@Salary", role.Salary);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
        //create request
        public static async Task createRequest(Models.Request request)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Request 
                        (role_id, worker_id, object_id, status, urgency) 
                        VALUES 
                        (@Role_Id, @Worker_Id, @Object_Id, @Status, @Urgency);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Role_Id", request.Role_Id);
                command.Parameters.AddWithValue("@Worker_Id", (object?)request.Worker_Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@Object_Id", request.Object_Id);
                command.Parameters.AddWithValue("@Status", request.Status);
                command.Parameters.AddWithValue("@Urgency", Convert.ToInt32(request.Urgency));

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }

        //create booking
        public static async Task createBooking(Models.Booking booking)
        {
            await database.openAsyncConnection();
            string query = @"INSERT INTO Object_Booking 
                        (object_id, date_start, date_end) 
                        VALUES 
                        (@Object_Id, @Date_Start, @Date_End);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Object_Id", booking.Object_id);
                command.Parameters.AddWithValue("@Date_Start", booking.Date_Start);
                command.Parameters.AddWithValue("@Urgency", booking.Date_End);

                await command.ExecuteNonQueryAsync();
            }
            await database.closeAsyncConnection();
        }
    }
}
