using Microsoft.EntityFrameworkCore;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;

namespace ScreenSound.Integration.Test;

public class Artista_GET : IClassFixture<ScreenSoundWebApplicationFactory>
{
    private readonly ScreenSoundWebApplicationFactory app;

    public Artista_GET(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
    }

    [Fact]
    public async Task Recupera_Artista_Por_Nome()
    {
        var artistaExistente = await app.Context.Artistas.FirstOrDefaultAsync();
        if (artistaExistente is null)
        {
            artistaExistente = new Modelos.Artista("João Donato", "Eu sou João Donato");
            app.Context.Artistas.Add(artistaExistente);
            await app.Context.SaveChangesAsync();
        }

        using var client = app.CreateClient();

        var response = await client.GetFromJsonAsync<Artista>("/Artistas/" + artistaExistente.Nome);

        Assert.NotNull(response);
        Assert.Equal(artistaExistente.Id, response.Id);
        Assert.Equal(artistaExistente.Nome, response.Nome);
    }
}
