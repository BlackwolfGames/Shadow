Feature: Graph layout - CenteringForce

 Scenario: Centering force pulls all nodes to the center
  Given we parse 'Layout/Metrics.cs'
  And we convert the project into a graph
  And we create a projection for the graph
  And we start to relax the layout

  When moving the projection of 'Metrics_1' to (5,5)
  And moving the projection of 'Metrics_2' to (-5,5)
  And moving the projection of 'Metrics_3' to (5,-5)
  And moving the projection of 'Metrics_4' to (-5,-5)

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

  When we enable the 'Centering' force
  And the graph is relaxed for 1 step
  Then we're at step 1
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [4.95, 4.95]
  Velocity: [-0.05, -0.05]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-4.95, 4.95]
  Velocity: [0.05, -0.05]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [4.95, -4.95]
  Velocity: [-0.05, 0.05]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-4.95, -4.95]
  Velocity: [0.05, 0.05]
  Edge: Metrics_4
  """

  When the graph is relaxed for 1 step
  Then we're at step 2
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [4.85, 4.85]
  Velocity: [-0.1, -0.1]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-4.85, 4.85]
  Velocity: [0.1, -0.1]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [4.85, -4.85]
  Velocity: [-0.1, 0.1]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-4.85, -4.85]
  Velocity: [0.1, 0.1]
  Edge: Metrics_4
  """

  When the graph is relaxed for 10 steps
  Then we're at step 12
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [1.58, 1.58]
  Velocity: [-0.47, -0.47]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [-1.58, 1.58]
  Velocity: [0.47, -0.47]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [1.58, -1.58]
  Velocity: [-0.47, 0.47]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [-1.58, -1.58]
  Velocity: [0.47, 0.47]
  Edge: Metrics_4
  """

  When the graph is relaxed until still
  Then we're at step 10012
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [-4.39, -4.39]
  Velocity: [-0.26, -0.26]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [4.39, -4.39]
  Velocity: [0.26, -0.26]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [-4.39, 4.39]
  Velocity: [-0.26, 0.26]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [4.39, 4.39]
  Velocity: [0.26, 0.26]
  Edge: Metrics_4
  """

  When we enable the 'Damping' force
  And the graph is relaxed until still
  Then we're at step 10303
  And printing the graph yields the following:
  """
  Name: Metrics_1
  Position: [-0.05, -0.05]
  Velocity: [0, 0]
  Edge: Metrics_2

  Name: Metrics_2
  Position: [0.05, -0.05]
  Velocity: [-0, 0]
  Edge: Metrics_2

  Name: Metrics_3
  Position: [-0.05, 0.05]
  Velocity: [0, -0]
  Edge: Metrics_4

  Name: Metrics_4
  Position: [0.05, 0.05]
  Velocity: [-0, -0]
  Edge: Metrics_4
  """