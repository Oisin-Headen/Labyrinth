using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Player
{
    public readonly ISet<Character> characters = new HashSet<Character>();
    private Character selectedCharacter;

    private SelectionType currentOrders = SelectionType.none;
    private IDictionary<Space, (IList<Space>, int)> selectedSpaces = new Dictionary<Space, (IList<Space>, int)>();

    private readonly GameManager gameManager;
    private readonly Map map;
    private readonly PlayerController controller;

    // TODO this should take in the initial characters once there's a character creation menu
    public Player(GameManager gameController, Map map, PlayerController controller)
    {
        this.gameManager = gameController;

        this.map = map;

        this.controller = controller;
        // create the player in the middle

        foreach (var look in new CharacterLook[]{CharacterLook.Warrior, CharacterLook.Mage})
        {
            var startSpace = map.GetSpawnLocation();

            // TODO creating test character here
            var characterModel = new Character(map, look, startSpace);
            gameController.CreateEntity(startSpace, characterModel);
            startSpace.Occupier = characterModel;
            characterModel.MoveToSpace(startSpace);

            characters.Add(characterModel);
        }
    }

    public void MoveSelectedCharacterTo(Space space)
    {
        if (selectedCharacter == null)
            return;

        selectedCharacter.QueueMove(space);
    }

    public void StartSpaceSelection(SelectionType type)
    {
        if (selectedCharacter == null)
            return;

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

        selectedSpaces = Dijkstras.GetSpacesInRange(map, selectedCharacter.GetCurrentSpace().Coordinates, range, type==SelectionType.attack);
        selectedSpaces.Remove(selectedCharacter.GetCurrentSpace());
        foreach (var space in selectedSpaces.Keys)
        {
            space.SetSelected(type);
        }
    }

    private void ClearSpaceSelection()
    {
        if (currentOrders == SelectionType.none)
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

    private void ClearSelectedCharacter()
    {
        selectedCharacter.GetCurrentSpace().SetSelected(SelectionType.none);
        selectedCharacter = null;
    }

    // TODO Is this the kind of thing a player should be doing?
    public void SpaceClickedOn(Space space)
    {
        if(space.Occupier != null && space.Occupier.GetType() == typeof(Character))
        {
            if(selectedCharacter != null)
            {
                selectedCharacter.GetCurrentSpace().SetSelected(SelectionType.none);
            }
            selectedCharacter = (Character)space.Occupier;
            selectedCharacter.GetCurrentSpace().SetSelected(SelectionType.character);
        }
        else if(currentOrders == SelectionType.move)
        {
            SpaceClickedMove(space);
        }
        else if(currentOrders == SelectionType.attack)
        {
            SpaceClickedAttack(space);
        }
    }

    private void SpaceClickedMove(Space space)
    {
        var path = selectedSpaces[space].Item1;
        Console.WriteLine(path.Count);
        foreach (var step in path)
        {
            MoveSelectedCharacterTo(step);
        }
        ClearSpaceSelection();
        ClearSelectedCharacter();
    }

    private void SpaceClickedAttack(Space space)
    {
        if (space.IsEmpty)
        {
            ClearSpaceSelection();
            return;
        }

        int damage = CombatCalculator.CalculateDamage(
            selectedCharacter.AttackValue,
            space.Occupier.Armour,
            space.Occupier.GetDamageEffectiveness(selectedCharacter.WeaponDamageType));

        // TODO shouldn't be in player, should be in the occupiers themselves
        bool targetDestroyed = space.Occupier.TakeDamage(damage);
        if (targetDestroyed)
        {
            space.Occupier = null;
            foreach(var character in characters)
            {
                character.MoveToSpace(character.GetCurrentSpace());
            }
        }

        ClearSpaceSelection();
        ClearSelectedCharacter();
    }

    public void StartTurn()
    {
        foreach (var character in characters)
        {
            character.StartTurn();
        }

        controller.StartTurn();
    }
}
