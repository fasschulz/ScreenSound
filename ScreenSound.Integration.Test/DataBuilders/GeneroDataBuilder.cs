using Bogus;
using ScreenSound.Shared.Modelos.Modelos;
using System.Globalization;

namespace ScreenSound.Integration.Test.DataBuilders;

public class GeneroDataBuilder : Faker<Genero>
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }

    public GeneroDataBuilder()
    {
        var lorem = new Bogus.DataSets.Lorem(locale: "pt_BR");
        CustomInstantiator(f =>
        {
            string nome = string.IsNullOrEmpty(Nome) ? lorem.Sentence(1) : Nome;
            string descricao = string.IsNullOrEmpty(Descricao) ? lorem.Sentence(3) : Descricao;
            return new Genero() { Nome = nome, Descricao = descricao };
        });
    }

    public Genero Build() => Generate();
}
