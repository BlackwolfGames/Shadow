Feature: Texture Features

 Scenario: 3D texture should be initialized with specified dimensions
  Given a renderer with 3D texture support enabled
  When I initialize a 3D texture with dimensions 256x256x256
  Then the 3D texture should have dimensions 256x256x256

 Scenario: Map a voxel's color to a corresponding texel in the 3D texture
  Given a 3D texture with dimensions 256x256x256
  And a voxel with color "Red" at coordinates 4x4x4
  When I map the voxel to the 3D texture
  Then the texel at 3D texture coordinates 4x4x4 should be "Red"

 Scenario: Validate that voxel colors match their mapped textures
  Given the world has a voxel at coordinates (0, 0, 0) with color "Red"
  And a texture map exists that maps "Red" to texture "Brick"
  When the renderer draws the voxel at coordinates (0, 0, 0)
  Then the voxel should be textured as "Brick"

 Scenario: Add an invalid 3D texture
  Given An empty 3D texture storage
  When I try to add a 3D texture with dimensions Width: 10, Height: 10, Depth: -1
  Then The 3D texture should not be added and an error "Invalid 3D texture dimensions" should be returned

 Scenario: Attempt to map a voxel to a nonexistent 3D texture
  Given A voxel at coordinates X: 0, Y: 0, Z: 0 and an empty 3D texture storage
  When I try to map the voxel to a 3D texture with ID: "Texture_1"
  Then The mapping should fail with an error "3D texture does not exist"

 Scenario: Texture mapping fails due to an invalid texture source
  Given a world with valid texture sources
  When I attempt to map a voxel to an invalid texture source
  Then the mapping should fail and I should receive an error message "Invalid texture source."

 Scenario: Texture mapping fails due to incorrect UV coordinates
  Given a world with a voxel at coordinates "0,0,0"
  When I attempt to map the voxel with UV coordinates "3,3"
  Then the texture mapping should fail with reason "Invalid UV coordinates"