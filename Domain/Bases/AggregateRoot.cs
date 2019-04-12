namespace Domain.Bases
{
    public abstract class AggregateRoot<TState> where TState : AggregateState
    {
        protected TState State { get; set; }

        protected AggregateRoot(TState state)
        {
            State = state;
        }

        public TState ToDocument() => State;
    }
}