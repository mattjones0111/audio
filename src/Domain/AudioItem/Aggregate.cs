﻿namespace Domain.AudioItem
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
            TimeSpan duration,
            string[] categories)
            : base(new State())
        {
            Ensure.IsNotNullOrEmpty(title, nameof(title));
            Ensure.IsNotNegative(duration, nameof(duration));

            State = new State
            {
                Id = id,
                Title = title,
                Duration = duration,
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
