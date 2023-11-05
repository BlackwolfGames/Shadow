Feature: Tree conversion - ConvertTreeToGraph

	Scenario: Single class with no dependencies
		Given we parse 'Graphing/Simple/SingleClass.cs'
		And we convert the project into a graph
		Then the graph contains 1 node
		Then there is a node 'ISingleClass'
		Then the node 'ISingleClass' has 0 edges
		
	Scenario: Two classes with a single dependency
		Given we parse 'Graphing/Simple/DerivedClass.cs'
		And we convert the project into a graph
		Then the graph contains 2 nodes
		Then there is a node 'DerivedClass'
		Then the node 'DerivedClass' has 1 edges
		Then there is a node 'IBase'
		Then the node 'IBase' has 0 edges
		Then the node 'DerivedClass' has an edge to 'IBase'
		Then the edge from node 'DerivedClass' to 'IBase' has 1 dependency of type 'Implementation' 
		
	Scenario: Class with multiple dependencies
		Given we parse 'Graphing/Simple/MultipleDependencies.cs'
		And we convert the project into a graph
		Then the graph contains 3 nodes
		
		And there is a node 'IDependency'
		And the node 'IDependency' has 0 edges
		
		And there is a node 'IDependency'
		And the node 'Dependency' has 1 edge
		And the node 'Dependency' has an edge to 'IDependency'
		And the edge from node 'Dependency' to 'IDependency' has 1 dependency of type 'Implementation' 
		
		And there is a node 'ConsumerClass'
		And the node 'ConsumerClass' has 1 edge
		And the node 'ConsumerClass' has an edge to 'IDependency'
		And the edge from node 'ConsumerClass' to 'IDependency' has 1 dependency of type 'Property' 
		And the edge from node 'ConsumerClass' to 'IDependency' has 1 dependency of type 'ParameterInjection' 