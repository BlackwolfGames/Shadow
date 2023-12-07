Feature: Graph layout - Spacing Force

 Scenario: Nodes are spaced apart and pull along edges
  Given we parse 'Layout/Metrics.cs'
  And we convert the project into a graph
  And we create a projection for the graph
  And we start to relax the layout

  When moving the projection of 'Metrics_1' to (5,5)
  And moving the projection of 'Metrics_2' to (-5,5)
  And moving the projection of 'Metrics_3' to (5,-5)
  And moving the projection of 'Metrics_4' to (-5,-5)

  When we enable the 'Spacing' force
  Then printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [5, 5]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-5, 5]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [5, -5]
  Velocity: [0, 0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-5, -5]
  Velocity: [0, 0]
  Edge: Metrics_4
  """

  When the graph is relaxed for 1 step
  Then we're at step 1
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [6.71, 6.71]
  Velocity: [1.71, 1.71]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-6.71, 6.71]
  Velocity: [-1.71, 1.71]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [6.71, -6.71]
  Velocity: [1.71, -1.71]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-6.71, -6.71]
  Velocity: [-1.71, -1.71]
  Edge: Metrics_4
  """

  When moving the projection of 'Metrics_1' to (5,5)
  And moving the projection of 'Metrics_2' to (-5,5)
  And moving the projection of 'Metrics_3' to (5,-5)
  And moving the projection of 'Metrics_4' to (-5,-5)

  When we disable the 'Spacing' force
  When we enable the 'Pulling' force
  Then printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [5, 5]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-5, 5]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [5, -5]
  Velocity: [0, 0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-5, -5]
  Velocity: [0, 0]
  Edge: Metrics_4
  """

  When the graph is relaxed for 1 step
  Then we're at step 2
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [4.9, 5]
  Velocity: [-0.1, 0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-5, 5]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [4.9, -5]
  Velocity: [-0.1, 0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-5, -5]
  Velocity: [0, 0]
  Edge: Metrics_4
  """

  When moving the projection of 'Metrics_1' to (5,5)
  And moving the projection of 'Metrics_2' to (-5,5)
  And moving the projection of 'Metrics_3' to (5,-5)
  And moving the projection of 'Metrics_4' to (-5,-5)

  When we enable the 'Spacing' force
  When we enable the 'Pulling' force
  When we enable the 'Horizontal' force
  When we enable the 'Centering' force
  When we enable the 'Damping' force

  When the graph is relaxed until still
  Then we're at step 625
  Then the average velocity is equal to '0.04'
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [-40.64, 179.6]
  Velocity: [0, -0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-122.04, 179.62]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [-40.64, -179.6]
  Velocity: [0, 0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-122.04, -179.62]
  Velocity: [0, -0]
  Edge: Metrics_4
  """