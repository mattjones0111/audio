namespace Integration.Tests
{
    using System;
    using System.Threading.Tasks;
    using Process.Features.Audio;
    using Process.Features.Audio.Markers;
    using Xunit;

    public class CanCreateAudioItem : IntegrationTest
    {
        [Fact]
        public async Task Test()
        {
            Guid newAggregateId = Guid.NewGuid();

            Create.Command command = new Create.Command
            {
                Id = newAggregateId,
                Categories = new[] { "/songs/recurrent" },
                Source = "http://www.audio.com/1.wav",
                Title = "Test audio"
            };

            await Mediator().Send(command);

            Add.Command addMarker = new Add.Command
            {
                Id = newAggregateId,
                Name = "marker 1",
                Offset = 123
            };

            await Mediator().Send(addMarker);

            Process.Features.Audio.Categories.Add.Command addCategory = 
                new Process.Features.Audio.Categories.Add.Command
                {
                    Id = newAggregateId.ToString(),
                    Category = "/songs/a-list",
                };

            await Mediator().Send(addCategory);
        }
    }
}
