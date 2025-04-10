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
            List<Models.Object> objects = new List<Models.Object>();
            using (var command = new MySqlCommand("SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object`;", database.connection))
            {
                var reader = await command.ExecuteReaderAsync();
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
            }
            await database.closeAsyncConnection();
            return objects;
        }

        //Get one object
        public static async Task<Models.Object?> getObject(int id)
        {
            await database.openAsyncConnection();
            Models.Object? _object = null;
            string query = $"SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object` WHERE `id_object` = {id};";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();
                
                while (await reader.ReadAsync())
                {
                    _object = new Models.Object
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
                }
            }            
            await database.closeAsyncConnection();
            return _object;
        }

        //Get Workers
        public static async Task<List<Worker>> getAllWorker()
        {
            await database.openAsyncConnection();
            List<Worker> workers = new List<Worker>();
            string query = "SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker`;";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();

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
            }
            await database.closeAsyncConnection();
            return workers;
        }
        //Get 1 worker
        public static async Task<Worker?> getWorker(int id)
        {
            await database.openAsyncConnection();
            Worker? worker = null;
            string query = $"SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker` WHERE `id_worker`={id};";
            using (var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    worker = new Models.Worker
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
                }
            }
            await database.closeAsyncConnection();
            return worker;
        }

        public static async Task<Role?> getRole(int id)
        {
            await database.openAsyncConnection();
            Models.Role? role = null;
            string query = $"SELECT `id_role`, `name`, `salary` FROM `Role` WHERE `id_role`={id};";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    role = new Models.Role
                    {
                        Role_Id = int.Parse(reader[0].ToString()!),
                        Name = reader[1].ToString()!,
                        Salary = int.Parse(reader[2].ToString()!)
                    };
                }
            }
            await database.closeAsyncConnection();
            return role;
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
            List<Models.Booking> booking = new List<Models.Booking>();
            string query = "SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status`;";
            using (var command = new MySqlCommand(query, database.connection)){
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Models.Booking oneBooking = new Models.Booking
                    {
                        Id_Booking = int.Parse(reader[0].ToString()!),
                        Object_id = int.Parse(reader[1].ToString()!),
                        Date_Start = DateTime.Parse(reader[2].ToString()!),
                        Date_End = DateTime.Parse(reader[3].ToString()!),
                        Status = reader[4].ToString()!
                    };
                    booking.Add(oneBooking);
                }
            }
            await database.closeAsyncConnection();
            return booking;
        }

        //Get One Booking
        public static async Task<Models.Booking?> getBooking(int id)
        {
            await database.openAsyncConnection();
            Models.Booking? booking = null;
            string query = $"SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status` WHERE `id_booking` = {id} ;";
            using (var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();
                await reader.ReadAsync();
                booking = new Models.Booking
                {
                    Id_Booking = int.Parse(reader[0].ToString()!),
                    Object_id = int.Parse(reader[1].ToString()!),
                    Date_Start = DateTime.Parse(reader[2].ToString()!),
                    Date_End = DateTime.Parse(reader[3].ToString()!),
                    Status = reader[4].ToString()!
                };
            }
            await database.closeAsyncConnection();
            return booking;
        }

        //Get One Booking
        public static async Task<List<Models.Booking?>> getBookingForDate(DateTime date)
        {
            await database.openAsyncConnection();
            List<Models.Booking?> booking = new List<Booking?>();
            string query = @"SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status` WHERE @Date BETWEEN `date_start` AND `date_end` ;";
            using (var command = new MySqlCommand(query, database.connection))
            {
                command.Parameters.Add("@Date", MySqlDbType.Date).Value = date;
                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var oneBooking = new Models.Booking
                    {
                        Id_Booking = int.Parse(reader[0].ToString()!),
                        Object_id = int.Parse(reader[1].ToString()!),
                        Date_Start = DateTime.Parse(reader[2].ToString()!),
                        Date_End = DateTime.Parse(reader[3].ToString()!),
                        Status = reader[4].ToString()!
                    };
                    booking.Add(oneBooking);
                }
            }
            await database.closeAsyncConnection();
            return booking;
        }

        //Get Roles
        public static async Task<List<Models.Role>> getAllRoles()
        {
            await database.openAsyncConnection();
            List<Models.Role> roles = new List<Role>();
            string query = "SELECT `id_role`, `name`, `salary` FROM `Role`;";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();

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
            }
            await database.closeAsyncConnection();
            return roles;
        }
        //Get additional material for id
        public static async Task<List<Models.Additionally>> getAdditionallyObject(int id)
        {
            await database.openAsyncConnection();
            List<Models.Additionally> additionally = new List<Models.Additionally>();
            string query = $"SELECT `id_object`, `name_parametr`, `value` FROM `Additionally` WHERE `id_object` = {id};";
            using (var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();

                while (reader.Read())
                {
                    Models.Additionally oneAdditionally = new Models.Additionally
                    {
                        ID_object = int.Parse(reader[0].ToString()!),
                        Parametr = reader[1].ToString()!,
                        Value = reader[2].ToString()!
                    };
                    additionally.Add(oneAdditionally);
                }
            }
            await database.closeAsyncConnection();
            return additionally;
        }

        //Get Office
        public static async Task<List<Models.Office>> getAllOffice()
        {
            await database.openAsyncConnection();
            List<Models.Office> offices = new List<Models.Office>();
            string query = "SELECT `id_office`, `street`, `house` FROM `Office`;";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();

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
            }
            await database.closeAsyncConnection();
            return offices;
        }
        //Get Access user
        public static async Task<int> getAuth(string login, string password)
        {
            await database.openAsyncConnection();
            string query = @$"SELECT id_worker, phoneNumber, password FROM Worker WHERE phoneNumber = {login};";
            using(var command = new MySqlCommand(query, database.connection))
            {
                var reader = await command.ExecuteReaderAsync();
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
}
