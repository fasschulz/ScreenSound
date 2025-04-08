using ScreenSound.API.Requests;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using System.Net;
using System.Net.Http.Json;

namespace ScreenSound.Integration.Test.Scenarios.ArtistaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_POST : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory app;
    private readonly ArtistaFakeData _artistaFakeData;

    public Artista_POST(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
        _artistaFakeData = new ArtistaFakeData(app);
    }

    public void Dispose()
    {
        _artistaFakeData.LimparDadosDoBanco();
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