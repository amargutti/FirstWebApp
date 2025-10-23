using System.Data;

namespace FirstWebApp.Models.Services.Infrastructure
{
    public interface IDatabaseAccessor
    {
        DataSet Query (string query);
    }
}
