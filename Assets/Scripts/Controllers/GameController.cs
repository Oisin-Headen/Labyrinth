using UnityEngine;
using static Utilities;

public class GameController : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Obstacle;
    public GameObject Entity;

    public GameObject MapHolder;
    public GameSprites Sprites;

    // this is plugged in via Unity
    public PlayerController playerController;

    public Player Player { get; private set; }
    public Map Map { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        Sprites.Setup();
        Map = new Map(this);
        Player = new Player(this, Map);
        playerController.player = Player;
    }

    public void CreateSpaceForModel(Coordinate coords, Space spaceModel)
    {
        var spaceController = Instantiate(Tile, new Vector3(coords.x * TILE_SIZE, coords.y * TILE_SIZE), Quaternion.identity, MapHolder.transform).GetComponent<SpaceController>();

        spaceController.SetModel(spaceModel);
        spaceModel.SetController(spaceController);
    }

    public GameObject CreateObstacleView(int xPos, int yPos)
    {
        return Instantiate(Obstacle, new Vector3(xPos * TILE_SIZE, yPos * TILE_SIZE), Quaternion.identity, MapHolder.transform);
    }

    private void CreateEntity(Space space, IAmAnEntity model, Sprite sprite)
    {
        var newEntity = Instantiate(Entity, new Vector3(space.Coordinates.x * TILE_SIZE, space.Coordinates.y * TILE_SIZE), Quaternion.identity);
        newEntity.GetComponent<SpriteRenderer>().sprite = sprite;
        newEntity.GetComponent<EntityController>().SetModel(model);
        model.View = newEntity.GetComponent<EntityController>();
    }

    public void CreateEntity(Space space, Character character)
    {
        CreateEntity(space, character, Sprites.GetSpriteFor(character.Look));
        space.Occupier = character;
    }
    public void CreateEntity(Space space, Enemy enemy)
    {
        CreateEntity(space, enemy, Sprites.GetSpriteFor(enemy.Type));
        space.Occupier = enemy;
    }
}
