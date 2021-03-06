﻿namespace Process.Adapters.InMemory
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Bases;
    using Ports;

    public class DocumentStore : IStoreDocuments
    {
        readonly IDictionary<string, object> innerDict;

        public DocumentStore()
        {
            innerDict = new Dictionary<string, object>();
        }

        public Task StoreAsync<TDocument>(TDocument document)
            where TDocument : AggregateState
        {
            if(innerDict.ContainsKey(document.Id))
            {
                innerDict[document.Id] = document;
            }
            else
            {
                innerDict.Add(document.Id, document);
            }

            return Task.CompletedTask;
        }

        public Task<TDocument> GetAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            TDocument docResult = default;

            if(innerDict.TryGetValue(id, out var result))
            {
                docResult = (TDocument) result;
            }

            return Task.FromResult(docResult);
        }

        public Task DeleteAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            if(innerDict.ContainsKey(id))
            {
                innerDict.Remove(id);
            }

            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync<TDocument>(string id)
            where TDocument : AggregateState
        {
            return Task.FromResult(innerDict.ContainsKey(id));
        }
    }
}
