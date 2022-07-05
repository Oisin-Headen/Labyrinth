using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SentinalAI : IEnemyAI
{
    private readonly ISet<Enemy> enemies = new HashSet<Enemy>();
    private readonly ISet<Task> enemyTasks = new HashSet<Task>();
    private readonly Map map;

    public SentinalAI(Map map)
    {
        this.map = map;
    }

    public void AddEnemies(IEnumerable<Enemy> newEnemies)
    {
        enemies.UnionWith(newEnemies);
    }


    public void Act()
    {
        foreach(var enemy in enemies)
        {
            enemyTasks.Add(Task.Run(() =>
            {
                // TODO don't want to modify stuff on other threads. Return the action the enemy will take.
                foreach (var space in FieldOfView.GetAllSpacesInSightRange(map, enemy.CurrentSpace, 1))
                {
                    if (!space.IsEmpty && space.Occupier.GetType() == typeof(Character))
                    {
                        // TODO this should be a list of actions returned to the main thread, to be excuted there.
                        //int damage = CombatCalculator.CalculateDamage(
                        //    enemy.Type.attackValue,
                        //    space.Occupier.Armour,
                        //    space.Occupier.GetDamageEffectiveness(enemy.Type.damageType));
                        //space.Occupier.TakeDamage(damage);
                    }
                }
            }));
        }
    }

    public Task CompletedTask()
    {
        foreach(var task in enemyTasks)
        {
            if(task.IsCompleted)
            {
                enemyTasks.Remove(task);
                return task;
            }
        }
        return null;
    }

    public bool TasksRemaining()
    {
        return enemyTasks.Count > 0;
    }
}
