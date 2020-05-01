namespace Integration.Tests
{
    using System;
    using System.Threading.Tasks;
    using Process.Features.Audio;
    using Process.Features.Audio.Markers;
    using Xunit;
    using Remove = Process.Features.Audio.Categories.Remove;
    using Process.DependencyResolution;

    public class CanCreateAudioItem : IntegrationTest
    {
        [Fact]
        public async Task Test()
        {
            string id = Guid.NewGuid().ToString();

            Create.Command command = new Create.Command
            {
                Id = id,
                Categories = new[] { "/songs/recurrent" },
                Title = "Test audio"
            };

            await Mediator().Send(command);

            Add.Command addMarker = new Add.Command
            {
                Id = id,
                Name = "marker 1",
                Offset = 123
            };

            await Mediator().Send(addMarker);

            Process.Features.Audio.Categories.Add.Command addCategory = 
                new Process.Features.Audio.Categories.Add.Command
                {
                    Id = id,
                    Category = "/songs/a-list",
                };

            await Mediator().Send(addCategory);

            Remove.Command removeCategory = new Remove.Command
            {
                Id = id,
                Category = "/songs/a-list",
            };

            await Mediator().Send(removeCategory);
        }
    }
}
