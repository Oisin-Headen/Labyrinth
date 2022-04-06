using System;
using UnityEngine;

public interface IAmAnEntity : IOccupy
{
    public void MoveReady();
    public void QueueMove(Utilities.CardinalDirection direction);

    public EntityController View { get; set; }
}
