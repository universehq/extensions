namespace Syntactic.UnitTests;

public class IntExtensionsTests
{
    private static readonly int[] Expected = [1, 2, 3, 4];

    [Fact]
    public void To_InclusiveRange_ReturnsAllValues()
    {
        var result = 1.To(4).ToList();
        Assert.Equal(Expected, result);
    }

    [Fact]
    public void To_SameStartEnd_ReturnsSingleValue()
    {
        var result = 5.To(5).ToList();
        Assert.Single(result);
        Assert.Equal(5, result[0]);
    }

    [Fact]
    public void To_StartGreaterThanEnd_ReturnsEmpty()
    {
        var result = 10.To(7).ToList();
        Assert.Empty(result);
    }

    [Fact]
    public void To_NegativeRange_WorksCorrectly()
    {
        var result = (-3).To(1).ToList();
        Assert.Equal(new[] { -3, -2, -1, 0, 1 }, result);
    }

    [Fact]
    public void To_LargeRange_PerformanceSmoke()
    {
        // smoke test - ensure it can iterate a moderately large range without error
        var count = 0;
        foreach (var i in 0.To(10000))
        {
            count++;
        }
        Assert.Equal(10001, count);
    }
}
