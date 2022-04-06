using System;
using System.Collections.Generic;
using static Utilities;

public class Player
{
    public readonly ISet<Character> characters = new HashSet<Character>();
    private Character selectedCharacter;

    private readonly Map map;
    private readonly GameController gameController;


    // TODO this should take in the initial characters once there's a character creation menu
    public Player(Map map, GameController gameController)
    {
        this.map = map;
        this.gameController = gameController;

        // TODO creating test character here
        // create the player in the middle
        var startSpace = map.GetSpawnLocation();

        var characterModel = new Character(startSpace, map, CharacterLook.Warrior);
        gameController.CreateEntity(startSpace, characterModel);
        startSpace.Occupier = characterModel;
        characterModel.MoveToSpace(startSpace);

        characters.Add(characterModel);
        selectedCharacter = characterModel;
    }

    public void Move(CardinalDirection direction)
    {
        if (selectedCharacter == null)
            return;

        selectedCharacter.QueueMove(direction);
    }



}
