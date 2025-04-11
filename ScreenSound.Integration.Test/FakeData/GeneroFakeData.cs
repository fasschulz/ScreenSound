using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;

namespace ScreenSound.Integration.Test.FakeData;

public class GeneroFakeData
{
    private readonly ScreenSoundWebApplicationFactory _app;

    public GeneroFakeData(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
    }

    public void LimparDadosDoBanco(int musicaId)
    {
        var generoIds = _app.Context.Database
        .SqlQuery<int>($"select [GenerosId] AS [Value] from [GeneroMusica] Where [MusicasId] = {musicaId}")
        .ToList();

        foreach(var generoId in generoIds)
        {
            _app.Context.Generos
            .Where(g => g.Id == generoId)
            .ExecuteDelete();
        }        
    }
    public void LimparDadosDoBanco(IEnumerable<Musica> musicas)
    {
        foreach(var musica in musicas)
        {
            var generoIds = _app.Context.Database
            .SqlQuery<int>($"select [GenerosId] AS [Value] from [GeneroMusica] Where [MusicasId] = {musica.Id}")
            .ToList();

            foreach (var generoId in generoIds)
            {
                _app.Context.Generos
                .Where(g => g.Id == generoId)
                .ExecuteDelete();
            }
        }
        
    }
}
