namespace Domain.Bases
{
    using Utilities;

    public abstract class AggregateRoot<TState> where TState : AggregateState
    {
        public string Id => State.Id;

        protected TState State { get; set; }

        protected AggregateRoot(TState state)
        {
            Ensure.IsNotNull(state, nameof(state));

            State = state;
        }

        public TState ToDocument() => State;
    }
}