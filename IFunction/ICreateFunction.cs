using MySql.Data.MySqlClient;

namespace edemBack.IFunction
{
    public interface ICreateFunction: IConnection
    {
        //Create object
        public void createObject(Models.Object _object)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Object` into Values ()";
            database.closeConnection();
        }
        //Create worker
        public void createWorker(Models.Worker worker)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Worker` into Values ()";
            database.closeConnection();
        }

        //Create filial
        public void createOffice(Models.Office office)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Office` into Values ()";
            database.closeConnection();
        }
        //Create additional material
        public void createAdditionalMaterial(Models.Additionally additionally)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Additionally` into Values ()";
            database.closeConnection();
        }
        //create role
        public void createRole(Models.Role role)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Role` into Values ()";
            database.closeConnection();
        }
        //create request
        public void createRequest(/*Models.Request request*/)
        {
            database.openConnection();
            MySqlCommand mySql_query = database.connection.CreateCommand();
            //Дозаполнить
            mySql_query.CommandText = "Create `Role` into Values ()";
            database.closeConnection();
        }
    }
}
