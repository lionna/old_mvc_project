using CarServiceApp.Domain.Config;
using CarServiceApp.Domain.Repository.Abstract;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace CarServiceApp.Domain.Repository
{
    public class DbConnectionLayer(IOptions<ApplicationSettings> settings) : IDbConnectionLayer
    {
#pragma warning disable CS0649 // Field 'DbConnectionLayer.sqlConnection' is never assigned to, and will always have its default value null
        private readonly SqlConnection sqlConnection;
#pragma warning restore CS0649 // Field 'DbConnectionLayer.sqlConnection' is never assigned to, and will always have its default value null
        private readonly string connectionString = settings?.Value?.DefaultConnection ?? throw new ArgumentNullException(nameof(settings));
        public string InfoMessage { set; get; }

        //первая версия метода - только имя процедуры и входящие параметры
        public async Task<int> ExecutionProcedureAsync(string procedureName, string name, int? id)
        {
            var sqlParams = new List<SqlParameter>();
            using (var connection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                using (var command = new SqlCommand(procedureName, sqlConnection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // добавление входных парамтеров
                    command.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 150)
                    {
                        Value = name
                    });

                    command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int)
                    {
                        Value = id ?? 0
                    });

                    var returnValueParam = new SqlParameter("@ReturnValue", DbType.Int32)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(returnValueParam);

                    command.Parameters.AddRange([.. sqlParams]);
                    await command.ExecuteNonQueryAsync();
                    await sqlConnection.CloseAsync();

                    return (int)command.Parameters["@ReturnValue"].Value;
                }
            }
        }

        //первая версия метода - только имя процедуры и входящие параметры
        public async Task<int> ExecutionProcedureAsync(string procedureName, Dictionary<string, object> inputParameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var sqlParams = new List<SqlParameter>();
                using (var cmd = new SqlCommand(procedureName, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var item in inputParameters)
                    {
                        var inParam = new SqlParameter("@" + item.Key, item.Value)
                        {
                            Direction = ParameterDirection.Input
                        };
                        sqlParams.Add(inParam);
                    }

                    var returnValueParam = new SqlParameter("@ReturnValue", DbType.Int32)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(returnValueParam);

                    cmd.Parameters.AddRange(sqlParams.ToArray());
                    await cmd.ExecuteNonQueryAsync();
                    await sqlConnection.CloseAsync();

                    return (int)cmd.Parameters["@ReturnValue"].Value;
                }
            }
        }

        //вторая версия метода -  имя процедуры, in параметры и out параметры 
        public async Task<int> ExecutionProcedureAsync(
            string procedureName,
            Dictionary<string, object> inputParameters,
            Dictionary<string, object> outParameters)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                var sqlParams = new List<SqlParameter>();
                using (var cmd = new SqlCommand(procedureName, sqlConnection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (var prm in inputParameters)
                    {
                        var inParam = new SqlParameter
                        {
                            Direction = ParameterDirection.Input,
                            ParameterName = "@" + prm.Key,
                            Value = prm.Value
                        };
                        sqlParams.Add(inParam);
                    }

                    //return value
                    var returnValueParam = new SqlParameter("@ReturnValue", DbType.Int32)
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    sqlParams.Add(returnValueParam);

                    //out параметры
                    foreach (var prm in outParameters)
                    {
                        var output = new SqlParameter
                        {
                            ParameterName = "@" + prm.Key,
                            Direction = ParameterDirection.Output,
                            DbType = (DbType)prm.Value,
                            Size = 8
                        };
                        sqlParams.Add(output);
                    }

                    cmd.Parameters.AddRange([.. sqlParams]);
                    await cmd.ExecuteNonQueryAsync();
                    await sqlConnection.CloseAsync();

                    //возвращаем out переменные
                    foreach (var item in outParameters.Keys.ToList())
                    {
                        outParameters[item] = cmd.Parameters["@" + item].Value;
                    }

                    return (int)cmd.Parameters["@ReturnValue"].Value;
                }
            }
        }

        private void GetInfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            this.InfoMessage = e.Message;
        }
    }
}