Feature: OpenGL

 Scenario: Initialize OpenGL context successfully
  Given I have a console application with OpenTK
  When I initialize the OpenGL context
  Then the OpenGL context should be successfully initialized

 Scenario: Create Vertex Array Object and Vertex Buffer Object successfully
  Given I have initialized the OpenGL context
  And I have loaded and compiled the shaders
  When I create a Vertex Array Object and a Vertex Buffer Object
  Then both the VAO and VBO should be successfully created

 Scenario: Enable and validate depth testing
  Given I have initialized the OpenGL context
  And I have rendered multiple overlapping voxels
  When I enable depth testing
  Then the rendered voxels should respect depth and not overlap incorrectly

 Scenario: Implement and validate frustum culling
  Given I have initialized the OpenGL context
  And I have rendered multiple voxels in a scene
  When I implement frustum culling
  Then only voxels within the camera's view frustum should be rendered

 Scenario: Implement and validate backface culling
  Given I have initialized the OpenGL context
  And I have rendered multiple voxels in a scene
  When I implement backface culling
  Then only the front faces of the voxels should be rendered