using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject Tile;
    public GameObject Obstacle;
    public GameObject Entity;

    public GameObject MapHolder;

    public Sprite Wall;
    public Sprite Floor;
    public Sprite Hard_wall;
    public Sprite Edge;


    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        var map = new Map(this);
        player = new Player(map, this);
    }

    public GameObject CreateTileView(int xPos, int yPos)
    {
        return Instantiate(Tile, new Vector3(xPos * Utilities.TILE_SIZE, yPos * Utilities.TILE_SIZE), Quaternion.identity, MapHolder.transform);
    }

    public GameObject CreateObstacleView(int xPos, int yPos)
    {
        return Instantiate(Obstacle, new Vector3(xPos * Utilities.TILE_SIZE, yPos * Utilities.TILE_SIZE), Quaternion.identity, MapHolder.transform);
    }

    public void CreateEntity(Utilities.Coordinate coordinates, IAmAnEntity model)
    {
        var newEntity = Instantiate(Entity, new Vector3(coordinates.x * Utilities.TILE_SIZE, coordinates.y * Utilities.TILE_SIZE), Quaternion.identity);
        newEntity.GetComponent<EntityController>().SetModel(model);
        model.SetView(newEntity.GetComponent<EntityController>());
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            Move();
        }
    }

    public void Move()
    {
        player.Move();
    }
}
