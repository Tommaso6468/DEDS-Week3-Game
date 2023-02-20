namespace Tests;

using Main;
using Xunit;

public class StapelTests<T>
{
    [Fact]
    public void StapelDuwEnPak()
    {
        var stapel = new Stapel<int>();
        var cijfer = 1;

        stapel.Duw(ref cijfer);
        var resultaatPak = stapel.Pak();

        Assert.Equal(resultaatPak, cijfer);
    }
}