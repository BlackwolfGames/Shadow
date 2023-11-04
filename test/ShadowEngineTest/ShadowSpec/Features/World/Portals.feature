Feature: Portals

    Scenario: A portal should be placed at the specified coordinates
        Given the world is initialized with dimensions 100x100x100
        When I place a portal at coordinates 30, 30, 30
        Then the world should contain a portal at coordinates 30, 30, 30
        
    Scenario: Moving through a portal should change the player's location to the corresponding exit point
        Given a portal is placed at coordinates 30, 30, 30 with an exit at 40, 40, 40
        When I move through the portal at coordinates 30, 30, 30
        Then my location should be 40, 40, 40

        
    Scenario: Portal placement should fail because the specified location is invalid
        Given a world with no portals
        When I attempt to place a portal at an invalid location with coordinates X: 500, Y: 500, Z: 500
        Then the portal should not be placed and I should receive an error message "Invalid location for portal placement."

    Scenario: Portal transition fails because the exit point does not exist    
        Given a world with a portal that has a nonexistent exit point
        When I attempt to transition through the portal
        Then the transition should fail and I should receive an error message "Portal exit point does not exist."

    Scenario: Portal transition fails due to invalid destination properties
        Given a world with a portal at coordinates "0,0,0"
        And the portal leads to an invalid location "underwater"
        When a player attempts to go through the portal
        Then the portal transition should fail with reason "Invalid destination properties"
