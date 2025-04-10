using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Integration.Test.Fixture;
using ScreenSound.Modelos;

namespace ScreenSound.Integration.Test.FakeData;

public class MusicaFakeData
{
    private readonly ScreenSoundWebApplicationFactory _app;

    public MusicaFakeData(ScreenSoundWebApplicationFactory app)
    {
        _app = app;
    }

    public IEnumerable<Musica> CriarDadosFake(int qtdDeDados = 1)
    {
        var listaDeMusicas = _app.Context.Musicas.Include(m => m.Generos).ToList();
        if (listaDeMusicas is null)
            listaDeMusicas = new List<Musica>();

        if (listaDeMusicas.Count() == 0)
        {
            var musicaDataBuilder = new MusicaDataBuilder().Build(qtdDeDados);
            listaDeMusicas.AddRange(musicaDataBuilder);

            _app.Context.Musicas.AddRange(listaDeMusicas);
            _app.Context.SaveChanges();
        }

        return listaDeMusicas;
    }
    public void LimparDadosDoBanco()
    {
        _app.Context.Database.ExecuteSqlRaw("DELETE FROM Musicas");
        _app.Context.Database.ExecuteSqlRaw("DELETE FROM Generos");
        _app.Context.Database.ExecuteSqlRaw("DELETE FROM Artistas");
    }

    public void LimparDadosDoBanco(IEnumerable<Musica> musicas)
    {
        foreach(var musica in musicas)
        {
            _app.Context.Musicas
                .Where(m => m.Id == musica.Id)
                .ExecuteDelete();
        }
    }

    public void LimparDadosDoBanco(Musica musica)
    {
        _app.Context.Remove(musica);
        _app.Context.SaveChanges();
    }

    public void LimparDadosDoBanco(int musicaId)
    {
        _app.Context.Musicas
            .Where(m => m.Id == musicaId)
            .ExecuteDelete();
    }
}
