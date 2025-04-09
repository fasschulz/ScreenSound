using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScreenSound.Integration.Test.Scenarios.ArtistaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_DELETE : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory _app;
    private readonly ArtistaFakeData _artistaFakeData;

    public Artista_DELETE(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
        _artistaFakeData = new ArtistaFakeData(app);
        _artistaFakeData.CriarDadosFake(5);
    }

    public void Dispose()
    {
        _artistaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Deleta_Artista_Por_Id()
    {        
        var artistaExistente = await _app.Context.Artistas.FirstOrDefaultAsync();
        using var client = _app.CreateClient();

        var response = await client.DeleteAsync("/Artistas/" + artistaExistente.Id);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task Retorna_Not_Found_Quando_Deleta_Artista_Inexistente()
    {
        using var client = _app.CreateClient();

        var response = await client.DeleteAsync("/Artistas/" + 0);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
