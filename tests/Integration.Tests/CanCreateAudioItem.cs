namespace Integration.Tests
{
    using System;
    using System.Threading.Tasks;
    using Process.Features.Audio;
    using Xunit;

    public class CanCreateAudioItem : IntegrationTest
    {
        [Fact]
        public async Task Test()
        {
            Create.Command command = new Create.Command
            {
                Categories = new[] { "/songs/recurrent" },
                Duration = TimeSpan.FromMinutes(3.33),
                Source = "http://www.audio.com/1.wav",
                Title = "Test audio"
            };

            await Mediator().Send(command);
        }
    }
}
