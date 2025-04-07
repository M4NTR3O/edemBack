using MySql.Data.MySqlClient;
using edemBack.Models;

namespace edemBack.IFunction
{
    public interface IGetFunction: IConnection
    {
        //Get all object
        public static async Task<List<Models.Object>> getAllObject()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object`;";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Object> objects = new List<Models.Object>();
            while (await reader.ReadAsync())
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
            await database.closeAsyncConnection();
            return objects;
        }

        //Get one object
        public static async Task<List<Models.Object>> getObject(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object` WHERE `id_object` = {id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Object> objects = new List<Models.Object>();
            while (await reader.ReadAsync())
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
            await database.closeAsyncConnection();
            return objects;
        }

        //Get Workers
        public static async Task<List<Worker>> getAllWorker()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker`;";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Worker> workers = new List<Worker>();
            while (await reader.ReadAsync())
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
                    Password = reader[9].ToString()!
                };
                workers.Add(worker);
            }
            await database.closeAsyncConnection();
            return workers;
        }
        //Get 1 worker
        public static async Task<List<Worker>> getWorker(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker` WHERE `id_worker`={id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Worker> workers = new List<Worker>();
            while (await reader.ReadAsync())
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
                    Password = reader[9].ToString()!
                };
                workers.Add(worker);
            }
            await database.closeAsyncConnection();
            return workers;
        }

        public static async Task<Role> getRole(int id)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = $"SELECT `id_role`, `name`, `salary` FROM `Role` WHERE `id_role`={id};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Role> roles = new List<Role>();
            while (await reader.ReadAsync())
            {
                Models.Role oneRole = new Role
                {
                    Role_Id = int.Parse(reader[0].ToString()!),
                    Name = reader[1].ToString()!,
                    Salary = int.Parse(reader[2].ToString()!)
                };
                roles.Add(oneRole);
            }
            await database.closeAsyncConnection();
            return roles[0];
        }

        //Get Request with filter
        public static async Task<List<Models.Request>> getAllRequest()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_request`, `role_id`, `worker_id`, `object_id`, `status`, `urgency` FROM `Request`;";
            mysql_query.Connection = database.connection;
            List<Models.Request> requests = new List<Models.Request>();
            using (var reader = await mysql_query.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
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
            }
            await database.closeAsyncConnection();
            return requests;
        }
        //Get Booking
        public static async Task<List<Models.Booking>> getAllBooking()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_booking`, `object_id`, `date_start`, `date_end` FROM `Object_Booking`;";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Booking> booking = new List<Models.Booking>();
            while (reader.Read())
            {
                Models.Booking oneBooking = new Models.Booking
                {
                    Id_Booking = int.Parse(reader[0].ToString()!),
                    Object_id = int.Parse(reader[1].ToString()!),
                    Date_Start = DateTime.Parse(reader[2].ToString()!),
                    Date_End = DateTime.Parse(reader[3].ToString()!)
                };
                booking.Add(oneBooking);
            }
            await database.closeAsyncConnection();
            return booking;
        }

        //Get Roles
        public static async Task<List<Models.Role>> getAllRoles()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_role`, `name`, `salary` FROM `Role`;";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Role> roles = new List<Role>();
            while(await reader.ReadAsync())
            {
                Models.Role oneRole = new Role
                {
                    Role_Id = int.Parse(reader[0].ToString()!),
                    Name = reader[1].ToString()!,
                    Salary = int.Parse(reader[2].ToString()!)
                };
                roles.Add(oneRole);
            }
            await database.closeAsyncConnection();
            return roles;
        }
        //Get additional material

        //Get Office
        public static async Task<List<Models.Office>> getAllOffice()
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = "SELECT `id_office`, `street`, `house` FROM `Office`;";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            List<Models.Office> offices = new List<Models.Office>();
            while (reader.Read())
            {
                Models.Office oneOffice = new Models.Office
                {
                    Office_Id = int.Parse(reader[0].ToString()!),
                    Street = reader[1].ToString()!,
                    House = reader[2].ToString()!
                };
                offices.Add(oneOffice);
            }
            await database.closeAsyncConnection();
            return offices;
        }
        //Get Access user
        public static async Task<int> getAuth(string login, string password)
        {
            await database.openAsyncConnection();
            MySqlCommand mysql_query = database.connection.CreateCommand();
            mysql_query.CommandText = @$"SELECT id_worker, phoneNumber, password FROM Worker WHERE phoneNumber = {login};";
            mysql_query.Connection = database.connection;
            var reader = await mysql_query.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {  
                if (reader[2].ToString() == password)
                {
                    int id = Convert.ToInt32(reader[0].ToString()!);
                    await database.closeAsyncConnection();
                    return id;
                }
                else
                {
                    await database.closeAsyncConnection();
                    return -1;
                }
            }
            else
            {
                await database.closeAsyncConnection();
                return 0;
            }
        }

        
    }
}
