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

    private int turnCounter = 1;


    // Start is called before the first frame update. Entry point for the game
    void Start()
    {
        // this needs to be first, since it sets up the static methods.
        Sprites.Setup();

        Map = new Map(this, enemies);
        Player = new Player(this, Map, playerController);
        playerController.player = Player;

        // get the ball rolling
        Player.StartTurn();
    }

    public void EndTurn()
    {
        ++turnCounter;

        foreach(var enemy in enemies)
        {
            enemy.Act();
        }

        Player.StartTurn();
    }

    public void CreateSpaceForModel(Coordinate coords, Space spaceModel)
    {
        var spaceController = Instantiate(Tile, new Vector3(coords.x * TILE_SIZE, coords.y * TILE_SIZE), Quaternion.identity, MapHolder.transform).GetComponent<SpaceController>();

        spaceController.SetModel(spaceModel);
        spaceModel.Controller = spaceController;
    }

    public GameObject CreateObstacleView(int xPos, int yPos)
    {
        return Instantiate(Obstacle, new Vector3(xPos * TILE_SIZE, yPos * TILE_SIZE), Quaternion.identity, MapHolder.transform);
    }

    // helper method, not public
    private void CreateEntity(Space space, IEntity model, Sprite sprite)
    {
        var newEntity = Instantiate(Entity, new Vector3(space.Coordinates.x * TILE_SIZE, space.Coordinates.y * TILE_SIZE), Quaternion.identity);
        newEntity.GetComponent<SpriteRenderer>().sprite = sprite;
        newEntity.GetComponent<EntityController>().SetModel(model);
        model.Controller = newEntity.GetComponent<EntityController>();
    }

    public void CreateEntity(Space space, Character character)
    {
        CreateEntity(space, character, GameSprites.GetSpriteFor(character.Look));
        space.Occupier = character;
    }
    public void CreateEntity(Space space, Enemy enemy)
    {
        CreateEntity(space, enemy, GameSprites.GetSpriteFor(enemy.Type));
        space.Occupier = enemy;
    }
}
