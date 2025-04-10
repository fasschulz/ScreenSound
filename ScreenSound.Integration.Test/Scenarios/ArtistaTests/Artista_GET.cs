using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;

namespace ScreenSound.Integration.Test.Scenarios.ArtistaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_GET
{
    private readonly ScreenSoundWebApplicationFactory app;
    private ArtistaFakeData _artistaFakeData;

    public Artista_GET(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
        _artistaFakeData = new ArtistaFakeData(app);
    }

    [Fact]
    public async Task Recupera_Artista_Por_Nome()
    {
        var artistaExistente = _artistaFakeData.CriarDadosFake().FirstOrDefault();
        
        using var client = app.CreateClient();

        var response = await client.GetFromJsonAsync<Artista>("/Artistas/" + artistaExistente.Nome);

        _artistaFakeData.LimparDadosDoBanco(artistaExistente);
        Assert.NotNull(response);
        Assert.Equal(artistaExistente.Id, response.Id);
        Assert.Equal(artistaExistente.Nome, response.Nome);
    }

    [Fact]
    public async Task Recupera_Todos_Os_Artistas()
    {
        var artistasExistentes = _artistaFakeData.CriarDadosFake(10);
        using var client = app.CreateClient();

        var response = await client.GetFromJsonAsync<IEnumerable<Artista>>("/Artistas/");

        _artistaFakeData.LimparDadosDoBanco(artistasExistentes);
        Assert.NotNull(response);
        Assert.True(response.Count() > 0);        
    }

    [Fact]
    public async Task Retorna_Not_Found_Quando_Nome_Artista_Inexistente()
    {
        using var client = app.CreateClient();

        var response = await client.GetAsync("/Artistas/" + "NomeInexistente");
                
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);        
    }

    [Fact]
    public async Task Retorna_Not_Found_Quando_Nao_Acha_Artistas_Na_Base()
    {
        using var client = app.CreateClient();

        var response = await client.GetAsync("/Artistas/");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);        
    }
}
