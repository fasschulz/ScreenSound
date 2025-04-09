﻿using ScreenSound.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ScreenSound.Integration.Test.FakeData;
using ScreenSound.Integration.Test.Fixture;

namespace ScreenSound.Integration.Test.Scenarios.ArtistaTests;

[Collection(nameof(ScreeSoundWebApplicationFactoryCollection))]
public class Artista_PUT : IDisposable
{
    private readonly ScreenSoundWebApplicationFactory app;
    private readonly ArtistaFakeData _artistaFakeData;

    public Artista_PUT(ScreenSoundWebApplicationFactory app)
    {
        this.app = app;
        _artistaFakeData = new ArtistaFakeData(app);
        _artistaFakeData.CriarDadosFake(5);
    }

    public void Dispose()
    {
        _artistaFakeData.LimparDadosDoBanco();
    }

    [Fact]
    public async Task Atualiza_ArtistaAsync()
    {
        using var client = app.CreateClient();

        var artistaExistente = await app.Context.Artistas.FirstOrDefaultAsync();
        
        var artista = new ArtistaRequestEdit(
            artistaExistente.Id, artistaExistente.Nome, artistaExistente.Bio);

        var response = await client.PutAsJsonAsync("/Artistas", artista);

        Assert.NotNull(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Retorna_Not_Found_Quando_Atualiza_Artista_Inexistente()
    {
        using var client = app.CreateClient();
        
        var artista = new ArtistaRequestEdit(
            0, "Joao Ninguem", "Bio");

        var response = await client.PutAsJsonAsync("/Artistas", artista);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
