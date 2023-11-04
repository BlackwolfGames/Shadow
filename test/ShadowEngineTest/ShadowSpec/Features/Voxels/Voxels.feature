Feature: Voxel Features

    Scenario: A single voxel should be added to the world at specified coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        When I add a voxel at coordinates "50, 50, 50"
        Then the world should contain a voxel at coordinates "50, 50, 50"

    Scenario: A single voxel should be removed from the world at specified coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        And I add a voxel at coordinates "50, 50, 50"
        When I remove the voxel at coordinates "50, 50, 50"
        Then the world should not contain a voxel at coordinates "50, 50, 50"

    Scenario: Multiple voxels should be added to the world
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        When I add "5" voxels at random coordinates
        Then the world should contain "5" voxels
        
    Scenario: A voxel should not be added if its coordinates exceed world boundaries
        Given the world is initialized with dimensions 10, 10, 10
        When I try to add a voxel at coordinates 15, 15, 15 with color "Red"
        Then the voxel should not be added and a "Coordinates exceed world boundaries" message should be shown

    Scenario: A voxel should not be added if the color parameters are invalid
        Given the world is initialized with dimensions 10, 10, 10
        When I try to add a voxel at coordinates 5, 5, 5 with invalid color "#ZZZZZZ"
        Then the voxel should not be added and an "Invalid color parameters" message should be shown

    Scenario: Removing a nonexistent voxel should fail
        Given the world is initialized with dimensions 10, 10, 10
        When I try to remove a voxel at coordinates 5, 5, 5
        Then the voxel removal should fail and a "Voxel does not exist" message should be shown

    Scenario: Voxel addition fails due to invalid physical properties
        Given an empty world
        When I attempt to add a voxel with invalid physical properties like density "-10"
        Then the voxel should not be added with reason "Invalid physical properties"

    Scenario: Voxel fails to be destroyed due to invalid damage parameters
        Given a world with a rock voxel at coordinates "0,0,0"
        When I attempt to destroy the voxel with damage value "-10"
        Then the voxel should remain intact with reason "Invalid damage parameters"
