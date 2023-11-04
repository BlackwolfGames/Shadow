Feature: Physics

    Scenario: Collision should be detected when moving into a voxel's space
        Given the world is initialized with dimensions 100x100x100
        And a voxel is placed at coordinates 20, 20, 20
        When I move to coordinates 20, 20, 20
        Then collision should be detected at coordinates 20, 20, 20

    Scenario: No collision should be detected when moving into empty space
        Given the world is initialized with dimensions 100x100x100
        When I move to coordinates 50, 50, 50
        Then no collision should be detected at coordinates 50, 50, 50

    Scenario: Validate gravity effect on player
        Given the player is at coordinates (5, 10, 5)
        And there is no voxel underneath the player
        When the physics simulation is updated
        Then the player should be at a lower y-coordinate

    Scenario: Collision detection should fail because the voxel at the specified location doesn't exist
        Given I have a voxel world with no voxel at coordinates "5,5,5"
        When I attempt to check for collision at coordinates "5,5,5"
        Then the operation should fail with an error message "No voxel found at specified coordinates"

    Scenario: Gravity effects fail due to invalid physical parameters
        Given a world with gravity enabled
        When I attempt to update gravity with invalid parameters
        Then the update should fail and I should receive an error message "Invalid parameters for gravity."

    Scenario: Collision detection fails due to an invalid collision model
        Given a world with a voxel at coordinates "0,0,0"
        And a dynamic element at coordinates "1,1,1"
        When the dynamic element moves to coordinates "0,0,0"
        Then the collision detection should fail with reason "Invalid collision model"
