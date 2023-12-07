Feature: Graph layout - Clustering

 Scenario: Clustering metrics

  Given we parse 'Layout/Clustering.cs'
  And we convert the project into a graph
  And we create a projection for the graph
  And we start to relax the layout
  When we generate a louvain clustering report
  And we generate a spatial clustering report