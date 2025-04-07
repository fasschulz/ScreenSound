using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;

namespace ScreenSound.Integration.Test;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_GET : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory app;
    private ArtistaFakeData _artistaFakeData;

    public Artista_GET(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
        _artistaFakeData = new ArtistaFakeData(app);
        _artistaFakeData.CriarDadosFake(20);
    }

    public void Dispose()
    {
        _artistaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Recupera_Artista_Por_Nome()
    {
        var artistaExistente = await app.Context.Artistas.FirstOrDefaultAsync();
        if (artistaExistente is null)
        {
            artistaExistente = new Artista("João Donato", "Eu sou João Donato");
            app.Context.Artistas.Add(artistaExistente);
            await app.Context.SaveChangesAsync();
        }

        using var client = app.CreateClient();

        var response = await client.GetFromJsonAsync<Artista>("/Artistas/" + artistaExistente.Nome);

        Assert.NotNull(response);
        Assert.Equal(artistaExistente.Id, response.Id);
        Assert.Equal(artistaExistente.Nome, response.Nome);
    }

    [Fact]
    public async Task Recupera_Todos_Os_Artistas()
    {       
        using var client = app.CreateClient();

        var response = await client.GetFromJsonAsync<IEnumerable<Artista>>("/Artistas/");

        Assert.NotNull(response);
        Assert.True(response.Count() > 0);        
    }
}
