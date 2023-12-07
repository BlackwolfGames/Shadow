namespace SourceVis.Spec.Steps.LayoutMetrics;

[Binding]
public sealed class ClusteringMetrics
{
  // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

  private readonly ScenarioContext _scenarioContext;

  public ClusteringMetrics(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [When("we generate a louvain clustering report")]
  public void GivenWeGenerateALouvainClusteringReport()
  {
    _scenarioContext.Pending();
  }

  [When("we generate a spatial clustering report")]
  public void GivenWeGenerateASpatialClusteringReport()
  {
    _scenarioContext.Pending();
  }


  [Then("the result should be (.*)")]
  public void ThenTheResultShouldBe(int result)
  {
    _scenarioContext.Pending();
  }
}