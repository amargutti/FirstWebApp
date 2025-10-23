using System.Data;

namespace FirstWebApp.Models.Services.Infrastructure
{
    public interface IDatabaseAccessor
    {
        Task<DataSet> QueryAsync (string query);
    }
}
