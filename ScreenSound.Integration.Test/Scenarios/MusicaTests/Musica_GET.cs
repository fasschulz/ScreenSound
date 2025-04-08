using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Integration.Test.Scenarios.MusicaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Musica_GET : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;

    public Musica_GET(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _musicaFakeData.CriarDadosFake(20);
    }

    public void Dispose()
    {
        _musicaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Retorna_Musica_Por_Nome()
    {
        var musicaExistente = await _app.Context.Musicas.FirstOrDefaultAsync();

        using var client = _app.CreateClient();

        var response = await client.GetFromJsonAsync<Musica>("/Musicas/" + musicaExistente.Nome);

        Assert.NotNull(response);
        Assert.Equal(musicaExistente.Id, response.Id);
        Assert.Equal(musicaExistente.Nome, response.Nome);
    }

    [Fact]
    public async Task Retorna_Todas_As_Musicas()
    {
        using var client = _app.CreateClient();

        var response = await client.GetFromJsonAsync<IEnumerable<Musica>>("/Musicas/");
                
        Assert.NotNull(response);
        Assert.True(response.Count() > 0);
    }
}
