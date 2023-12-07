using System.Numerics;
using System.Text;
using NCalc;
using NUnit.Framework;
using SourceVisCore.Graphing;
using SourceVisCore.Layout;
using SourceVisCore.Layout.Forces;

namespace SourceVis.Spec.Steps.LayoutMetrics;

[Binding]
public class LayoutSteps
{
  private readonly ScenarioContext _scenarioContext;

  public LayoutSteps(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Given(@"we create a projection for the graph")]
  public void GivenWeCreateAProjectionForTheGraph()
  {
    _scenarioContext.Set(GraphProjection.FromGraph(_scenarioContext.Get<IDependencyGraph>()));
  }

  [Then(@"the (projection of '.*') is at (\([^)]*\))")]
  public void ThenTheProjectionOfIsAt(ProjectedNode node, Vector2 expected)
  {
    Assert.That(node.Position.X, Is.EqualTo(expected.X).Within(0.01f));
    Assert.That(node.Position.Y, Is.EqualTo(expected.Y).Within(0.01f));
  }

  [Then(@"the (projection of '.*') is not at (\([^)]*\))")]
  public void ThenTheProjectionOfIsNotAt(ProjectedNode node, Vector2 expected)
  {
    Assert.That(node.Position.X, Is.Not.EqualTo(expected.X).Within(0.1f));
    Assert.That(node.Position.Y, Is.Not.EqualTo(expected.Y).Within(0.1f));
  }

  [Then(@"the (projection of '.*') is (\d*) away from (\([^)]*\))")]
  public void ThenTheProjectionOfIsWithinDFrom(ProjectedNode node, float distance, Vector2 expected)
  {
    Assert.That(Vector2.Distance(node.Position, expected), Is.EqualTo(distance).Within(0.01f));
  }

  [When(@"moving the (projection of '.*') to (\([^)]*\))")]
  public static void WhenMovingTheProjectionOfTo(ProjectedNode testCallConsole, Vector2 p1)
  {
    testCallConsole.Position = p1;
    testCallConsole.Velocity = new Vector2();
  }

  [When(@"the graph is relaxed for (.*) steps?")]
  public void WhenTheGraphIsRelaxedForStep(int p0)
  {
    GraphProjection? projection = _scenarioContext.Get<GraphProjection>();
    LayoutOptimizer? optimizer = _scenarioContext.Get<LayoutOptimizer>();
    for (var i = 0; i < p0; i++)
      optimizer.Optimize(projection);
  }


  [When(@"the graph is relaxed until still")]
  public void WhenTheGraphIsRelaxedUntilStill()
  {
    GraphProjection? projection = _scenarioContext.Get<GraphProjection>();
    LayoutOptimizer? optimizer = _scenarioContext.Get<LayoutOptimizer>();

    var overallCounter = 0;
    var stillnessCounter = 0;
    const int stillnessThreshold = 50; // Number of iterations to consider stillness
    const int maxIterations = 10000;
    const double velocityThreshold = 0.01;

    while (overallCounter++ < maxIterations)
    {
      optimizer.Optimize(projection);
      if (projection.Nodes.Average(node => node.Velocity.Length()) >= velocityThreshold)
      {
        stillnessCounter = 0; // Reset stillness counter if movement is detected
        continue;
      }

#pragma disable csharpsquid:S2589
      if (++stillnessCounter >= stillnessThreshold) break;
#pragma enable csharpsquid:S2589
    }
  }

  [Given(@"we start to relax the layout")]
  public void GivenWeStartToRelaxTheLayout()
  {
    _scenarioContext.Set(new LayoutOptimizer());
  }

  [Then(@"we're at step (.*)")]
  public void ThenWereAtStep(int p0)
  {
    Assert.That(_scenarioContext.Get<LayoutOptimizer>().Step, Is.EqualTo(p0));
  }

  [When(@"we (enable|disable) the '(.*)' force")]
  public void WhenWeEnableTheForce(bool isEnabled, Forces forceType)
  {
    if (isEnabled)
      _scenarioContext.Get<LayoutOptimizer>().AddForce(forceType);
    else
      _scenarioContext.Get<LayoutOptimizer>().RemoveForce(forceType);
  }

