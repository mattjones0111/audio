namespace Domain.AudioItem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionOfMarkers
    {
        readonly List<MarkerState> state;

        public CollectionOfMarkers(List<MarkerState> state)
        {
            this.state = state;
        }

        public void Add(string name, long offset)
        {
            if(Contains(name, offset))
            {
                // TODO use a DomainException
                throw new Exception(
                    "Marker collection already contains a " +
                    $"marker with name '{name}' at offset {offset}.");
            }

            state.Add(new MarkerState { Name = name, Offset = offset });
        }

        public void Delete(string name, long offset)
        {
            state.RemoveAll(x =>
                string.Equals(name, x.Name, StringComparison.InvariantCultureIgnoreCase) &&
                x.Offset == offset);
        }

        bool Contains(string name, long offset)
        {
            return state.Any(x => 
                string.Equals(name, x.Name, StringComparison.InvariantCultureIgnoreCase) &&
                x.Offset == offset);
        }
    }
}