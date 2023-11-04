Feature: Performance

    Scenario: Validate that renderer maintains at least specified FPS during gameplay
        Given the world is initialized with 100 chunks
        And the renderer is initialized
        When the game is played for 10 seconds
        Then the average FPS should be greater than or equal to 60

    Scenario: Renderer fails to run due to insufficient GPU memory
        Given a system with 1GB GPU memory
        When the renderer attempts to start
        Then the renderer should fail to start with error "Insufficient GPU memory"
