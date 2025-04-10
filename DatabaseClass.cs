
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;

namespace edemBack
{
    public class DatabaseClass
    {
        internal MySqlConnection connection;
        public DatabaseClass()
        {
            string host = "127.0.0.1"; // Имя хоста
            string port = "3306";
            string database = "edem_back"; // Имя базы данных
            string user = "root"; // Имя пользователя
            string password = ""; // Пароль пользователя

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
