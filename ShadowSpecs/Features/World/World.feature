Feature: World Features

    Scenario: The world should be empty upon initialization
        Given a world is initialized
        Then the world should contain "0" voxels

    Scenario: The world should have specified dimensions upon initialization
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        Then the world dimensions should be width "100", height "100", and depth "100"
        
    Scenario: The world should fail to initialize with negative dimensions
        Given the world dimensions are set to -10, -10, -10
        When I try to initialize the world
        Then the world initialization should fail due to invalid negative dimensions

        
    Scenario: A single chunk should be added to the world at specified coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        When I add a chunk at coordinates "10, 10, 10" with dimensions "10, 10, 10"
        Then the world should contain a chunk at coordinates "10, 10, 10"

    Scenario: A single chunk should be removed from the world at specified coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        And I add a chunk at coordinates "10, 10, 10" with dimensions "10, 10, 10"
        When I remove the chunk at coordinates "10, 10, 10"
        Then the world should not contain a chunk at coordinates "10, 10, 10"
        
    Scenario: Adding a chunk at existing position should fail
        Given a chunk already exists at coordinates 2, 2, 2
        When I try to add another chunk at coordinates 2, 2, 2
        Then the chunk addition should fail and a "Chunk already exists" message should be shown

    Scenario: Removing a nonexistent chunk should fail
        Given the world is initialized with dimensions 10, 10, 10
        When I try to remove a chunk at coordinates 5, 5, 5
        Then the chunk removal should fail and a "Chunk does not exist" message should be shown