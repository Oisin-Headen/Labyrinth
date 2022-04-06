using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    private IAmAnEntity model;

    private bool moving = false;

    private Vector2 startPos, endPos;
    private float startTime;
    private float distance;

    public void SetModel(IAmAnEntity model)
    {
        this.model = model;
        //gameObject.GetComponent<SpriteRenderer>().sprite = model.GetSprite();
    }


    // Update is called once per frame
    void Update()
    {
        if(moving)
        {
            float current = (Time.time - startTime) * Utilities.MOVEMENT_SPEED;
            float fraction = current / distance;
            fraction = Mathf.Max(0f, fraction);
            fraction = Mathf.Min(1f, fraction);


            transform.position = Vector2.Lerp(startPos, endPos, fraction);

            if (fraction >= 1f)
            {
                moving = false;
            }
        }
        else
        {
            model.MoveReady();
        }
    }

    public void MoveTo(GameObject gameObject)
    {
        moving = true;

        startPos = transform.position;
        startTime = Time.time;
        endPos = gameObject.transform.position;
        distance = Vector2.Distance(startPos, endPos);
    }
}
