using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface ICreateFunction: IConnection
    {
        //Create object
        public static void createObject(Models.Object _object)
        {
            database.openConnection();
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

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }
        //Create worker
        public static void createWorker(Models.Worker worker)
        {
            database.openConnection();
            string query = @"INSERT INTO Object 
                        (surname, name, fatherName, phoneNumber, email, birthday, role_id, office_id, login, password) 
                        VALUES 
                        (@Surname, @Name, @Fathername, @PhoneNumber, @Email, @Birthday, @Id_Role, @Id_Office, @Login, @Password);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Surname", worker.Surname);
                command.Parameters.AddWithValue("@Name", worker.Name);
                command.Parameters.AddWithValue("@Fathername", worker.Fathername);
                command.Parameters.AddWithValue("@PhoneNumber", worker.PhoneNumber);
                command.Parameters.AddWithValue("@Email", worker.Email);
                //Скорее всего проблема, протестировать
                command.Parameters.AddWithValue("@Birthday", worker.Birthday);
                command.Parameters.AddWithValue("@Id_Role", worker.Id_Role);
                command.Parameters.AddWithValue("@Id_Office", worker.Id_Office);
                command.Parameters.AddWithValue("@Login", worker.Login);
                command.Parameters.AddWithValue("@Password", worker.Password);

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }

        //Create filial
        public static void createOffice(Models.Office office)
        {
            database.openConnection();
            string query = @"INSERT INTO Office 
                        (street, house) 
                        VALUES 
                        (@Street, @House);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Street", (object?)office.Street ?? DBNull.Value);
                command.Parameters.AddWithValue("@House", (object?)office.House ?? DBNull.Value);

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }
        //Create additional material
        /*
        public static void createAdditionalMaterial(Models.Additionally additionally)
        {
            database.openConnection();
            string query = @"INSERT INTO Object 
                        (office_id, street, house, apartment, rooms, status, area, kitchen, balcony) 
                        VALUES 
                        (@Office, @Street, @House, @Apartment, @Rooms, @Status, @Area, @Kitchen, @Balcony);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Office", additionally.Office_Id);
                command.Parameters.AddWithValue("@Street", (object?)additionally.Street ?? DBNull.Value);
                command.Parameters.AddWithValue("@House", (object?)_object.House ?? DBNull.Value);
                command.Parameters.AddWithValue("@Apartment", (object?)_object.Apartment ?? DBNull.Value);
                command.Parameters.AddWithValue("@Rooms", _object.Rooms);
                command.Parameters.AddWithValue("@Status", (object?)_object.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Area", _object.Area);
                command.Parameters.AddWithValue("@Kitchen", _object.Kitchen);
                command.Parameters.AddWithValue("@Balcony", _object.Balcony);

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }*/
        //create role
        public static void createRole(Models.Role role)
        {
            database.openConnection();
            string query = @"INSERT INTO Role 
                        (name, salary) 
                        VALUES 
                        (@Name, @Salary);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Street", (object?)role.Name ?? DBNull.Value);
                command.Parameters.AddWithValue("@House", role.Salary);

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }
        //create request
        public static void createRequest(Models.Request request)
        {
            database.openConnection();
            string query = @"INSERT INTO Request 
                        (role_id, worker_id, object_id, status, urgency) 
                        VALUES 
                        (@Role_Id, @Worker_Id, @Object_Id, @Status, @Urgency);";

            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.AddWithValue("@Role_Id", request.Role_Id);
                command.Parameters.AddWithValue("@Worker_Id", (object?)request.Worker_Id ?? DBNull.Value);
                command.Parameters.AddWithValue("@Object_Id", request.Object_Id);
                command.Parameters.AddWithValue("@Status", (object?)request.Status ?? DBNull.Value);
                command.Parameters.AddWithValue("@Urgency", Convert.ToInt32(request.Urgency));

                command.ExecuteNonQuery();
            }
            database.closeConnection();
        }
    }
}
