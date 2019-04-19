namespace Domain.AudioItem
{
    using System;
    using Bases;
    using Utilities;

    public class Aggregate : AggregateRoot<State>
    {
        public Aggregate(
            Guid id,
            string title,
            TimeSpan duration,
            string[] categories)
            : base(new State())
        {
            Ensure.IsNotNullOrEmpty(title, nameof(title));
            Ensure.IsNotNegative(duration, nameof(duration));

            State = new State
            {
                Id = id.ToString(),
                Title = title,
                Duration = duration,
                Categories = categories
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
