using NUnit.Framework;
using SourceVisCore.AST;

namespace SourceVisSpec.Steps;

[Binding]
public class ParsingSteps
{
    private Project _parsed;

    [Given(@"we parse the following code")]
    public async Task GivenWeParseTheFollowingCode(string sourceCode)
    {
        _parsed = await Parser.ParseFromSource(sourceCode);
    }

    [Then(@"there is (.*) class")]
    public void ThenThereIsClass(int p0)
    {
        Assert.That(_parsed.Classes.Count, Is.EqualTo(1));
    }

    private Func<KeyValuePair<string,T>, bool> ContainsKey<T>(string key)
    {
        return pair => Equals(pair.Key, key);
    }
    [Then(@"The class '(.*)' depends on '(.*)' (.*) times?")]
    public void ThenTheClassDependsOnTimes(string className, string dependencyName, int dependencyCount)
    {
        Assert.That(_parsed.Classes.Select(pair => pair.Key), Contains.Item(className));
        Assert.That(_parsed.Classes.First(ContainsKey<Class>(className)).Value.Dependencies.Select(pair => pair.Key), Contains.Item(dependencyName));
        Assert.That(
            _parsed.Classes.First(ContainsKey<Class>(className)).Value
                .Dependencies.First(ContainsKey<int>(dependencyName)).Value,
            Is.EqualTo(dependencyCount));
    }
}