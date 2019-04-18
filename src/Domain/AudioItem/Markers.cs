namespace Domain.AudioItem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Markers
    {
        readonly List<MarkerState> state;

        public Markers(List<MarkerState> state)
        {
            this.state = state;
        }

        public void Add(string name, long offset)
        {
            if(state.Any(x =>
                x.Offset == offset &&
                string.Equals(x.Name, name,
                    StringComparison.InvariantCultureIgnoreCase)))
            {
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
    }
}