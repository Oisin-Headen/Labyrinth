using UnityEngine;
using static Utilities;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Obstacle;
    public GameObject Entity;

    public GameObject MapHolder;
    public GameSprites Sprites;

    // this is plugged in via Unity. Don't delete it again.
    public PlayerController playerController;

    public Player Player { get; private set; }
    public Map Map { get; private set; }

    private List<Enemy> enemies = new List<Enemy>();
    private List<IEnemyAI> enemyAIs = new List<IEnemyAI>();

    private int turnCounter = 1;
    private bool playerTurn = true;


    // Start is called before the first frame update. Entry point for the game
    void Start()
    {
        // this needs to be first, since it sets up the static methods.
        Sprites.Setup();

        Map = new Map(this, enemies);

        var sentinal = new SentinalAI(Map);
        sentinal.AddEnemies(enemies);
        enemyAIs.Add(sentinal);

        Player = new Player(this, Map, playerController);
        playerController.player = Player;

        // get the ball rolling
        Player.StartTurn();
    }

    public void EndTurn()
    {
        Player.EndTurn();
        ++turnCounter;
        playerTurn = false;

        foreach(var enemyAI in enemyAIs)
        {
            enemyAI.Act();
        }
    }

    public void StartTurn()
    {
        playerTurn = true;
        Player.StartTurn();
    }



    public void Update()
    {
        if(!playerTurn)
        {
            var nextAITask = enemyAIs[0].CompletedTask();
            while(nextAITask != null)
            {
                nextAITask.Wait();
                nextAITask = enemyAIs[0].CompletedTask();
            }
            if(!enemyAIs[0].TasksRemaining())
            {
                StartTurn();
            }
        }
    }


    public void CreateSpaceForModel(Space spaceModel)
    {
        var spaceController = Instantiate(Tile, new Vector3(spaceModel.Coordinates.x * TILE_SIZE, spaceModel.Coordinates.y * TILE_SIZE), Quaternion.identity, MapHolder.transform).GetComponent<SpaceController>();

        spaceController.SetModel(spaceModel);
        spaceModel.Controller = spaceController;
    }

    public Obstacle CreateObstacle(Space spaceModel, ObstacleType type)
    {
        var obstacleView = Instantiate(Obstacle, spaceModel.Controller.transform.position, Quaternion.identity, MapHolder.transform);
        var obstacle = new Obstacle(obstacleView, spaceModel, type);
        spaceModel.Occupier = obstacle;
        return obstacle;
    }



    // helper method, not public
    private void CreateEntity(Space space, IEntity model, Sprite sprite)
    {
        var newEntity = Instantiate(Entity, new Vector3(space.Coordinates.x * TILE_SIZE, space.Coordinates.y * TILE_SIZE), Quaternion.identity);
        newEntity.GetComponent<SpriteRenderer>().sprite = sprite;
        newEntity.GetComponent<EntityController>().SetModel(model);
        model.Controller = newEntity.GetComponent<EntityController>();
    }

    public void CreateCharacterViewFor(Character character)
    {
        CreateEntity(character.CurrentSpace, character, GameSprites.GetSpriteFor(character.Look));
    }

    public Enemy CreateEnemy(Space space, EnemyType enemyType)
    {
        var enemy = new Enemy(Map, enemyType, space);
        CreateEntity(space, enemy, GameSprites.GetSpriteFor(enemy.Type));
        space.Occupier = enemy;
        return enemy;
    }
}
