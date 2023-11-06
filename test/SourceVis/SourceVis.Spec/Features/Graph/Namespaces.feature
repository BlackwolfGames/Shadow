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