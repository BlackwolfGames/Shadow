Feature: Graph layout - node controls

Scenario: Graph initially has all nodes spread randomly 10 units away from origin, and nodes can be moved
	Given we parse 'Parsing/Calls/TestCallConsole.cs'
	And we convert the project into a graph
	And we create a projection for the graph
	
	Then the projection of 'TestCallConsole' is not at (0,0)
	And the projection of 'TestCallConsole' is 10 away from (0,0)
	And the projection of 'System.Console' is not at (0,0)
	And the projection of '.Console' is 10 away from (0,0)
	
	When moving the projection of 'TestCallConsole' to (3,5)
	Then the projection of 'TestCallConsole' is at (3,5)
	
Scenario: Graph layout engine basic behavior
	Given we parse 'Parsing/Calls/TestCallConsole.cs'
	And we convert the project into a graph
	And we create a projection for the graph
	And we start to relax the layout
	And we want to memorize positions
	 
	When moving the projection of 'TestCallConsole' to (10,0)
	When moving the projection of '.Console' to (0,10)
	When we remember the projection of 'TestCallConsole' as 'pos1'  
	When we remember the projection of '.Console' as 'pos2'  
	Then we're at step 0
	And 'pos1' has moved nowhere
	And 'pos2' has moved nowhere
	When the graph is relaxed for 1 step
	Then we're at step 1
	When the graph is relaxed for 1 step
	Then we're at step 2
	And 'pos1' has moved nowhere
	And 'pos2' has moved nowhere
	
	When we enable the 'Centering' force
	Then there is 1 active force
	When the graph is relaxed for 1 step

	Then 'pos1' has moved right by 0
	And 'pos2' has moved down by 0