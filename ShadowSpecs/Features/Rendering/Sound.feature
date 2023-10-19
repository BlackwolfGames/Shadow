Feature: Sound

    Scenario: Validate sound effect plays upon specific interaction
        Given the world has a door at coordinates (10, 10, 10)
        And the sound system is initialized
        When the player interacts with the door at coordinates (10, 10, 10)
        Then a "Door_Open" sound effect should be played

    Scenario: Sound effect fails to play due to invalid source file
        Given a world with valid sound sources
        When I attempt to trigger a sound effect using an invalid source file
        Then the sound should not play and I should receive an error message "Invalid sound source file."

    Scenario: Sound effect fails to play due to incompatible frequency settings
        Given a world with an available sound effect "Explosion"
        When I attempt to play the sound effect with an incompatible frequency "44kHz"
        Then the sound effect should fail to play with reason "Incompatible frequency settings"
