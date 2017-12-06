using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingAround : MonoBehaviour
{

    public float xmin;
    public float xmax;
    public float ymin;
    public float ymax;
    private Vector3 targetposition;
    private float smoothingTime = 1f;

    public bool waiting = false;
    private float moveDelay = 1;
    void FixedUpdate()
    {

        if (waiting == false)
        {
            SendMessage("LerpThings");
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetposition, Time.deltaTime * smoothingTime);
        }
    }


    IEnumerator LerpThings()
    {
        targetposition = new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
        waiting = true;
        yield return new WaitForSeconds(moveDelay);
        waiting = false;
        //Debug.Log ("waited");
    }
}
