namespace Domain.AudioItem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bases;
    using Utilities;

    public class Aggregate : AggregateRoot<State>
    {
        public Aggregate(string title, TimeSpan duration, string[] categories)
            : base(new State())
        {
            Ensure.IsNotNullOrEmpty(title, nameof(title));
            Ensure.IsNotNegative(duration, nameof(duration));

            State = new State
            {
                Id = Guid.NewGuid().ToString(),
                Title = title,
                Duration = duration,
                Categories = categories
            };
        }

        public Aggregate(State state) : base(state)
        {
            Markers = new Markers(state.Markers);
        }

        public Markers Markers { get; }
    }

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
                string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception(
                    "Marker collection already contains a " +
                    $"marker with name '{name}' at offset {offset}.");
            }

            state.Add(new MarkerState { Name = name, Offset = offset });
        }
    }
}
