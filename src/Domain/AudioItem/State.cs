namespace Domain.AudioItem
{
    using System;
    using Bases;

    public class State : AggregateState
    {
        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string[] Categories { get; set; }
    }
}