using FluentAssertions;

namespace RestApiBackend.Test;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        "dbuzs".Should().BeEmpty();
    }
}