using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public GameManager gameManager;
    private bool acceptingInput = false;

    public void Update()
    {
        if (!acceptingInput)
        {
            return;
        }
        if (Input.GetKeyUp(KeyCode.M))
        {
            player.StartSpaceSelection(SelectionType.move);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player.StartSpaceSelection(SelectionType.attack);
        }
        if (Input.GetKeyUp(KeyCode.Return))
        {
            acceptingInput = false;
            gameManager.EndTurn();
        }
    }


    public void StartTurn()
    {
        acceptingInput = true;
    }
}