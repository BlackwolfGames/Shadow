using NUnit.Framework;
using SourceVisCore.AST;

namespace SourceVisSpec.Steps;

[Binding]
public class ParsingSteps
{
    private Project _parsed = new();

    [Given(@"we parse '(.*)'")]
    public async Task GivenWeParseTheFollowingCode(string filename)
    {
        var finalPath = Directory.GetCurrentDirectory() + "../../../../TestFiles/" + filename;
        _parsed = Parser.ParseFromSource(await File.ReadAllTextAsync(finalPath));
    }

    [Then(@"there (?:is|are) (.*) class(?:es|)")]
    public void ThenThereIsClass(int classCount)
    {
        Assert.That(_parsed.Classes.Count, Is.EqualTo(classCount));
    }
    [Then(@"there is a class named '(.*)'")]
    public void ThenThereIsAClassNamed(string className)
    {
        Assert.That(_parsed.Classes.Select(pair => pair.Key), Contains.Item(className));
    }

    [Then(@"The class '(.*)' depends on '(.*)' as (.*) (.*) times?")]
    public void ThenTheClassDependsOnAsTimeS(string className, string dependencyName, DependencyType type, int dependencyCount)
    {
        Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.First(ContainsKey<Dependency>(dependencyName)).Value[type],
            Is.EqualTo(dependencyCount));
    }

    [Then(@"The class '(.*)' depends on '(.*)' (.*) times?")]
    public void ThenTheClassDependsOnTimes(string className, string dependencyName, int dependencyCount)
    {
        Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.First(ContainsKey<Dependency>(dependencyName)).Value.Total,
            Is.EqualTo(dependencyCount));
    }

    [Then(@"The class '(.*)' has (.*) dependency")]
    [Then(@"The class '(.*)' has (.*) dependencies")]
    public void ThenTheClassHasDependencies(string className, int dependencyCount)
    {
        Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.Count(),
            Is.EqualTo(dependencyCount));
    }

    [Then(@"The class '(.*)' uses '(.*)'")]
    public void ThenTheClassUses(string className, string dependencyName)
    {
        Assert.That(_parsed.Classes.First(ContainsKey<Class>(className)).Value.Dependencies.Select(pair => pair.Key), Contains.Item(dependencyName));
    }
    
    private static Func<KeyValuePair<string,T>, bool> ContainsKey<T>(string key)
    {
        return pair => Equals(pair.Key, key);
    }
}