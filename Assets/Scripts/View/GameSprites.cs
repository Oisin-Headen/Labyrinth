using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSprites : MonoBehaviour
{
    // terrain 
    public Sprite Wall;
    public Sprite Floor;
    public Sprite HardWall;
    public Sprite Edge;
    public Sprite Gold;

    // player characters
    public Sprite PlayerWarrior;

    // enemies 
    public Sprite FlameSentinal;

    private static readonly Dictionary<ObstacleType, Sprite> Obstacles = new Dictionary<ObstacleType, Sprite>();
    private static readonly Dictionary<CharacterLook, Sprite> CharacterLooks = new Dictionary<CharacterLook, Sprite>();
    private static readonly Dictionary<EnemyType, Sprite> Enemies = new Dictionary<EnemyType, Sprite>();



    public void Setup()
    {
        Obstacles.Add(ObstacleType.Wall, Wall);
        Obstacles.Add(ObstacleType.Gold, Gold);

        CharacterLooks.Add(CharacterLook.Warrior, PlayerWarrior);

        Enemies.Add(EnemyType.FlameSentinal, FlameSentinal);
    }

    public static Sprite GetSpriteFor(CharacterLook look)
    {
        return CharacterLooks[look];
    }

    public static Sprite GetSpriteFor(EnemyType type)
    {
        return Enemies[type];
    }

    public static Sprite GetSpriteFor(ObstacleType obstacle)
    {
        return Obstacles[obstacle];
    }

}
