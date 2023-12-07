Feature: Persistence

 Scenario: World state should be successfully saved to disk
  Given the world is initialized with dimensions 100x100x100
  And a voxel is placed at coordinates 20, 20, 20
  When I save the world state to "savefile1"
  Then "savefile1" should contain a voxel at coordinates 20, 20, 20

 Scenario: World state should be successfully loaded from disk
  Given the world is initialized with dimensions 100x100x100
  And "savefile1" contains a voxel at coordinates 20, 20, 20
  When I load the world state from "savefile1"
  Then the world should contain a voxel at coordinates 20, 20, 20

 Scenario: World save operation should fail due to insufficient disk space
  Given I have a voxel world and my disk space is "10 MB"
  When I attempt to save the voxel world requiring "50 MB"
  Then the operation should fail with an error message "Insufficient disk space"

 Scenario: World load operation should fail due to corrupted save file
  Given I have a corrupted voxel world save file
  When I attempt to load this voxel world
  Then the operation should fail with an error message "Corrupted save file"