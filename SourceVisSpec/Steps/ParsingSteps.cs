using NUnit.Framework;
using SourceVisCore.AST;
using SourceVisSpec.Hooks;

namespace SourceVisSpec.Steps;

[Binding]
public class ParsingSteps : LogHelper
{
    private Project _parsed = new();
    private string _classPrefix;
    private string _dependencyPrefix;

    [Given(@"we parse '(.*)'")]
    public async Task GivenWeParseTheFollowingCode(string filename)
    {
        var finalPath = Directory.GetCurrentDirectory() + "../../../../TestFiles/" + filename;
        _parsed = Parser.ParseFromSource(await File.ReadAllTextAsync(finalPath));
        _classPrefix = "";
        _dependencyPrefix = "";
    }

    [When(@"we prefix classnames with '(.*)'")]
    public void wePrefixClassnamesWith(string prefix)
    {
        _classPrefix = prefix;
    }

    [When(@"we prefix dependencies with '(.*)'")]
    public void wePrefixDependenciesWith(string prefix)
    {
        _dependencyPrefix = prefix;
    }

    [Then(@"there (?:is|are) (.*) class(?:es|)")]
    public void ThenThereIsClass(int classCount)
    {
        LogAssert(() => Assert.That(_parsed.Classes.Count, Is.EqualTo(classCount)));
    }

    [Then(@"there is a class named '(.*)'")]
    public void ThenThereIsAClassNamed(string className)
    {
        LogAssert(() => Assert.That(_parsed.Classes.Select(pair => pair.Key), Contains.Item(_classPrefix + className)));
    }

    [Then(@"The class '(.*)' uses '(.*)' as (.*) (.*) times?")]
    public void ThenTheClassDependsOnAsTimeS(string className, string dependencyName, DependencyType type,
        int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(_classPrefix + className)).Value
                .Dependencies.First(ContainsKey<Dependency>(_dependencyPrefix + dependencyName)).Value[type],
            Is.EqualTo(dependencyCount)));
    }

    [Then(@"The class '(.*)' uses '(.*)' (.*) times?")]
    public void ThenTheClassDependsOnTimes(string className, string dependencyName, int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(_classPrefix + className)).Value
                .Dependencies.First(ContainsKey<Dependency>(_dependencyPrefix + dependencyName)).Value.Total,
            Is.EqualTo(dependencyCount)));
    }

    [Then(@"The class '(.*)' has (.*) dependency")]
    [Then(@"The class '(.*)' has (.*) dependencies")]
    public void ThenTheClassHasDependencies(string className, int dependencyCount)
    {
        LogAssert(() => Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(_classPrefix + className)).Value
                .Dependencies.Count(),
            Is.EqualTo(dependencyCount)));
    }

    private static Func<KeyValuePair<string, T>, bool> ContainsKey<T>(string key)
    {
        return pair => Equals(pair.Key, key);
    }
}