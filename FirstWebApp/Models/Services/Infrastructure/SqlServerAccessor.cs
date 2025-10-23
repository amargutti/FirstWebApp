using System.Data;
using Microsoft.Data.SqlClient;

namespace FirstWebApp.Models.Services.Infrastructure
{
    public class SqlServerAccessor : IDatabaseAccessor
    {
        public async Task<DataSet> QueryAsync(string query)
        {
            using (var conn = new SqlConnection("Data Source=(LocalDb)\\FirstWebApp;Database=FirstWebAppDB;Trusted_Connection=True;"))
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
