using System;
public interface IEntityModel
{
    public void MoveReady();
    public void QueueMove(Utilities.CardinalDirection direction);
    public void SetView(EntityController controller);
}
