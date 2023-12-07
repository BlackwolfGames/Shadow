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

  When moving the projection of 'TestCallConsole' to (9.9,0)
  When moving the projection of '.Console' to (0,9)
  Then 'pos1' has moved left by 0.1
  And 'pos2' has moved down by 1
  When moving the projection of 'TestCallConsole' to (10.4,-1)
  When moving the projection of '.Console' to (-2,12)
  Then 'pos1' has moved downright by 1.08
  And 'pos2' has moved upleft by 2.83
  When moving the projection of 'TestCallConsole' to (10,0)
  When moving the projection of '.Console' to (0,10)
  Then 'pos1' has moved nowhere
  And 'pos2' has moved nowhere