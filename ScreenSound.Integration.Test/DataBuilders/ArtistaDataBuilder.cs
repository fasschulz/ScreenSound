using Bogus;
using ScreenSound.Modelos;

namespace ScreenSound.Integration.Test.DataBuilders;

internal class ArtistaDataBuilder : Faker<Artista>
{
    public string? Nome { get; set; }
    public string? Bio { get; set; }
    public ArtistaDataBuilder()
    {
        var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");
        CustomInstantiator(f =>
        {
            string nome = string.IsNullOrEmpty(Nome) ? lorem.Sentence(2) : Nome;
            string bio = string.IsNullOrEmpty(Bio) ? lorem.Sentence(10) : Bio;
            return new Artista(nome, bio);
        });
    }

    public IEnumerable<Artista> Build(int qtdDeDadosFakes = 1)
    {
        return Generate(qtdDeDadosFakes);
    }
}
