using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ScreenSound.Integration.Test.Scenarios.MusicaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Musica_POST
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;
    private readonly ArtistaFakeData _artistaFakeData;
    private readonly GeneroFakeData _generoFakeData;
    private readonly MusicaDataBuilder _musicaDataBuilder;

    public Musica_POST(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _artistaFakeData = new ArtistaFakeData(app);
        _generoFakeData = new GeneroFakeData(app);
        _musicaDataBuilder = new MusicaDataBuilder();
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

        JsonSerializerOptions options = new(JsonSerializerDefaults.Web)
        {
            WriteIndented = true
        };
        string jsonString = await response.Content.ReadAsStringAsync();
        var musicaCadastrada = JsonSerializer.Deserialize<Musica>(jsonString, options);

        //Tem que ser nessa ordem para conseguir pegar o generoId na tabela GeneroMusica
        //E tambem por conta das FKs
        _generoFakeData.LimparDadosDoBanco(musicaCadastrada.Id);
        _musicaFakeData.LimparDadosDoBanco(musicaCadastrada.Id);
        _artistaFakeData.LimparDadosDoBanco(artista.Id);        
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
