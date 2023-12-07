Feature: Graph layout - Metrics

 Scenario: Graph metrics
  Given we parse 'Parsing/Calls/TestCallConsole.cs'
  And we convert the project into a graph
  And we create a projection for the graph
  And we start to relax the layout

  When moving the projection of 'TestCallConsole' to (10,0)
  And moving the projection of '.Console' to (0,10)
  Then the total distance from center is equal to '20'
  And the average distance between nodes is equal to '14.14'
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

  Then the total distance from center is equal to '10+10+Sqrt(100+100)'
  Then the mass distance from center is equal to 'Sqrt(25+25)'
  And the average distance between nodes is equal to '((10*4)+(Sqrt(Pow(10, 2)*2)*2))/6'
  And the number of intersections is equal to '1'
  When moving the projection of 'Metrics_1' to (0,0)
  And moving the projection of 'Metrics_4' to (10,0)

  Then the total distance from center is equal to '10+10+Sqrt(100+100)'
  And the mass distance from center is equal to 'Sqrt(25+25)'
  And the average distance between nodes is equal to '((10*4)+(Sqrt(Pow(10, 2)*2)*2))/6'
  And the number of intersections is equal to '0'
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [0, 0]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [0, 10]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [10, 10]
  Velocity: [0, 0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [10, 0]
  Velocity: [0, 0]
  Edge: Metrics_4
  """

  Then the average distance between nodes is equal to '((10*4)+(Sqrt(Pow(10, 2)*2)*2))/6'
  And the minimum distance between nodes is equal to '10'
  And the maximum distance between nodes is equal to 'Sqrt(Pow(10, 2)*2)'
  And the standard deviation of distances between nodes is equal to '2'

  And the average edge length is equal to '(10+10+0+0)/4'
  And the minimum edge length is equal to '0'
  And the maximum edge length is equal to '10'
  And the standard deviation of edge length is equal to '5'

  And the bounding box is equal to '10' by '10'
  And the contained node density is equal to '100/4'
  And the contained edge density is equal to '100/4'