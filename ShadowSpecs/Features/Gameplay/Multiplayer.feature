Feature: Multiplayer

    Scenario: Validate that all players see the same world state in a multiplayer session
        Given the world has two players Player1 and Player2
        And Player1 adds a voxel at coordinates (5, 5, 5)
        When Player2 queries the voxel at coordinates (5, 5, 5)
        Then Player2 should see a voxel at coordinates (5, 5, 5)

    Scenario: Multiplayer sync fails due to inconsistent world states among players
        Given two players are in a multiplayer game session
        And player1 has world state "A"
        And player2 has world state "B"
        When both players attempt to synchronize their worlds
        Then the synchronization should fail with reason "Inconsistent world states"
