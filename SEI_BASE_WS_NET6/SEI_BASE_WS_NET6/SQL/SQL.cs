using System.Data.SqlClient;

namespace SEI_WEBSERVICE
{
    public class SQL
    {
        public SqlConnection sqlConnection;
        private readonly ILogger logger;

        public SQL(ILogger _logger)
        {
            logger = _logger;
        }

        public void openSQLConnection()
        {
            string connectionString = string.Empty;
            logger.LogInformation("Conectando a SQL");

            try
            {
                connectionString = "Password=@DbPassword;Persist Security Info=True;User ID=@DbUserName;Initial Catalog=@CompanyDB;Data Source=@Server";
                connectionString = connectionString.Replace("@Server", AppSettings.DataBaseServer);
                connectionString = connectionString.Replace("@CompanyDB", AppSettings.DataBaseName);
                connectionString = connectionString.Replace("@DbUserName", AppSettings.DataBaseUserName);
                connectionString = connectionString.Replace("@DbPassword", AppSettings.DataBasePassword);

                logger.LogInformation("STRING SQL: " + connectionString);
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                if (sqlConnection.State != System.Data.ConnectionState.Open)
                {
                    throw new Exception("No es posible establecer la conexión SQL");
                }
                logger.LogInformation("Conectado a SQL");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
