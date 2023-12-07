Feature: Lighting

 Scenario: Light source should illuminate voxels within its range
  Given a light source is placed at coordinates 5, 5, 5 with a range of 10 units
  When the scene is rendered
  Then voxels within 10 units of coordinates 5, 5, 5 should be illuminated

 Scenario: Lighting should fail due to invalid light source parameters
  Given I have a voxel world with existing light sources
  When I attempt to add a light source with an invalid intensity of "-1"
  Then the operation should fail with an error message "Invalid light source intensity"

 Scenario: Light source addition should fail due to invalid parameters
  Given I have a voxel world with existing light sources
  When I attempt to add a light source at coordinates "5,5,5" with an invalid color "XYZ"
  Then the operation should fail with an error message "Invalid light source color"

 Scenario: Light source removal should fail because it doesn't exist at the specified location
  Given I have a voxel world with no light source at coordinates "5,5,5"
  When I attempt to remove the light source at coordinates "5,5,5"
  Then the operation should fail with an error message "No light source found at specified coordinates"

 Scenario: Shadows should be incorrectly rendered due to computational errors
  Given I have a voxel world with existing light sources
  When I attempt to render shadows with an incorrect algorithm
  Then the operation should fail with an error message "Incorrect shadow calculation"