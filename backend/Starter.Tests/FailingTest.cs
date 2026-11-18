using Xunit;

namespace Starter.Tests;

public class FailingTest
{
    [Fact]
    public void Fails_on_purpose()
    {
        Assert.Equal(1, 2);
    }
}