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
public class Musica_GET
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;
    private readonly GeneroFakeData _generoFakeData;
    private readonly ArtistaFakeData _artistaFakeData;

    public Musica_GET(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _generoFakeData = new GeneroFakeData(app);
        _artistaFakeData = new ArtistaFakeData(app);
    }

    [Fact]
    public async Task Retorna_Musica_Por_Nome()
    {
        var musicaExistente = _musicaFakeData.CriarDadosFake().FirstOrDefault();

        using var client = _app.CreateClient();

        var response = await client.GetFromJsonAsync<Musica>("/Musicas/" + musicaExistente.Nome);

        _generoFakeData.LimparDadosDoBanco(musicaExistente.Id);
        _musicaFakeData.LimparDadosDoBanco(musicaExistente.Id);
        _artistaFakeData.LimparDadosDoBanco(musicaExistente.ArtistaId);
        Assert.NotNull(response);
        Assert.Equal(musicaExistente.Id, response.Id);
        Assert.Equal(musicaExistente.Nome, response.Nome);
    }

    [Fact]
    public async Task Retorna_Todas_As_Musicas()
    {
        var musicasExistentes = _musicaFakeData.CriarDadosFake(5);
        using var client = _app.CreateClient();

        var response = await client.GetFromJsonAsync<IEnumerable<Musica>>("/Musicas/");

        _generoFakeData.LimparDadosDoBanco(musicasExistentes);        
        _musicaFakeData.LimparDadosDoBanco(musicasExistentes);
        _artistaFakeData.LimparDadosDoBanco(musicasExistentes);
        Assert.NotNull(response);
        Assert.True(response.Count() > 0);
    }
}
