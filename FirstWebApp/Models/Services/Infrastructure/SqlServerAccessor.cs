using System.Data;
using FirstWebApp.Models.Options;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace FirstWebApp.Models.Services.Infrastructure
{
    public class SqlServerAccessor : IDatabaseAccessor
    {
        private readonly IConfiguration _configuration;

        private readonly IOptionsMonitor<ConnectionStringsOptions> connectionStringsOptions;
        public SqlServerAccessor(IOptionsMonitor<ConnectionStringsOptions> connectionStringsOptions)
        {
            this.connectionStringsOptions = connectionStringsOptions;
        } 

        public async Task<DataSet> QueryAsync(string query)
        {
            using (var conn = new SqlConnection(connectionStringsOptions.CurrentValue.Default))
            {
                await conn.OpenAsync();
                using (var cmd = new SqlCommand(query, conn))
                {
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        //mentro leggo i dati creo un DataSet
                        var dataSet = new DataSet();
                        
                        do
                        {
                            //creo anche una DataTable
                            var dataTable = new DataTable();
                            //carico i la tabella nel DataSet
                            dataSet.Tables.Add(dataTable);
                            //carico i dati letti dal reader nella DataTable
                            dataTable.Load(reader);
                        } while (!reader.IsClosed); //finchè il reader è aperto (ci sono altre tabelle da leggere)

                        return dataSet;
                    }
                }
            }
        }
    }
}
