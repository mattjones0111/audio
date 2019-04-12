namespace Process.Ports
{
    using System.Threading.Tasks;
    using Domain.Bases;

    public interface IStoreDocuments
    {
        Task StoreAsync<TDocument>(TDocument document)
            where TDocument : AggregateState;

        Task<TDocument> GetAsync<TDocument>(string id)
            where TDocument : AggregateState;
    }
}