Feature: Elemental
    
    Scenario: Validate that voxel behaves as a slippery surface when it's made of ice
        Given the world has an ice voxel at coordinates (2, 2, 2)
        When the player moves onto the voxel at coordinates (2, 2, 2)
        Then the player's movement speed should be increased

    Scenario: Validate that voxel behaves as a damaging liquid when it's made of lava
        Given the world has a lava voxel at coordinates (3, 3, 3)
        When the player moves onto the voxel at coordinates (3, 3, 3)
        Then the player should receive damage

    Scenario: Validate that voxel behaves as a liquid with appropriate physics when it's made of water
        Given the world has a water voxel at coordinates (4, 4, 4)
        When the player moves onto the voxel at coordinates (4, 4, 4)
        Then the player's movement speed should be decreased

    Scenario: Liquid voxel addition fails due to invalid properties
        Given a world with a water chunk at coordinates X: 0, Y: 0, Z: 0
        When I attempt to add a liquid voxel with invalid properties at coordinates X: 1, Y: 1, Z: 1
        Then the liquid voxel should not be added and I should receive an error message "Invalid properties for liquid voxel."

    Scenario: Surface voxel addition fails due to invalid properties
        Given a world with an ice chunk at coordinates X: 10, Y: 10, Z: 10
        When I attempt to add a surface voxel with invalid properties at coordinates X: 11, Y: 11, Z: 11
        Then the surface voxel should not be added and I should receive an error message "Invalid properties for surface voxel."
