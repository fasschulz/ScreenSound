using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Integration.Test.Scenarios.MusicaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Musica_PUT : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;

    public Musica_PUT(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _musicaFakeData.CriarDadosFake(5);
    }

    public void Dispose()
    {
        _musicaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Atualiza_Musica()
    {        
        var musica = await _app.Context.Musicas.FirstOrDefaultAsync();
        musica.Nome = "Faroeste Caboclo";
        musica.AnoLancamento = 1996;
        musica.Artista = null;
        musica.Generos = null;
        using var client = _app.CreateClient();

        var response = await client.PutAsJsonAsync("/Musicas/", musica);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
