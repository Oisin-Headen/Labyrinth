using System;
using System.Collections.Generic;
using static Utilities;

public class Player
{
    public readonly ISet<Character> characters = new HashSet<Character>();
    private Character selectedCharacter;

    private SelectionType currentOrders;
    private IDictionary<Space, (IList<Space>, int)> selectedSpaces = new Dictionary<Space, (IList<Space>, int)>();

    private readonly GameController gameController;
    private Map map;


    // TODO this should take in the initial characters once there's a character creation menu
    public Player(GameController gameController, Map map)
    {
        this.gameController = gameController;

        this.map = map;
        // create the player in the middle
        var startSpace = map.GetSpawnLocation();

        // TODO creating test character here
        var characterModel = new Character(startSpace, map, CharacterLook.Warrior);
        gameController.CreateEntity(startSpace, characterModel);
        startSpace.Occupier = characterModel;
        characterModel.MoveToSpace(startSpace);

        characters.Add(characterModel);
        selectedCharacter = characterModel;
        currentOrders = SelectionType.none;
    }

    public void Move(Space space)
    {
        if (selectedCharacter == null)
            return;

        selectedCharacter.QueueMove(space);
    }

    public void StartSpaceSelection(SelectionType type)
    {
        if (currentOrders == type)
        {
            ClearSpaceSelection();
            return;
        }

        ClearSpaceSelection();
        currentOrders = type;

        int range = 0;
        if (type == SelectionType.move)
        {
            range = selectedCharacter.MoveRange;
        }
        else if (type == SelectionType.attack)
        {
            range = selectedCharacter.AttackRange;
        }

        selectedSpaces = Dijkstras.GetSpacesInRange(map, selectedCharacter.GetCurrentSpace().Coordinates, range, false);
        selectedSpaces.Remove(selectedCharacter.GetCurrentSpace());
        foreach (var space in selectedSpaces.Keys)
        {
            space.SetSelected(type);
        }
    }

    public void ClearSpaceSelection()
    {
        if(currentOrders == SelectionType.none)
        {
            return;
        }
        currentOrders = SelectionType.none;
        foreach (var space in selectedSpaces.Keys)
        {
            space.SetSelected(SelectionType.none);
        }
        selectedSpaces = null;
    }

    public void SpaceClicked(Space space)
    {
        var path = selectedSpaces[space].Item1;
        Console.WriteLine(path.Count);
        foreach(var step in path)
        {
            Move(step);
        }
        ClearSpaceSelection();
    }
}
