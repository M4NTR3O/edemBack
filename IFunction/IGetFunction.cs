using MySql.Data.MySqlClient;
using edemBack.Models;

namespace edemBack.IFunction
{
    public interface IGetFunction
    {
        //Get all object
        public static async Task<List<Models.Object>> getAllObject(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Object> objects = new List<Models.Object>();
                using (var command = new MySqlCommand("SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object`;", connection))
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
                            Balcony = bool.Parse(reader[9].ToString()!),
                            Photos = await getPhotoOneObject(int.Parse(reader[0].ToString()!), factory)
                        };
                        objects.Add(oneObject);
                    }
                }
                await connection.CloseAsync();
                return objects;
            }
            
        }

        //Get one object
        public static async Task<Models.Object?> getObject(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Object? _object = null;
                string query = @"SELECT `id_object`, `office_id`, `street`, `house`, `apartment`, `rooms`, `status`, `area`, `kitchen`, `balcony` FROM `Object` WHERE `id_object` = @id;";
                using(var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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
                            Balcony = bool.Parse(reader[9].ToString()!),
                            Photos = await getPhotoOneObject(int.Parse(reader[0].ToString()!), factory)
                        };
                    }
                }            
                await connection.CloseAsync();
                return _object;
            }
            
        }

        //Get Workers
        public static async Task<List<Worker>> getAllWorker(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Worker> workers = new List<Worker>();
                string query = "SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker`;";
                using(var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();

                    while (await reader.ReadAsync())
                    {
                        Models.Worker worker = new Models.Worker
                        {
                            Id = int.Parse(reader[0].ToString()!),
                            Surname = reader[1].ToString()!,
                            Name = reader[2].ToString()!,
                            Fathername = reader[3].ToString()! != null ? reader[3].ToString() : null,
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
                await connection.CloseAsync();
                return workers;
            }
            
        }
        //Get 1 worker
        public static async Task<Worker?> getWorker(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Worker? worker = null;
                string query = $"SELECT `id_worker`, `surname`, `name`, `fatherName`, `phoneNumber`, `email`, `birthday`, `role_id`, `office_id`, `password` FROM `Worker` WHERE `id_worker`={id};";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        worker = new Models.Worker
                        {
                            Id = int.Parse(reader[0].ToString()!),
                            Surname = reader[1].ToString()!,
                            Name = reader[2].ToString()!,
                            Fathername = reader[3].ToString()! != null ? reader[3].ToString() : null,
                            PhoneNumber = reader[4].ToString()!,
                            Email = reader[5].ToString()!,
                            Birthday = DateTime.Parse(reader[6].ToString()!),
                            Id_Role = int.Parse(reader[7].ToString()!),
                            Id_Office = int.Parse(reader[8].ToString()!),
                            Password = reader[9].ToString()!
                        };
                    }
                }
                await connection.CloseAsync();
                return worker;
            }
            
        }
        //Get Role for Id Role
        public static async Task<Role?> getRole(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Role? role = null;
                string query = @"SELECT `id_role`, `name`, `salary` FROM `Role` WHERE `id_role`= @id;";
                using(var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    role = new Models.Role
                    {
                        Role_Id = int.Parse(reader[0].ToString()!),
                        Name = reader[1].ToString()!,
                        Salary = int.Parse(reader[2].ToString()!)
                    };
                }
                await connection.CloseAsync();
                return role;
            }          
        }

        //Get Role for Id Worker
        public static async Task<Role?> getRoleIdWorker (int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Role? role = null;
                string query = @"SELECT `role_id`, `Role`.`name`, `Role`.`salary` FROM `Worker` INNER JOIN `Role` ON `Worker`.`role_id` = `Role`.`id_role` WHERE `id_worker`= @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    role = new Models.Role
                    {
                        Role_Id = int.Parse(reader[0].ToString()!),
                        Name = reader[1].ToString()!,
                        Salary = int.Parse(reader[2].ToString()!)
                    };
                }
                await connection.CloseAsync();
                return role;
            }
        }

        //Get Request with filter
        public static async Task<List<Models.Request>> getAllRequest(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = "SELECT `id_request`, `type_id`, `worker_id`, `object_id`, `status`, `urgency`, `description` FROM `Request`;";
                List<Models.Request> requests = new List<Models.Request>();
                using(var command = new MySqlCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Models.Request oneRequest = new Models.Request
                            {
                                Request_Id = int.Parse(reader[0].ToString()!),
                                Type_Work = int.Parse(reader[1].ToString()!).ToString(),
                                Worker_Id = (reader[2] != DBNull.Value ? int.Parse(reader[2].ToString()!) : null),
                                Object_Id = int.Parse(reader[3].ToString()!),
                                Status = int.Parse(reader[4].ToString()!).ToString(),
                                Urgency = bool.Parse(reader[5].ToString()!),
                                Description = reader[6].ToString()!,
                                Photos = null,
                                Roles_Id = null
                            };
                            requests.Add(oneRequest);
                        }
                    }
                }
                await connection.CloseAsync();
                return requests;
            }
        }

        //Get One Request
        public static async Task<Models.Request?> getRequest(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Request? request = null;
                string query = @"SELECT `id_request`, `type_id`, `worker_id`, `object_id`, `status`, `urgency`, `description` FROM `Request` WHERE `id_request` = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    Console.WriteLine(reader[2].ToString()! == null);
                    request = new Models.Request
                    {
                        Request_Id = int.Parse(reader[0].ToString()!),
                        Type_Work = reader[1].ToString()!,
                        Worker_Id = (reader[2] == DBNull.Value ? int.Parse(reader[2].ToString()!) : null),
                        Object_Id = int.Parse(reader[3].ToString()!),
                        Status = reader[4].ToString()!,
                        Urgency = bool.Parse(reader[5].ToString()!),
                        Description = reader[6].ToString()!,
                    };
                }
                await connection.CloseAsync();
                return request;
            }
        }
        //Get Booking
        public static async Task<List<Models.Booking>> getAllBooking(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Booking> booking = new List<Models.Booking>();
                string query = "SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status`;";
                using (var command = new MySqlCommand(query, connection))
                {
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
                await connection.CloseAsync();
                return booking;
            }            
        }

        //Get One Booking
        public static async Task<Models.Booking?> getBooking(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Booking? booking = null;
                string query = @"SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status` WHERE `id_booking` = @id ;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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
                await connection.CloseAsync();
                return booking;
            }
        }

        //Get One Booking
        public static async Task<List<Models.Booking?>> getBookingForDate(DateTime date, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Booking?> booking = new List<Booking?>();
                string query = @"SELECT `id_booking`, `object_id`, `date_start`, `date_end`, `Status`.`name` FROM `Object_Booking` INNER JOIN `Status` ON `Object_Booking`.`status` = `Status`.`id_status` WHERE @Date BETWEEN `date_start` AND `date_end` ;";
                using (var command = new MySqlCommand(query, connection))
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
                await connection.CloseAsync();
                return booking;
            }
        }

        //Get Roles
        public static async Task<List<Models.Role>> getAllRoles(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Role> roles = new List<Role>();
                string query = "SELECT `id_role`, `name`, `salary` FROM `Role`;";
                using(var command = new MySqlCommand(query, connection))
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
                await connection.CloseAsync();
                return roles;
            }
        }
        //Get additional material for id
        public static async Task<List<Models.Additionally>> getAdditionallyObject(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Additionally> additionally = new List<Models.Additionally>();
                string query = @"SELECT `id_object`, `name_parametr`, `value` FROM `Additionally` WHERE `id_object` = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
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
                await connection.CloseAsync();
                return additionally;
            }
        }

        //Get Office
        public static async Task<List<Models.Office>> getAllOffice(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Office> offices = new List<Models.Office>();
                string query = "SELECT `id_office`, `street`, `house` FROM `Office`;";
                using(var command = new MySqlCommand(query, connection))
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
                await connection.CloseAsync();
                return offices;
            }
        }
        //Get Access user
        public static async Task<int> getAuth(string login, string password, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"SELECT id_worker, phoneNumber, password FROM Worker WHERE phoneNumber = @login;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        bool valid = PasswordHasher.Verify(password, reader[2].ToString()!);
                        if (valid /*reader[2].ToString() == password*/)
                        {
                            int id = Convert.ToInt32(reader[0].ToString()!);
                            await connection.CloseAsync();
                            return id;
                        }
                        else
                        {
                            await connection.CloseAsync();
                            return -1;
                        }
                    }
                    else
                    {
                        await connection.CloseAsync();
                        return 0;
                    }
                }
            }
        }
        //Get Status id
        public static async Task<Models.Status?> getOneStatus(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                Models.Status? status = null;
                string query = @"SELECT `id_status`, `name` FROM `Status` WHERE `id_status` = @Id;";
                using(var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    status = new Models.Status
                    {
                        Id = int.Parse(reader[0].ToString()!),
                        Name = reader[1].ToString()!
                    };
                }
                await connection.CloseAsync();
                return status;
            }
        }
        //Get All Status
        public static async Task<List<Models.Status>> getAllStatus(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Status> statusAll = new List<Status>();
                string query = @"SELECT `id_status`, `name` FROM `Status`;";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while(await reader.ReadAsync())
                    {
                        Models.Status status = new Models.Status
                        {
                            Id = int.Parse(reader[0].ToString()!),
                            Name = reader[1].ToString()!
                        };
                        statusAll.Add(status);
                    }
                }
                await connection.CloseAsync();
                return statusAll;
            }
        }
        //Get Not Important
        public static async Task<List<Models.Role>> getNotImportnantRoles(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Role> roles = new List<Role>();
                string query = "SELECT `id_role`, `name` FROM `Role` WHERE NOT `id_role` BETWEEN 1 AND 2 AND NOT `id_role` BETWEEN 4 AND 5;";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Role role = new Models.Role
                        {
                            Role_Id = int.Parse(reader[0].ToString()!),
                            Name = reader[1].ToString()!
                        };
                        roles.Add(role);
                    }
                }
                await connection.CloseAsync();
                return roles;
            }
        }

        //Get Types Work
        public static async Task<List<Models.TypeWork>> getTypesWorks(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.TypeWork> typeWorks = new List<TypeWork>();
                string query = "SELECT `id_work`, `name` FROM `Type_Work";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.TypeWork type = new TypeWork
                        {
                            Id_Work = int.Parse(reader[0].ToString()!),
                            Name = reader[1].ToString()!
                        };
                        typeWorks.Add(type);
                    }
                }
                await connection.CloseAsync();
                return typeWorks;
            }
        }
        //Get One Types Work
        public static async Task<Models.TypeWork?> getOneTypesWorks(IDatabaseFactory factory, int id)
        {
            var database = factory.Create();
            Models.TypeWork? typeWork = null;
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                string query = @"SELECT `id_work`, `name` FROM `Type_Work` WHERE `id_work`=@id";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    await reader.ReadAsync();
                    typeWork = new TypeWork
                    {
                        Id_Work = int.Parse(reader[0].ToString()!),
                        Name = reader[1].ToString()!
                    };
                }
                await connection.CloseAsync();
            }
            if (typeWork == null)
            {
                return typeWork;
            }
            else
            {
                typeWork.Roles = new List<Role>();
                using (var connection = database.CreateNewConnection())
                {
                    await connection.OpenAsync();
                    string query = @"SELECT `role_id`, `Role`.`name` FROM `Work_Role` INNER JOIN `Role` ON `Role`.`id_role` = `Work_Role`.`role_id` WHERE `work_id` = @Id";
                    using (var command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Id", id);
                        var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            Models.Role role = new Role
                            {
                                Role_Id = int.Parse(reader[0].ToString()!),
                                Name = reader[1].ToString()!
                            };
                            typeWork.Roles.Add(role);
                        }
                    }
                    await connection.CloseAsync();
                }
            } 
            return typeWork;
        }
        //Get Work_Role As Number Id
        public static async Task<List<Models.Work_Role>> getWorkRolesNumber(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using(var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Work_Role> works_roles = new List<Work_Role>();
                string query = "SELECT `work_id`, `role_id` FROM `Work_Role`;";
                using(var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Work_Role work_Role = new Work_Role
                        {
                            Work = reader[0].ToString()!,
                            Role = reader[1].ToString()!
                        };
                        works_roles.Add(work_Role);
                    }
                }
                await connection.CloseAsync();
                return works_roles;
            }
        }
        //Get Work_Role As Text
        public static async Task<List<Models.Work_Role>> getWorkRolesText(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Work_Role> works_roles = new List<Work_Role>();
                string query = "SELECT (`Type_Work`.`name`) AS `work`, `Role`.`name` AS `role` FROM `Work_Role` INNER JOIN `Type_Work` ON `Type_Work`.`id_work` = `Work_Role`.`work_id` INNER JOIN `Role` ON `Role`.`id_role` = `Work_Role`.`role_id`;";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Work_Role work_Role = new Work_Role
                        {
                            Work = reader[0].ToString()!,
                            Role = reader[1].ToString()!
                        };
                        works_roles.Add(work_Role);
                    }
                }
                await connection.CloseAsync();
                return works_roles;
            }
        }
        //Get Additional Parametrs For All Role
        public static async Task<List<Models.Add_Parametrs_Role>> getAddParametrsAllRole(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Add_Parametrs_Role> adds = new List<Add_Parametrs_Role>();
                string query = "SELECT `id_parametr`, `role_id`, `parametr` FROM `Add_Parametrs_Role`;";
                using (var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Add_Parametrs_Role add_ = new Add_Parametrs_Role
                        {
                            Id_Parametr = int.Parse(reader[0].ToString()!),
                            Role_Id = int.Parse(reader[1].ToString()!),
                            Parametr = reader[2].ToString()!
                        };
                        adds.Add(add_);
                    }
                    await connection.CloseAsync();
                    return adds;
                }
            }
        }
        //Get Additional Parametrs For One Role
        public static async Task<List<Models.Add_Parametrs_Role>> getAddParametrsOneRole(IDatabaseFactory factory, int id)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Add_Parametrs_Role> adds = new List<Add_Parametrs_Role>();
                string query = @"SELECT `id_parametr`, `role_id`, `parametr` FROM `Add_Parametrs_Role` WHERE `id_role` = @id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Add_Parametrs_Role add_ = new Add_Parametrs_Role
                        {
                            Id_Parametr = int.Parse(reader[0].ToString()!),
                            Role_Id = int.Parse(reader[1].ToString()!),
                            Parametr = reader[2].ToString()!
                        };
                        adds.Add(add_);
                    }
                    await connection.CloseAsync();
                    return adds;
                }
            }
        }
        //Get Photo_Object
        public static async Task<List<Models.Photo_Object>> getPhotoObject(IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Photo_Object> photo_Objects = new List<Photo_Object>();
                string query = "SELECT `photo_id`, `object_id` FROM `Photo_Object`;";
                using(var command = new MySqlCommand(query, connection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Photo_Object @object = new Photo_Object
                        {
                            Photo_id = int.Parse(reader[0].ToString()!),
                            Object_id = int.Parse(reader[1].ToString()!)
                        };
                        photo_Objects.Add(@object);
                    }
                }
                await connection.CloseAsync();
                return photo_Objects;
            }
        }

        //Get Photo_Object for Object
        public static async Task<List<Models.Photo>> getPhotoOneObject(int id, IDatabaseFactory factory)
        {
            var database = factory.Create();
            using (var connection = database.CreateNewConnection())
            {
                await connection.OpenAsync();
                List<Models.Photo> photo_Objects = new List<Photo>();
                string query = @"SELECT `photo_id`, `Photo`.`url` FROM `Photo_Object` INNER JOIN `Photo` ON `Photo`.`id_photo` = `Photo_Object`.`photo_id` WHERE `object_id`=@id;";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        Models.Photo @object = new Photo
                        {
                            Id = int.Parse(reader[0].ToString()!),
                            Url = reader[1].ToString()!
                        };
                        photo_Objects.Add(@object);
                    }
                }
                await connection.CloseAsync();
                return photo_Objects;
            }
        }
    }
}
