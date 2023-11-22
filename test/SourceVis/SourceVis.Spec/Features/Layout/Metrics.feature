Feature: Graph layout - Metrics
	
Scenario: Graph metrics
	Given we parse 'Parsing/Calls/TestCallConsole.cs'
	And we convert the project into a graph
	And we create a projection for the graph
	And we start to relax the layout
	 
	When moving the projection of 'TestCallConsole' to (10,0)
	And moving the projection of '.Console' to (0,10)
	Then the total distance from center is 20
	And the average distance between nodes is 14.14
	When moving the projection of 'TestCallConsole' to (1000,0)
	And moving the projection of '.Console' to (0,1000)
	
	Given we parse 'Layout/Metrics.cs'
	And we convert the project into a graph
	And we create a projection for the graph
	And we start to relax the layout
	 
	When moving the projection of 'Metrics_1' to (10,0)
	And moving the projection of 'Metrics_2' to (0,10)
	And moving the projection of 'Metrics_3' to (10,10)
	And moving the projection of 'Metrics_4' to (0,0)
	
	Then the total distance from center is 34.14
	And the average distance between nodes is 11.38
	And there is 1 intersection
	When moving the projection of 'Metrics_1' to (0,10)
	And moving the projection of 'Metrics_2' to (10,0)
	
	Then the total distance from center is 34.14
	And the average distance between nodes is 11.38
	And there are 0 intersections