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
public class Musica_DELETE
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;
    private readonly GeneroFakeData _generoFakeData;
    private readonly ArtistaFakeData _artistaFakeData;

    public Musica_DELETE(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _generoFakeData = new GeneroFakeData(app);
        _artistaFakeData = new ArtistaFakeData(app);
    }

    [Fact]
    public async Task Deleta_Musica_Por_Id()
    {
        var musica = _musicaFakeData.CriarDadosFake().FirstOrDefault();
        using var client = _app.CreateClient();

        _generoFakeData.LimparDadosDoBanco(musica.Id);        
        var response = await client.DeleteAsync("/Musicas/" + musica.Id);
        _artistaFakeData.LimparDadosDoBanco(musica.ArtistaId);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
}
