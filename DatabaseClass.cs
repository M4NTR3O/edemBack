
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace edemBack
{
    public class DatabaseClass
    {
        internal MySqlConnection connection;
        public DatabaseClass(IConfiguration configuration)
        {
            string host = configuration["Database:Host"]; // Имя хоста
            string port = configuration["Database:Port"];
            string database = configuration["Database:Name"]; // Имя базы данных
            string user = configuration["Database:User"]; // Имя пользователя
            string password = configuration["Database:Password"]; // Пароль пользователя

            string Connect = "server=" + host + ";port=" + port + ";username=" + user + ";password=" + password + ";database=" + database + ";";

            this.connection = new MySqlConnection(Connect);
        }
        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
        }
        public async Task openAsyncConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                await connection.OpenAsync();
        }
        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }
        public async Task closeAsyncConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
                await connection.CloseAsync();
        }
        public MySqlConnection getConnection()
        {
            return connection;
        }
        public MySqlConnection CreateNewConnection()
        {
            var newConnection = new MySqlConnection(connection.ConnectionString);
            return newConnection;
        }
    }
}
