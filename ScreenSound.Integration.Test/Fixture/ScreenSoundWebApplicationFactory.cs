using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ScreenSound.Banco;

namespace ScreenSound.Integration.Test.Fixture
{
    public class ScreenSoundWebApplicationFactory: WebApplicationFactory<Program>
    {
        public ScreenSoundContext Context { get; }

        private IServiceScope scope;

        public ScreenSoundWebApplicationFactory()
        {
            scope = Services.CreateScope();
            Context = scope.ServiceProvider.GetRequiredService<ScreenSoundContext>();
        }
    }
}
