Feature: Objects

    Scenario: A dynamic element like a door should be added at the specified location
        Given an empty world
        When I add a dynamic element of type "Door" at coordinates 5x5x5
        Then a "Door" should exist at coordinates 5x5x5

    Scenario: Specified dynamic element should be removed from the world
        Given a world with a "Door" at coordinates 5x5x5
        When I remove the dynamic element at coordinates 5x5x5
        Then the world should have no dynamic element at coordinates 5x5x5

    Scenario: Door should open when interacted with
        Given a world with a "Door" at coordinates 5x5x5
        When I interact with the "Door" at coordinates 5x5x5
        Then the "Door" at coordinates 5x5x5 should be in the "Open" state

    Scenario: Dynamic element addition should fail because the specified location is already occupied by a voxel
        Given I have a voxel world with a voxel at coordinates "5,5,5"
        When I attempt to add a dynamic element at coordinates "5,5,5"
        Then the operation should fail with an error message "Location already occupied by voxel"

    Scenario: Dynamic element fails to interact due to invalid parameters
        Given a world with a door at coordinates "0,0,0"
        When I attempt to open the door without a key
        Then the door should remain closed with reason "Invalid interaction parameters"
