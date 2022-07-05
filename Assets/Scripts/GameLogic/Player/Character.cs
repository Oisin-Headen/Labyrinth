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
    public int AttackValue { get; private set; }
    public int AttackRange { get; private set; }

    public int MoveRange { get; private set; }
    public int RemainingMovement { get; private set; }



    // TODO Characters should be expanded, with a 'Class' class.

    // TODO temp variables, should be put elsewhere. 'Race' and 'Weapon' classes
    private int viewRange = 5;

    public Character(Map map, CharacterLook look, Space spawnSpace) : base(map, spawnSpace)
    {
        Look = look;

        //TODO do actual stats
        if (look == CharacterLook.Warrior)
        {
            stats = new StatBlock(5, 3, 3, 3, 3, 3);
            observerList.Add(new BasicWeapon("Hammer", 2));
        }
        else
        {
            stats = new StatBlock(2, 2, 2, 4, 4, 4);
            observerList.Add(new BasicWeapon("Mage Staff", 0, 2));
        }
        StartTurn();
    }

    private void CalculateFromStats()
    {
        MoveRange = stats.Dexerity;
        AttackValue = stats.Strength;
        AttackRange = 1;
        foreach(var observer in observerList)
        {
            var changes = observer.WhenCalculatingFromStats();
            MoveRange += changes.moveAddition;
            AttackValue += changes.attackAddition;
            AttackRange += changes.attackRangeAddition;
        }
    }


    public void StartTurn()
    {
        // TODO reset movement points, heal health and mana, etc
        CalculateFromStats();

        RemainingMovement = MoveRange;
    }

    public override void MoveToSpace(Space space)
    {
        base.MoveToSpace(space);
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
