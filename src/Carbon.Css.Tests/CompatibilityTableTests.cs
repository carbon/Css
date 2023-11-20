namespace Carbon.Css.Tests;

public class CompatibilityTableTests
{
    [Fact]
    public void TestIsDefined()
    {
        Assert.False(default(CompatibilityTable).IsDefined);
        Assert.True(new CompatibilityTable(1, 2, 3, 4).IsDefined);
    }
}