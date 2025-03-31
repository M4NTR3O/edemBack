using MySql.Data.MySqlClient;
using edemBack.Models;

namespace edemBack.IFunction
{
    public interface IGetFunction: IConnection
    {
        //Get all object
        public static List<Models.Object> getAllObject()
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object`;";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            List<Models.Object> objects = new List<Models.Object>();
            while (reader.Read())
            {
                Models.Object oneObject = new Models.Object
                {
                    Id = int.Parse(reader[0].ToString()!),
                    Office_Id = int.Parse(reader[1].ToString()!),
                    Street = reader[2].ToString()!,
                    House = reader[3].ToString()!,
                    Apartment = reader[4].ToString()!,
                    Rooms = int.Parse(reader[5].ToString()!),
                    Status = reader[6].ToString()!,
                    Area = decimal.Parse(reader[7].ToString()!),
                    Kitchen = bool.Parse(reader[8].ToString()!),
                    Balcony = bool.Parse(reader[9].ToString()!)
                };
                objects.Add(oneObject);
            }
            database.closeConnection();
            return objects;
        }

        //Get one object
        public static List<Models.Object> getObject(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object` WHERE `id_object` = {id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            List<Models.Object> objects = new List<Models.Object>();
            while (reader.Read())
            {
                Models.Object oneObject = new Models.Object
                {
                    Id = int.Parse(reader[0].ToString()!),
                    Office_Id = int.Parse(reader[1].ToString()!),
                    Street = reader[2].ToString()!,
                    House = reader[3].ToString()!,
                    Apartment = reader[4].ToString()!,
                    Rooms = int.Parse(reader[5].ToString()!),
                    Status = reader[6].ToString()!,
                    Area = decimal.Parse(reader[7].ToString()!),
                    Kitchen = bool.Parse(reader[8].ToString()!),
                    Balcony = bool.Parse(reader[9].ToString()!)
                };
                objects.Add(oneObject);
            }
            database.closeConnection();
            return objects;
        }

        //Get Workers
        public static List<Worker> getAllWorker()
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `login`, `password` FROM `Worker`;";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            List<Worker> workers = new List<Worker>();
            while (reader.Read())
            {
                Models.Worker worker = new Models.Worker
                {
                    Id = int.Parse(reader[0].ToString()!),
                    Surname = reader[1].ToString()!,
                    Name = reader[2].ToString()!,
                    Fathername = reader[3].ToString()!,
                    PhoneNumber = reader[4].ToString()!,
                    Email = reader[5].ToString()!,
                    Birthday = DateTime.Parse(reader[6].ToString()!),
                    Id_Role = int.Parse(reader[7].ToString()!),
                    Id_Office = int.Parse(reader[8].ToString()!),
                    Login = reader[9].ToString()!,
                    Password = reader[10].ToString()!
                };
                workers.Add(worker);
            }
            database.closeConnection();
            return workers;
        }
        //Get 1 worker
        public static List<Worker> getWorker(int id)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `login`, `password` FROM `Worker` WHERE `id_worker`={id};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            List<Worker> workers = new List<Worker>();
            while (reader.Read())
            {
                Models.Worker worker = new Models.Worker
                {
                    Id = int.Parse(reader[0].ToString()!),
                    Surname = reader[1].ToString()!,
                    Name = reader[2].ToString()!,
                    Fathername = reader[3].ToString()!,
                    PhoneNumber = reader[4].ToString()!,
                    Email = reader[5].ToString()!,
                    Birthday = DateTime.Parse(reader[6].ToString()!),
                    Id_Role = int.Parse(reader[7].ToString()!),
                    Id_Office = int.Parse(reader[8].ToString()!),
                    Login = reader[9].ToString()!,
                    Password = reader[10].ToString()!
                };
                workers.Add(worker);
            }
            database.closeConnection();
            return workers;
        }

        //Get Request with filter
        public static List<Models.Request> getAllRequest()
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_request`, `role_id`, `worker_id`, `object_id`, `status`, `urgency` FROM `Request`;";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            List<Models.Request> requests = new List<Models.Request>();
            while (reader.Read())
            {
                Models.Request oneRequest = new Models.Request
                {
                    Request_Id = int.Parse(reader[0].ToString()!),
                    Role_Id = int.Parse(reader[1].ToString()!),
                    Worker_Id = (reader[2].ToString()! != null ? int.Parse(reader[2].ToString()!) : null),
                    Object_Id = int.Parse(reader[3].ToString()!),
                    Status = reader[4].ToString()!,
                    Urgency = bool.Parse(reader[5].ToString()!)
                };
                requests.Add(oneRequest);
            }
            database.closeConnection();
            return requests;
        }

        //Get additional material

        //Get Access user
        public static int getAuth(string login, string password)
        {
            database.openConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = @$"SELECT id_worker, login, password FROM Worker WHERE login = {login};";
            mysql_query.Connection = database.connection;
            MySqlDataReader reader = mysql_query.ExecuteReader();
            if (reader.Read())
            {  
                if (reader[2].ToString() == password)
                {
                    int id = Convert.ToInt32(reader[0].ToString()!);
                    database.closeConnection();
                    return id;
                }
                else
                {
                    database.closeConnection();
                    return -1;
                }
            }
            else
            {
                database.closeConnection();
                return -2;
            }
        }
    }
}
