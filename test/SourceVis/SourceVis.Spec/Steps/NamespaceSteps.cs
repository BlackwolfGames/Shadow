using NUnit.Framework;
using SourceVis.Spec.Hooks;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class NamespaceSteps : LogHelper
{
    [Then(@"(node '[^']*') (is|is not) in the same namespace as (node '[^']*')")]
    public void ThenTheNodeIsNotInTheSameNamespaceAsNode(INode classA, bool same, INode classB)
    {
        LogAssert(() => Assert.That(classA.Namespaces(),
            same ? Is.EqualTo(classB.Namespaces()) : Is.Not.EqualTo(classB.Namespaces())));
    }

    [Then(@"(node '[^']*') (is|is not) in namespace '(.*)'")]
    public void ThenTheNodeIsInNamespace(INode classA, bool inNamespace, string sourceVis)
    {
        LogAssert(() => Assert.That(classA.Namespaces(),
            inNamespace ? Has.One.EqualTo(sourceVis) : Has.None.EqualTo(sourceVis)));
    }
}