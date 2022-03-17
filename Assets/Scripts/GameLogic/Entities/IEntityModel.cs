using System;
public interface IAmAnEntity
{
    public void MoveReady();
    public void QueueMove(Utilities.CardinalDirection direction);
    public void SetView(EntityController controller);
}
