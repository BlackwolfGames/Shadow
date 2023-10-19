Feature: Color Features

    Scenario: Query the color of a voxel at specific coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        And I add a voxel with color "Red" at coordinates "50, 50, 50"
        When I query the color of the voxel at coordinates "50, 50, 50"
        Then the returned color should be "Red"

    Scenario: Update the color of a voxel at specific coordinates
        Given a world is initialized with dimensions width of "100", height of "100", and depth of "100"
        And I add a voxel with color "Red" at coordinates "50, 50, 50"
        When I update the color of the voxel at coordinates "50, 50, 50" to "Blue"
        Then the color of the voxel at coordinates "50, 50, 50" should be "Blue"
        
    Scenario: Voxel color should approximate the original color within a specified tolerance
        Given a voxel with original color "FF5733"
        When I apply color quantization with a tolerance of 10%
        Then the voxel's color should be within 10% of "FF5733"

    Scenario: Chunk color should be the average of all voxel colors within the chunk
        Given a chunk with 10 voxels of varying colors
        When I apply color chunking to the chunk
        Then the chunk color should be the average of all voxel colors within the chunk

    Scenario: Updating the color of a nonexistent voxel should fail
        Given the world is initialized with dimensions 10, 10, 10
        When I try to update the color of a voxel at coordinates 5, 5, 5 to "Green"
        Then the color update should fail and a "Voxel does not exist" message should be shown

    Scenario: Attempt to add a duplicate color to the color palette
        Given A color palette that already contains the color RGB(255, 0, 0)
        When I try to add the color RGB(255, 0, 0) to the color palette
        Then The addition should fail with an error "Duplicate color entry"

    Scenario: Exceed maximum capacity of the color palette
        Given A color palette that is already at its maximum capacity of 256 unique colors
        When I try to add another unique color to the palette
        Then The color should not be added and an error "Palette at maximum capacity" should be returned

    Scenario: Apply color quantization with out-of-bounds tolerance
        Given A color palette with existing colors
        When I try to apply color quantization with a tolerance value of 100
        Then The operation should fail with an error "Tolerance exceeds permissible limits"
