using System;
using UnityEngine;

public interface IEntity : IOccupy
{
    public void MoveReady();
    public void QueueMove(Space space);

    public EntityController Controller { get; set; }
}
