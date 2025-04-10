using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
public class Musica_PUT
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly MusicaFakeData _musicaFakeData;
    private readonly GeneroFakeData _generoFakeData;
    private readonly ArtistaFakeData _artistaFakeData;

    public Musica_PUT(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _musicaFakeData = new MusicaFakeData(app);
        _generoFakeData = new GeneroFakeData(app);
        _artistaFakeData = new ArtistaFakeData(app);
    }

    [Fact]
    public async Task Atualiza_Musica()
    {
        var musica = _musicaFakeData.CriarDadosFake().FirstOrDefault();
        musica.Nome = "Faroeste Caboclo";
        musica.AnoLancamento = 1996;
        musica.Artista = null;
        musica.Generos = null;
        using var client = _app.CreateClient();

        var response = await client.PutAsJsonAsync("/Musicas/", musica);

        //Tem que ser nessa ordem para conseguir pegar o generoId na tabela GeneroMusica
        //E tambem por conta das FKs
        _generoFakeData.LimparDadosDoBanco(musica.Id);
        _musicaFakeData.LimparDadosDoBanco(musica.Id);
        _artistaFakeData.LimparDadosDoBanco(musica.ArtistaId);
        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
