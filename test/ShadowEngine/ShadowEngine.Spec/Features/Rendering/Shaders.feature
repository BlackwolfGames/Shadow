Feature: Shaders

    
    Scenario: Load and compile vertex and fragment shaders successfully
        Given I have initialized the OpenGL context
        When I load and compile vertex and fragment shaders from "shader.vert" and "shader.frag"
        Then both shaders should be successfully loaded and compiled

    Scenario: Render a single voxel on the screen
        Given I have initialized the OpenGL context
        And I have created the VAO and VBO
        When I render a voxel at coordinates (0, 0, 0)
        Then a single voxel should be rendered at coordinates (0, 0, 0)

    Scenario: Render multiple voxels without overlap
        Given I have initialized the OpenGL context
        And I have created the VAO and VBO
        When I render voxels at coordinates (0, 0, 0) and (1, 1, 1)
        Then two voxels should be rendered without overlapping

    Scenario: Apply a single uniform color to all rendered voxels
        Given I have initialized the OpenGL context
        And I have rendered multiple voxels
        When I apply a uniform color "Red" to all voxels
        Then all rendered voxels should display in "Red"

    Scenario: Dynamically change the color of a voxel
        Given I have initialized the OpenGL context
        And I have a rendered voxel at coordinates (0, 0, 0) in "Red"
        When I change the color of the voxel at coordinates (0, 0, 0) to "Blue"
        Then the voxel at coordinates (0, 0, 0) should display in "Blue"
