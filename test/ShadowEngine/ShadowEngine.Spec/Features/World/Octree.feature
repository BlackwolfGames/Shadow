Feature: Octree management

 Scenario: The Octree should be empty upon initialization
  Given a new octree is initialized
  When I query the octree for any voxel
  Then the octree should not contain any voxel

 Scenario: A voxel should be added to the Octree
  Given a new octree is initialized
  When I add a voxel to the octree at coordinates "30, 30, 30"
  Then the octree should contain a voxel at coordinates "30, 30, 30"

 Scenario: A voxel should be removed from the Octree
  Given a new octree is initialized
  And I add a voxel to the octree at coordinates "30, 30, 30"
  When I remove the voxel from the octree at coordinates "30, 30, 30"
  Then the octree should not contain a voxel at coordinates "30, 30, 30"

 Scenario: Exceed maximum depth of Octree
  Given An Octree with maximum depth of 6
  When I attempt to add a voxel at a depth of 7
  Then The voxel addition should fail with an error "Maximum Octree depth exceeded"