using Bogus;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Collections.ObjectModel;
using System.Globalization;

namespace ScreenSound.Integration.Test.DataBuilders;

public class MusicaDataBuilder : Faker<Musica>
{    
    public string? Nome { get; set; }
    public int ArtistaId { get; set; } = 0;
    public int AnoLancamento { get; set; } = 0;
    public ICollection<Genero> Generos { get; set; } = new Collection<Genero>();

    public MusicaDataBuilder()
    {
        var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");
        
        CustomInstantiator(f =>
        {
            var artista = new ArtistaDataBuilder().Build(1).FirstOrDefault();
            var genero = new GeneroDataBuilder().Build();

            string nome = string.IsNullOrEmpty(Nome) ? lorem.Sentence(2) : Nome;
            int artistaId = ArtistaId == 0 ? artista.Id : ArtistaId;
            int anoLancamento = AnoLancamento == 0 ? f.Random.Number(1900, 2025) : AnoLancamento;
            ICollection<Genero> generos = new Collection<Genero>();

            if (Generos.Count == 0)
                generos.Add(genero);
            else
                generos = Generos;

            return new Musica()
            {
                Nome = nome,
                ArtistaId = artistaId,
                AnoLancamento = anoLancamento,
                Generos = generos,
                Artista = artista
            };
        });
    }

    public IEnumerable<Musica> Build(int qtdDeDados = 1)
    {
        return Generate(qtdDeDados);
    }
}
