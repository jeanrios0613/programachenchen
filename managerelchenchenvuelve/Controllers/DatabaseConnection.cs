using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace managerelchenchenvuelve.Services
{
	public class DatabaseConnection
    {
        private readonly string? _connectionString;
        private readonly ILogger<DatabaseConnection> _logger;

        public DatabaseConnection(IConfiguration configuration, ILogger<DatabaseConnection> logger)
        {
            _connectionString = configuration.GetConnectionString("conexion");
            _logger = logger;
        }

        public DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
		{
			DataTable result = new DataTable();

            _logger.LogInformation("Executing query: {Query}", query);

			using (SqlConnection connection = new SqlConnection(_connectionString))
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				if (parameters != null)
					command.Parameters.AddRange(parameters);

				using (SqlDataAdapter adapter = new SqlDataAdapter(command))
				{
					adapter.Fill(result);
				}
			}

			return result;
		}

		public int ExecuteNonQuery(string query, params SqlParameter[] parameters)
		{
			int rowsAffected;
            _logger.LogInformation("Executing non-query: {Query}", query);

			using (SqlConnection connection = new SqlConnection(_connectionString))
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				if (parameters != null)
					command.Parameters.AddRange(parameters);

				connection.Open();
				rowsAffected = command.ExecuteNonQuery();
			}

			return rowsAffected;
		}

		public object ExecuteScalar(string query, params SqlParameter[] parameters)
		{
			object result;
            _logger.LogInformation("Executing scalar query: {Query}", query);

			using (SqlConnection connection = new SqlConnection(_connectionString))
			using (SqlCommand command = new SqlCommand(query, connection))
			{
				if (parameters != null)
					command.Parameters.AddRange(parameters);

				connection.Open();
				result = command.ExecuteScalar();
			}

			return result;
		}
	}
}
