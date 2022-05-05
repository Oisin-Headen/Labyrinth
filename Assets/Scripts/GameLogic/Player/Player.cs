﻿using System;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class Player
{
    public readonly ISet<Character> characters = new HashSet<Character>();
    private Character selectedCharacter;

    private SelectionType currentOrders;
    private IDictionary<Space, (IList<Space>, int)> selectedSpaces = new Dictionary<Space, (IList<Space>, int)>();

    private readonly GameController gameController;
    private readonly Map map;

    // TODO this should take in the initial characters once there's a character creation menu
    public Player(GameController gameController, Map map)
    {
        this.gameController = gameController;

        this.map = map;
        // create the player in the middle

        foreach (var look in new CharacterLook[]{CharacterLook.Warrior, CharacterLook.Mage})
        {
            var startSpace = map.GetSpawnLocation();

            // TODO creating test character here
            var characterModel = new Character(startSpace, map, look);
            gameController.CreateEntity(startSpace, characterModel);
            startSpace.Occupier = characterModel;
            characterModel.MoveToSpace(startSpace);

            characters.Add(characterModel);
        }
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

    private void ClearSelectedPlayer()
    {
        selectedCharacter.GetCurrentSpace().SetSelected(SelectionType.none);
        selectedCharacter = null;
    }

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
            Move(step);
        }
        ClearSpaceSelection();
        ClearSelectedPlayer();
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
        ClearSelectedPlayer();
    }

   
}
