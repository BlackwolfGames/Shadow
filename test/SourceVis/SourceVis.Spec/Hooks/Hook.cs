using NUnit.Framework;

namespace SourceVis.Spec.Hooks;

[Binding]
public class LogHelper
{
    private string FeatureFilePath = null;
    string currentFeatureName = null;
    string currentFeatureDesc = null;
    string currentScenarioText = null;
    string currentStepText = null;

    [BeforeScenario]
    public void CaptureScenarioInformation(ScenarioContext scenario, FeatureContext feature)
    {
        FeatureFilePath = feature.FeatureInfo.FolderPath;
        currentFeatureName = feature.FeatureInfo.Title.Split('-')[1].Trim();
        currentFeatureDesc = feature.FeatureInfo.Description;
        currentScenarioText = scenario.ScenarioInfo.Title;
    }

    [BeforeStep]
    public void CaptureStepInformation()
    {
        var stepInfo = ScenarioStepContext.Current.StepInfo;
        currentStepText = stepInfo.Text;

        // If you could access the line number, store it as well
    }

    public void LogAssert(Action assertion)
    {
        try
        {
            assertion();
        }
        catch (Exception ex)
        {
            var currentDir = Directory.GetCurrentDirectory();
            currentDir = Directory.GetParent(currentDir)?.Parent?.Parent?.ToString() ?? "Failure";

            var fullFilePath = $"{currentDir}\\{FeatureFilePath}\\{currentFeatureName}.feature";
            string[] lines = File.ReadAllLines(fullFilePath);
            int lineNr = Array.FindIndex(lines, s => s.Contains(currentStepText)) + 1;
            throw new AssertionException(
                $"{ex.Message}\n" +
                $"\n" +
                $"Feature: '{currentFeatureName}: {currentFeatureDesc}\n" +
                $"- Scenario: {currentScenarioText}\n" +
                $"-- step: `{currentStepText}`\n" +
                $"\n " +
                $"in {currentDir}\\{FeatureFilePath}\\{currentFeatureName}.feature:{lineNr}");
        }
    }
}