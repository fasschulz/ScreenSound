using ScreenSound.API.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ScreenSound.Integration.Test
{
    public class Artista_PUT : IClassFixture<ScreenSoundWebApplicationFactory>
    {
        private readonly ScreenSoundWebApplicationFactory app;

        public Artista_PUT(ScreenSoundWebApplicationFactory app)
        {
            this.app = app;
        }

        [Fact]
        public async Task Atualiza_ArtistaAsync()
        {
            using var client = app.CreateClient();

            var artistaExistente = await app.Context.Artistas.FirstOrDefaultAsync();
            if(artistaExistente is null)
            {
                artistaExistente = new Modelos.Artista("João Donato", "Eu sou João Donato");
                app.Context.Artistas.Add(artistaExistente);
                await app.Context.SaveChangesAsync();
            }

            var artista = new ArtistaRequestEdit(
                artistaExistente.Id, artistaExistente.Nome, artistaExistente.Bio);

            var response = await client.PutAsJsonAsync("/Artistas", artista);

            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
