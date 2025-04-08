using Newtonsoft.Json;
using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ScreenSound.Integration.Test.Scenarios.MusicaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Musica_POST : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;
    private readonly ArtistaFakeData _artistaFakeData;
    private readonly MusicaDataBuilder _musicaDataBuilder;

    public Musica_POST(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _artistaFakeData = new ArtistaFakeData(app);
        _musicaDataBuilder = new MusicaDataBuilder();
    }

    public void Dispose()
    {
        _musicaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Cadastra_Musica()
    {
        var artista = _artistaFakeData.CriarDadosFake().FirstOrDefault();
        var musica = _musicaDataBuilder.Build().FirstOrDefault();
        musica.ArtistaId = artista.Id;
        musica.Artista = null;
        using var client = _app.CreateClient();

        var response = await client.PostAsJsonAsync("/Musicas/", musica);
        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
