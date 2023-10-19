Feature: Chunk Features

    Scenario: A voxel should be added to a specific chunk
        Given a world is initialized with dimensions width of "100", height of "100" and depth of "100"
        And I add a chunk at coordinates "20, 20, 20"
        When I add a voxel at coordinates "25, 25, 25" in the chunk at "20, 20, 20"
        Then the chunk at coordinates "20, 20, 20" should contain a voxel at "25, 25, 25"


        Scenario: Adding a voxel to a nonexistent chunk should fail
            Given the world is initialized with dimensions 10, 10, 10
            When I try to add a voxel with color "Red" to a chunk at coordinates 5, 5, 5
            Then the voxel should not be added and a "Target chunk does not exist" message should be shown

    Scenario: Querying the dimensions of a nonexistent chunk should fail
        Given the world is initialized with dimensions 10, 10, 10
        When I try to query the dimensions of a chunk at coordinates 5, 5, 5
        Then the dimension query should fail and a "Chunk does not exist" message should be shown

    Scenario: Add voxel outside of chunk boundaries
        Given A chunk at coordinates X: 0, Y: 0, Z: 0 with dimensions Width: 16, Height: 16, Depth: 16
        When I try to add a voxel at coordinates X: 20, Y: 20, Z: 20 within the chunk
        Then The voxel should not be added and an error "Outside chunk boundaries" should be returned

    Scenario: Add a voxel to a full chunk
        Given A chunk at coordinates X: 0, Y: 0, Z: 0 that is already at its maximum capacity of 4096 voxels
        When I try to add another voxel to the chunk
        Then The voxel should not be added and an error "Chunk at maximum capacity" should be returned
