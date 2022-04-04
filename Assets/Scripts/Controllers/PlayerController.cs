using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utilities;

public class PlayerController : MonoBehaviour
{
    public Player player;



    public void Update()
    {



        if (Input.GetKeyUp(KeyCode.W))
        {
            player.Move(CardinalDirection.Up);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            player.Move(CardinalDirection.Left);
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            player.Move(CardinalDirection.Down);
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            player.Move(CardinalDirection.Right);
        }
    }

}