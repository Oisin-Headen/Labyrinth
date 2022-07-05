using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using static Utilities;

public class Character : AbstractEntity, IOccupy, IEntity, IViewSpaces, ICharacterObservable
{
    // Initilising to an empty set.
    private ISet<Space> spacesInView = new HashSet<Space>();
    private ISet<IObserveCharacters> observerList = new HashSet<IObserveCharacters>();


    public readonly StatBlock stats;

    public CharacterLook Look { get; private set; }

    public override bool BlocksLOS { get { return false; } }
    public override int Armour { get { return 0; } }
    public DamageType WeaponDamageType { get { return DamageType.Bludgeoning; } }

    // TODO Increase by weapon damage or something.
    private ExtraCharacterStatistics extraStats;

    public int AttackValue { get { return extraStats.AttackValue; } }
    public int AttackRange { get { return extraStats.AttackRange; } }
    public int RemainingMovement { get { return extraStats.RemainingMovement; } }


    // TODO Characters should be expanded, with a 'Class' class.

    // TODO temp variables, should be put elsewhere. 'Race' and 'Weapon' classes
    private int viewRange = 5 * Dijkstras.ONE_SPACE;

    public Character(Map map, CharacterLook look, Space spawnSpace) : base(map, spawnSpace)
    {
        Look = look;

        //TODO do actual stats
        if (look == CharacterLook.Warrior)
        {
            stats = new StatBlock(5, 3, 3, 3, 3, 3);
            observerList.Add(new BasicWeapon("Hammer", 5));
        }
        else
        {
            stats = new StatBlock(2, 2, 2, 4, 4, 4);
            observerList.Add(new BasicWeapon("Mage Staff", 2, 2));
        }
        CalculateFromStats();
        StartTurn();
    }

    private void CalculateFromStats()
    {
        extraStats = new ExtraCharacterStatistics(stats);

        foreach(var observer in observerList)
        {
            observer.WhenCalculatingFromStats(extraStats);
        }

        // TODO, you know, maybe this isn't the place to be doing this, but we need to keep track of fractional moves.
        extraStats.MoveRange *= Dijkstras.ONE_SPACE;
    }


    public void StartTurn()
    {
        // TODO reset movement points, heal health and mana, etc


        extraStats.RemainingMovement = extraStats.MoveRange;
    }

    public override void MoveToSpace(Space space)
    {
        base.MoveToSpace(space);
        extraStats.RemainingMovement -= space.MovementCost * Dijkstras.ONE_SPACE;
        RefreshLineOfSight();
    }

    public void StopViewingSpaces()
    {
        foreach (var oldSpaceInView in spacesInView)
        {
            oldSpaceInView.RemoveViewer(this);
        }
        spacesInView.Clear();
    }

    public void RefreshLineOfSight()
    {
        StopViewingSpaces();
        spacesInView = FieldOfView.GetAllSpacesInSightRange(map, currentSpace, viewRange);
        spacesInView.IntersectWith(Dijkstras.GetSpacesInRange(map, currentSpace, viewRange, true).Keys);
        spacesInView.Add(currentSpace);

        foreach (var newSpaceInView in spacesInView)
        {
            newSpaceInView.AddViewer(this);
        }
    }


    public override DamageEffectiveness GetDamageEffectiveness(DamageType damageType)
    {
        return DamageEffectiveness.Normal;
    }

    //TODO
    public override bool TakeDamage(int value)
    {
        throw new NotImplementedException();
    }

    public void AddObserver(IObserveCharacters newObserver)
    {
        observerList.Add(newObserver);
    }

    public void RemoveObserver(IObserveCharacters observer)
    {
        observerList.Remove(observer);
    }
}
