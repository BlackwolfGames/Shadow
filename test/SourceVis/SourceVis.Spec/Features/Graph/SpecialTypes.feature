Feature: Tree conversion - SpecialTypes
	
Scenario: Dependencies on undefined types (like generics)
	Given we parse 'Graphing/SpecialTypes/Generics.cs'
	And we convert the project into a graph
	
	Then there is a node 'GenericHolder<T>'
	Then there is a node 'T'
	Then there is a node 'DateTime'
	
	And node 'GenericHolder<T>' has 2 edge
	And node 'GenericHolder<T>' has an edge to node 'T'
	And node 'GenericHolder<T>' has an edge to node 'DateTime'
	And the edge from node 'GenericHolder<T>' to node 'T' has 1 dependency of type 'VariableDeclaration' 
	And node 'T' has 0 edges
	
	And node 'GenericHolder<T>' is of 'normal' type
	And node 'T' is of 'generic' type
	And node 'DateTime' is of 'builtin' type