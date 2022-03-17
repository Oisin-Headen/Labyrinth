﻿using System;
using System.Collections.Generic;

public class Player
{
    public readonly ISet<Character> characters = new HashSet<Character>();
    private readonly Map map;
    private readonly GameController gameController;

    // TODO this should take in the initial characters once there's a character creation menu
    public Player(Map map, GameController gameController)
    {
        this.map = map;
        this.gameController = gameController;

        // TODO creating test character here
        // create the player in the middle
        var coordinates = map.GetSpawnLocation();

        var characterModel = new Character(map.GetSpace(coordinates), map);
        gameController.CreateEntity(coordinates, characterModel);
        map.GetSpace(coordinates).Occupier = characterModel;
        characterModel.MoveToSpace(map.GetSpace(coordinates));
    }

    public void Move()
    {
    }
}
