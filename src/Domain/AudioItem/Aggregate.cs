namespace Domain.AudioItem
{
    using System;
    using System.Linq;
    using Bases;
    using Utilities;

    public class Aggregate : AggregateRoot<State>
    {
        public Aggregate(
            string id,
            string title,
            string[] categories)
            : base(new State())
        {
            Ensure.IsNotNullOrEmpty(title, nameof(title));

            State = new State
            {
                Id = id,
                Title = title,
                Categories = categories.ToList()
            };
        }

        public Aggregate(State state) : base(state)
        {
            Markers = new CollectionOfMarkers(state.Markers);
            Categories = new CollectionOfCategories(state.Categories);
        }

        public CollectionOfMarkers Markers { get; }

        public CollectionOfCategories Categories { get; }
    }
}
