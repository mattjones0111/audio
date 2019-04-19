namespace Domain.AudioItem
{
    using System;
    using System.Collections.Generic;
    using Bases;

    public class State : AggregateState
    {
        public State()
        {
            Categories = Array.Empty<string>();
            Markers = new List<MarkerState>();
        }

        public string Title { get; set; }
        public TimeSpan Duration { get; set; }
        public string[] Categories { get; set; }
        public List<MarkerState> Markers { get; set; }
    }

    public class MarkerState
    {
        public string Name { get; set; }
        public long Offset { get; set; }
    }
}