using ShadowEngine;

namespace ShadowTest;

public class Tests
{

    [Test]
    public void Engine_is_initialized_as_non_running()
    {
        //Arrange
        var engine = new Engine();
        //Act
        //Assert
        Assert.That(engine.State, Is.EqualTo(EngineState.Inactive));
    }
}