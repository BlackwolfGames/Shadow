Feature: Graph layout - node controls

Scenario: Graph initially has all nodes at the origin
	Given we parse 'Parsing/Calls/TestCallConsole.cs'
	And we convert the project into a graph
	And we create a projection for the graph
	
	Then the projection of 'TestCallConsole' is not at (0,0)
	Then the projection of 'TestCallConsole' is 10 away from (0,0)
	When moving the projection of 'TestCallConsole' to (3,5)
	Then the projection of 'TestCallConsole' is at (3,5)