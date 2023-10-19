Feature: Camera

    Scenario: Validate camera zoom to specified level
        Given the camera is initialized at zoom level 1.0
        When the player triggers a zoom to level 1.5
        Then the camera zoom level should be 1.5

    Scenario: Validate camera rotation to specified angle
        Given the camera is initialized at rotation angle 0 degrees
        When the player rotates the camera to 90 degrees
        Then the camera should be at rotation angle 90 degrees

    Scenario: Camera zoom fails because the specified level is out of valid range
        Given a camera with a current zoom level of 1.0
        When I attempt to set the zoom level to 10.0
        Then the zoom should not change and I should receive an error message "Zoom level is out of valid range."

    Scenario: Camera rotation fails because the specified angle is invalid
        Given a camera with a current rotation angle of 0 degrees
        When I attempt to set the rotation angle to 400 degrees
        Then the rotation should not change and I should receive an error message "Invalid rotation angle."
