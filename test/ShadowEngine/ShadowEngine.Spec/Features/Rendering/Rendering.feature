Feature: Rendering

 Scenario: Only chunks within the camera frustum should be rendered
  Given a world with 10 chunks
  And a camera frustum that can fit 4 chunks
  When I render the scene
  Then only 4 chunks should be visible

 Scenario: Perform frustum culling with invalid camera parameters
  Given A camera with invalid parameters FOV: 190, AspectRatio: 1.5, NearClip: 0.1, FarClip: -10
  When I try to perform frustum culling
  Then The culling should fail with an error "Invalid camera parameters"

 Scenario: Lower resolution color data should be used for chunks farther from the camera
  Given a world with multiple chunks at varying distances from the camera
  When I render the scene
  Then chunks farther than a distance of 100 units should use lower-resolution color data

 Scenario: Exceed level of detail thresholds
  Given A rendering engine with level of detail thresholds set to Min: 0.1, Max: 1
  When I try to set the level of detail to 1.5
  Then The operation should fail with an error "Level of detail out of range"

 Scenario: Frustum culling fails due to incorrectly defined frustum
  Given a world with a set of visible voxels
  When the renderer attempts frustum culling with invalid frustum parameters
  Then the frustum culling should fail with reason "Invalid frustum definition"

 Scenario: Reflections are incorrectly rendered due to computational errors
  Given a world with a water voxel at coordinates "0,0,0"
  And a light source at coordinates "10,10,10"
  When the renderer attempts to render reflections
  Then the reflections should be incorrectly rendered with reason "Computational errors"