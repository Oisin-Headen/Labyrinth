using System;
using UnityEngine;

public interface IAmAnEntity : IOccupy
{
    public void MoveReady();
    public void QueueMove(Space space);

    public EntityController View { get; set; }
}
