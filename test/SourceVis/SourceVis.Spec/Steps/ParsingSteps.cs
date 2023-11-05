using NUnit.Framework;
using SourceVis.Spec.Hooks;
using SourceVisCore.AST;

namespace SourceVis.Spec.Steps;

[Binding]
public class ParsingSteps : LogHelper
{
    private readonly ScenarioContext _scenarioContext;

    public ParsingSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    [Given(@"we parse '(.*)'")]
    public async Task GivenWeParseTheFollowingCode(string filename)
    {
        var finalPath = Directory.GetCurrentDirectory() + "../../../../TestFiles/" + filename;
        
        _scenarioContext.Set(Parser.ParseFromSource(await File.ReadAllTextAsync(finalPath)));
    }

    [Then(@"there (?:is|are) (\d*) class(?:es|)")]
    public void ThenThereIsClass(int classCount)
    {
        LogAssert(() => Assert.That(_scenarioContext.Get<Project>().Classes.Count, Is.EqualTo(classCount)));
    }

    [Then(@"there is a class named '(.*)'")]
    public void ThenThereIsAClassNamed(string className)
    {
        LogAssert(() => Assert.That(_scenarioContext.Get<Project>().Classes.Select(pair => pair.Key.Split('.')[^1]),
            Contains.Item(className.Split('.')[^1])));
    }

    [Then(@"The class '(.*)' uses '(.*)' as (.*) (\d*) times?")]
    public void ThenTheClassDependsOnAsTimeS(string className, string dependencyName, DependencyType type,
        int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _scenarioContext.Get<Project>().Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.First(ContainsKey<Dependency>(dependencyName)).Value[type],
            Is.EqualTo(dependencyCount)));
    }

    [Then(@"The class '(.*)' uses '(.*)' (\d*) times?")]
    public void ThenTheClassDependsOnTimes(string className, string dependencyName, int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _scenarioContext.Get<Project>().Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.First(ContainsKey<Dependency>(dependencyName)).Value.Total,
            Is.EqualTo(dependencyCount)));
    }

    [Then(@"The class '(.*)' has (\d*) dependency")]
    [Then(@"The class '(.*)' has (\d*) dependencies")]
    public void ThenTheClassHasDependencies(string className, int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _scenarioContext.Get<Project>().Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.Count(),
            Is.EqualTo(dependencyCount)));
    }

    private static Func<KeyValuePair<string, T>, bool> ContainsKey<T>(string key)
    {
        return pair =>
        {
            var rhs = key.Split('.')[^1];
            var lhs = pair.Key.Split('.')[^1];
            return lhs.Equals(rhs);
        };
    }
}