Feature: Tree conversion - ConvertTreeToGraph

	Scenario: Single class with no dependencies
		Given we parse 'Graphing/Simple/SingleClass.cs'
		And we convert the project into a graph
		Then the graph contains 1 node
		Then there is a node 'ISingleClass'
		Then node 'ISingleClass' has 0 edges
		
	Scenario: Two classes with a single dependency
		Given we parse 'Graphing/Simple/DerivedClass.cs'
		And we convert the project into a graph
		Then the graph contains 2 nodes
		Then there is a node 'DerivedClass'
		Then node 'DerivedClass' has 1 edges
		Then there is a node 'IBase'
		Then node 'IBase' has 0 edges
		Then node 'DerivedClass' has an edge to node 'IBase'
		Then the edge from node 'DerivedClass' to node 'IBase' has 1 dependency of type 'Implementation' 
		
	Scenario: Class with multiple dependencies
		Given we parse 'Graphing/Simple/MultipleDependencies.cs'
		And we convert the project into a graph
		Then the graph contains 3 nodes
		
		And there is a node 'IDependency'
		And node 'IDependency' has 0 edges
		
		And there is a node '.Dependency'
		And node '.Dependency' has 1 edge
		And node '.Dependency' has an edge to node 'IDependency'
		And the edge from node '.Dependency' to node 'IDependency' has 1 dependency of type 'Implementation' 
		
		And there is a node 'ConsumerClass'
		And node 'ConsumerClass' has 1 edge
		And node 'ConsumerClass' has an edge to node 'IDependency'
		And the edge from node 'ConsumerClass' to node 'IDependency' has 1 dependency of type 'Property' 
		And the edge from node 'ConsumerClass' to node 'IDependency' has 1 dependency of type 'ParameterInjection' 