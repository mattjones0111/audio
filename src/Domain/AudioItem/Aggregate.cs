namespace Domain.AudioItem
{
    using System;
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
            Ensure.IsNotNull(state, nameof(state));
        }
    }
}
