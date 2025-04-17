using edemBack.IFunction;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;

namespace edemBack
{
    public class DatabaseFactory: IDatabaseFactory
    {
        private readonly IConfiguration _configuration;
        public DatabaseFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DatabaseClass Create()
        {
            return new DatabaseClass(_configuration);
        }
    }
}
