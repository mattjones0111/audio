namespace Process.Ports
{
    using System.Threading.Tasks;

    public interface IStoreTabularData
    {
        Task Insert<TEntity>(string tableName, TEntity entity);
    }
}
