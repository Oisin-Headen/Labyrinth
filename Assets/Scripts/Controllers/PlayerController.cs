using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Player player;
    public GameController gameController;

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            player.StartSpaceSelection(SelectionType.move);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player.StartSpaceSelection(SelectionType.attack);
        }
    }

}