  [When(@"we enable the '(.*)' force with strength (\d*)")]
  public void WhenWeEnableTheForce(bool isEnabled, Forces forceType, float strength)
  {
    _scenarioContext.Get<LayoutOptimizer>().AddForce(forceType, strength);
  }

  [Then(@"there is (.*) active force")]
  public void ThenThereIsActiveForce(int p0)
  {
    Assert.That(_scenarioContext.Get<LayoutOptimizer>().Forces, Has.Count.EqualTo(p0));
  }

  [When(@"we remember the (projection of '.*') as '(.*)'")]
  public void WhenWeRememberTheProjectionOfNode(ProjectedNode node, string variable)
  {
    _scenarioContext.Get<Dictionary<string, (Vector2, Func<Vector2>)>>().Add(variable, (node.Position, () => node.Position));
  }

  [Given(@"we want to memorize positions")]
  public void GivenWeWantToMemorizePositions()
  {
    _scenarioContext.Set(new Dictionary<string, (Vector2, Func<Vector2>)>());
  }

  public enum MovementDir
  {
    Up,
    Left,
    Right,
    Down,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
  }

  [Then(@"'(.*)' has moved nowhere")]
  public void ThenHasMovedNowhere(string p0)
  {
    var delta = _scenarioContext.Get<Dictionary<string, (Vector2, Func<Vector2>)>>()[p0];
    Assert.That(delta.Item2(), Is.EqualTo(delta.Item1).Within(0.01f));
  }

  [Then(@"'(.*)' has moved (up|left|right|down|upleft|upright|downleft|downright) by ([\d\.]*)")]
  public void ThenHasMovedTo(string p0, MovementDir movement, float distance)
  {
    (Vector2 initialPosition, var currentPositionFunc) = _scenarioContext.Get<Dictionary<string, (Vector2, Func<Vector2>)>>()[p0];
    Vector2 currentPosition = currentPositionFunc();

    // Calculate the actual distance moved
    double actualDistanceMoved = Vector2.Distance(initialPosition, currentPosition);

    // Calculate the direction of movement
    var directionMoved = Vector2.Normalize(currentPosition - initialPosition);

    // Define acceptable angle ranges for each direction
    var directionRanges = new Dictionary<MovementDir, (float min, float max)>
    {
      { MovementDir.Right, (350, 370) }, // Assuming straight up is 0 degrees and the angle increases clockwise
      { MovementDir.UpRight, (10, 80) },
      { MovementDir.Up, (80, 100) },
      { MovementDir.UpLeft, (100, 170) },
      { MovementDir.Left, (170, 190) },
      { MovementDir.DownLeft, (190, 260) },
      { MovementDir.Down, (260, 280) },
      { MovementDir.DownRight, (280, 350) }
      // Define other directions similarly...
    };

    // Check if the direction of movement is within the acceptable range
    var movementAngleDegrees = Math.Atan2(directionMoved.Y, directionMoved.X) * (180 / Math.PI);
    movementAngleDegrees += 360;
    movementAngleDegrees %= 360;
    if (movementAngleDegrees <= 10)
      movementAngleDegrees += 360;
    var (minAngle, maxAngle) = directionRanges[movement];

    // Assert that both direction and distance are correct
    Assert.That(actualDistanceMoved, Is.EqualTo(distance).Within(0.1f), $"Object {p0} did not move the right distance.");
    if (actualDistanceMoved != 0)
      Assert.That(movementAngleDegrees, Is.GreaterThanOrEqualTo(minAngle).And.LessThanOrEqualTo(maxAngle), $"Object {p0} did not move the right direction.");
  }

  [Then(@"printing the graph yields the following:")]
  public void ThenPrintingTheGraphYieldsTheFollowing(string multilineText)
  {
    var sb = new StringBuilder();
    _scenarioContext.Get<GraphProjection>().Print(new StringWriter(sb));
    Assert.That(sb.ToString().Trim(), Is.EquivalentTo(multilineText.Trim()));
  }

  [Then(@"the average velocity is (.*) ('[^\']*')")]
  public void ThenTheTotalVelocityIs(EqualClass eq, Expression expectedVelocity)
  {
    GraphProjection? projection = _scenarioContext.Get<GraphProjection>();
    Assert.That(projection.Nodes.Average(node => node.Velocity.Length()), eq.TestAs(expectedVelocity.Evaluate()));
  }
}