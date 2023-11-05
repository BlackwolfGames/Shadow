Feature: Tree conversion - CycleFinder
	
Scenario: Cyclic dependency detection
	Given we parse 'Graphing/Cycles/SimpleCycle.cs'
	And we convert the project into a graph
	Then the graph contains 1 cycle
	Then the node 'ClassA' is part of a cycle
	Then the node 'ClassB' is part of a cycle
	Then the node 'ClassC' is not part of a cycle