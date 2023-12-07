Feature: Querying

 Scenario: Query the dimensions of a specific chunk
  Given a world is initialized with dimensions width of "100", height of "100" and depth of "100"
  And I add a chunk at coordinates "20, 20, 20" with dimensions width of "10", height of "10" and depth of "10"
  When I query the dimensions of the chunk at coordinates "20, 20, 20"
  Then the dimensions should match width of "10", height of "10" and depth of "10"

 Scenario: Test if voxels only exist within the chunk boundaries
  Given a world is initialized with dimensions width of "100", height of "100" and depth of "100"
  And I add a chunk at coordinates "20, 20, 20" with dimensions width of "10", height of "10" and depth of "10"
  And I add a voxel at coordinates "25, 25, 25" in the chunk at "20, 20, 20"
  When I query for a voxel at coordinates "15, 15, 15"
  Then the world should not contain a voxel at coordinates "15, 15, 15"

 Scenario: Returned voxels should be within the query region inside a chunk
  Given a chunk with 10 voxels at coordinates ranging from 1x1x1 to 10x10x10
  When I perform a spatial query for the region 2x2x2 to 5x5x5
  Then the returned voxels should only be within the coordinates 2x2x2 to 5x5x5

 Scenario: Returned voxels should be within the query region spanning multiple chunks
  Given a world with two adjacent chunks filled with voxels
  When I perform a spatial query spanning both chunks
  Then the returned voxels should only be within the queried region spanning both chunks

 Scenario: Query for voxels within a specified region inside a chunk
  Given I have a chunk filled with "N" voxels
  When I perform a spatial query within the chunk from "StartX, StartY, StartZ" to "EndX, EndY, EndZ"
  Then the query should return "ExpectedCount" voxels

 Scenario: Query for voxels within a specified region spanning multiple chunks
  Given I have multiple chunks filled with "N" voxels each
  When I perform a spatial query across chunks from "StartX, StartY, StartZ" to "EndX, EndY, EndZ"
  Then the query should return "ExpectedCount" voxels

 Scenario: Querying the color of a nonexistent voxel should fail
  Given the world is initialized with dimensions 10, 10, 10
  When I try to query the color of a voxel at coordinates 5, 5, 5
  Then the color query should fail and a "Voxel does not exist" message should be shown

 Scenario: Spatial query should fail because the query region is invalid
  Given I have an existing voxel world with boundaries "0,0,0" and "10,10,10"
  When I attempt to perform a spatial query with region "11,11,11" and "12,12,12"
  Then the query should fail with an error message "Query region is outside world boundaries"