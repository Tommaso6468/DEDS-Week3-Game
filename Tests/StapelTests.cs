namespace Tests;

using Main;
using Xunit;

public class StapelTests
{
    [Fact]
    public void StapelDuwEnPakEnkel()
    {
        var stapel = new Stapel<int>();
        var cijfer = 1;

        stapel.Duw(cijfer);
        var resultaatPak = stapel.Pak();

        Assert.Equal(resultaatPak, cijfer);
    }

    [Fact]
    public void StapelDuwEnPakMeerdere()
    {
        var stapel = new Stapel<string>();
        string[] strings = { "a", "b", "c" };
        var verwachtResultaat = new List<string>();
        verwachtResultaat.Add(strings[2]);
        verwachtResultaat.Add(strings[1]);
        verwachtResultaat.Add(strings[0]);

        stapel.Duw(strings[0]);
        stapel.Duw(strings[1]);
        stapel.Duw(strings[2]);

        var resultaatPak = new List<string>();
        resultaatPak.Add(stapel.Pak());
        resultaatPak.Add(stapel.Pak());
        resultaatPak.Add(stapel.Pak());

        Assert.Equal(resultaatPak, verwachtResultaat);
    }

    [Fact]
    public void StapelPakLeeg()
    {
        var stapel = new Stapel<int>();
        var resultaatPak = stapel.Pak();

        Assert.Equal(resultaatPak, default);
    }
}