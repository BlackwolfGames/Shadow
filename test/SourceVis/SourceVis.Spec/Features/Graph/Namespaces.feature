Feature: Tree conversion - Namespaces

 Scenario: Handling Different Namespaces
  Given we parse 'Graphing/Namespaces/NamespaceTest.cs'
  And we convert the project into a graph

  Then node 'IClassA' is not in the same namespace as node 'IClassB'
  Then node 'IClassA' is not in the same namespace as node 'IClassC'
  Then node 'IClassB' is in the same namespace as node 'IClassC'

  Then node 'IClassA' is in namespace 'SourceVis'
  Then node 'IClassA' is in namespace 'Spec'
  Then node 'IClassA' is in namespace 'TestFiles'
  Then node 'IClassA' is in namespace 'Graphing'
  Then node 'IClassA' is in namespace 'Namespaces'
  Then node 'IClassA' is in namespace 'NamespaceOne'
  Then node 'IClassA' is not in namespace 'IClassA'

  Then node 'IClassB' is in namespace 'SourceVis'
  Then node 'IClassB' is in namespace 'Spec'
  Then node 'IClassB' is in namespace 'TestFiles'
  Then node 'IClassB' is in namespace 'Graphing'
  Then node 'IClassB' is in namespace 'Namespaces'
  Then node 'IClassB' is in namespace 'NamespaceTwo'
  Then node 'IClassB' is not in namespace 'NamespaceOne'
  Then node 'IClassB' is not in namespace 'IClassB'
  Then node 'IClassB' is not in namespace 'IClassC'

 Scenario: Edges show namespace changes
  Given we parse 'Graphing/Namespaces/NamespaceCrossingTest.cs'
  And we convert the project into a graph

  Then there are 4 classes
  Then the edge from 'Processor' to 'SharedService' enters 1 namespace
  Then the edge from 'Processor' to 'SharedService' leaves 1 namespace
  Then the edge from 'Processor' to 'SharedService' crosses 2 namespaces
  Then the edge from 'Processor' to 'SharedService' shares 2 namespaces

  Then the edge from '.Orchestrator' to 'Processor' enters 1 namespace
  Then the edge from '.Orchestrator' to 'Processor' leaves 0 namespace
  Then the edge from '.Orchestrator' to 'Processor' crosses 1 namespace
  Then the edge from '.Orchestrator' to 'Processor' shares 2 namespace

  Then the edge from '.Orchestrator' to 'SharedService' enters 1 namespace
  Then the edge from '.Orchestrator' to 'SharedService' leaves 0 namespace
  Then the edge from '.Orchestrator' to 'SharedService' crosses 1 namespace
  Then the edge from '.Orchestrator' to 'SharedService' shares 2 namespace

  Then the edge from '.Orchestrator' to 'ExternalOrchestrator' enters 1 namespace
  Then the edge from '.Orchestrator' to 'ExternalOrchestrator' leaves 2 namespace
  Then the edge from '.Orchestrator' to 'ExternalOrchestrator' crosses 3 namespace
  Then the edge from '.Orchestrator' to 'ExternalOrchestrator' shares 0 namespace

  Then the edge from 'ExternalOrchestrator' to 'SharedService' enters 3 namespace
  Then the edge from 'ExternalOrchestrator' to 'SharedService' leaves 1 namespace
  Then the edge from 'ExternalOrchestrator' to 'SharedService' crosses 4 namespace
  Then the edge from 'ExternalOrchestrator' to 'SharedService' shares 0 namespace

  Then the edge from 'SharedService' to 'ExternalOrchestrator' enters 1 namespace
  Then the edge from 'SharedService' to 'ExternalOrchestrator' leaves 3 namespace
  Then the edge from 'SharedService' to 'ExternalOrchestrator' crosses 4 namespace
  Then the edge from 'SharedService' to 'ExternalOrchestrator' shares 0 namespace

  Then the edge from 'SharedService' to '.Orchestrator' enters 0 namespace
  Then the edge from 'SharedService' to '.Orchestrator' leaves 1 namespace
  Then the edge from 'SharedService' to '.Orchestrator' crosses 1 namespace
  Then the edge from 'SharedService' to '.Orchestrator' shares 2 namespace