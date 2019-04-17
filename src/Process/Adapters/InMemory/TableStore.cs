namespace Process.Adapters.InMemory
{
    using System.Threading.Tasks;
    using Ports;

    public class TableStore : IStoreTabularData
    {
        public Task Insert<TEntity>(string tableName, TEntity entity)
        {
            return Task.CompletedTask;
        }
    }
}