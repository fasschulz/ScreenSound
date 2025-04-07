using ScreenSound.API.Requests;
using System.Net;
using System.Net.Http.Json;

namespace ScreenSound.Integration.Test;
public class Artista_POST : IClassFixture<ScreenSoundWebApplicationFactory>
{
    private readonly ScreenSoundWebApplicationFactory app;

    public Artista_POST(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Cadastra_ArtistaAsync()
    {
        using var client = app.CreateClient();
        var artista = new ArtistaRequest("João Silva", "Nascido no ES");

        var response = await client.PostAsJsonAsync("/Artistas", artista);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}