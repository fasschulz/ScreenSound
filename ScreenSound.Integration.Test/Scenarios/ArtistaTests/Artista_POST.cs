using ScreenSound.API.Requests;
using ScreenSound.API.Response;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ScreenSound.Integration.Test.Scenarios.ArtistaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_POST
{
    private readonly ScreenSoundWebApplicationFactory app;
    private readonly ArtistaFakeData _artistaFakeData;

    public Artista_POST(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
        _artistaFakeData = new ArtistaFakeData(app);
    }

    [Fact]
    public async Task Cadastra_ArtistaAsync()
    {
        using var client = app.CreateClient();
        var artista = new ArtistaRequest("João Silva", "Nascido no ES");

        var response = await client.PostAsJsonAsync("/Artistas", artista);

        JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        string jsonString = await response.Content.ReadAsStringAsync();
        var artistaCadastrado = JsonSerializer.Deserialize<Artista>(jsonString, options);
                
        _artistaFakeData.LimparDadosDoBanco(artistaCadastrado);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }    
}