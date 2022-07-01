using System.Threading.Tasks;

public interface IEnemyAI
{
    public void Act();
    public Task CompletedTask();
    public bool TasksRemaining();
}