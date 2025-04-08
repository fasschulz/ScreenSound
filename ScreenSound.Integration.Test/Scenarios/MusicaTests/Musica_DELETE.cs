using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Integration.Test.Scenarios.MusicaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Musica_DELETE : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;

    public Musica_DELETE(ScreenSoundWebApplicationFactory app)
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
    public async Task Deleta_Musica_Por_Id()
    {
        var musica = await _app.Context.Musicas.FirstOrDefaultAsync();
        using var client = _app.CreateClient();

        var response = await client.DeleteAsync("/Musicas/" + musica.Id);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
