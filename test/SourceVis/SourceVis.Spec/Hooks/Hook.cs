using NUnit.Framework;

namespace SourceVis.Spec.Hooks;

[Binding]
public class LogHelper
{
  private string _featureFilePath = string.Empty;
  private string _currentFeatureName = string.Empty;
  private string _currentFeatureDesc = string.Empty;
  private string _currentScenarioText = string.Empty;
  private string _currentStepText = string.Empty;

  [BeforeScenario]
  public void CaptureScenarioInformation(ScenarioContext scenario, FeatureContext feature)
  {
    _featureFilePath = feature.FeatureInfo.FolderPath;
    _currentFeatureName = feature.FeatureInfo.Title.Split('-')[1].Trim();
    _currentFeatureDesc = feature.FeatureInfo.Description;
    _currentScenarioText = scenario.ScenarioInfo.Title;
  }

  [BeforeStep]
  public void CaptureStepInformation()
  {
    var stepInfo = ScenarioStepContext.Current.StepInfo;
    _currentStepText = stepInfo.Text;

    // If you could access the line number, store it as well
  }

  protected void LogAssert(Action assertion)
  {
    try
    {
      assertion();
    }
    catch (Exception ex)
    {
      var currentDir = Directory.GetCurrentDirectory();
      currentDir = Directory.GetParent(currentDir)?.Parent?.Parent?.ToString() ?? "Failure";

      var fullFilePath = $"{currentDir}\\{_featureFilePath}\\{_currentFeatureName}.feature";
      var lines = File.ReadAllLines(fullFilePath);
      var lineNr = Array.FindIndex(lines, s => s.Contains(_currentStepText)) + 1;
      throw new AssertionException(
        $"{ex.Message}\n"
        + $"\n"
        + $"Feature: '{_currentFeatureName}: {_currentFeatureDesc}\n"
        + $"- Scenario: {_currentScenarioText}\n"
        + $"-- step: `{_currentStepText}`\n"
        + $"\n "
        + $@"in {currentDir}\{_featureFilePath}\{_currentFeatureName}.feature:{lineNr}");
    }
  }
}
