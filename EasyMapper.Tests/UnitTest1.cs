using FluentAssertions;

namespace EasyMapper.Tests;

public class Source
{
    public string StringProperty { get; set; }
    public int IntProperty { get; set; }
    public double DoubleProperty { get; set; }
}

public class Target
{
    public string StringProperty { get; set; }
    public int IntProperty { get; set; }
    public float FloatProperty { get; set; }
    public double DoubleProperty { get; set; }
}

public class UnitTest1
{

    [Fact]
    public void HydrateFrom_ShouldPopulateTargetPropertiesFromSource()
    {
        // Arrange
        var source = new Source
        {
            StringProperty = "Test",
            IntProperty = 42,
            DoubleProperty = 3.14
        };

        var target = new Target();

        // Act
        target.HydrateFrom(source);

        // Assert
        target.StringProperty.Should().Be(source.StringProperty);
        target.IntProperty.Should().Be(source.IntProperty);
        target.FloatProperty.Should().Be((float)source.DoubleProperty);
        target.DoubleProperty.Should().Be(source.DoubleProperty);
    }

    [Fact]
    public void HydrateFrom_ShouldNotThrowException_WhenSourceIsNull()
    {
        // Arrange
        Source source = null;
        var target = new Target();

        // Act
        var action = () => target.HydrateFrom(source);

        // Assert
        action.Should().NotThrow();
    }

    [Fact]
    public void HydrateFrom_ShouldNotThrowException_WhenTargetIsNull()
    {
        // Arrange
        var source = new Source
        {
            StringProperty = "Test",
            IntProperty = 42,
            DoubleProperty = 3.14
        };

        Target target = null;

        // Act
        var action = () => target.HydrateFrom(source);

        // Assert
        action.Should().NotThrow();
    }
}