﻿using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.DataBuilders;
using ScreenSound.Modelos;

namespace ScreenSound.Integration.Test.FakeData;

public class ArtistaFakeData
{
    private readonly ScreenSoundWebApplicationFactory app;

    public ArtistaFakeData(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
    }

    public IEnumerable<Artista> CriarDadosFake(int qtdDeDados = 1)
    {
        var listaDeArtistas = app.Context.Artistas.ToList();
        if (listaDeArtistas is null)
            listaDeArtistas = new List<Artista>();

        if (listaDeArtistas.Count() == 0)
        {
            var artistaDataBuilder = new ArtistaDataBuilder().Build(qtdDeDados);
            listaDeArtistas.AddRange(artistaDataBuilder);

            app.Context.Artistas.AddRange(listaDeArtistas);
            app.Context.SaveChanges();
        }

        return listaDeArtistas;
    }

    public void LimparDadosDoBanco()
    {
        app.Context.Database.ExecuteSqlRaw("DELETE FROM Artistas");
    }
}
