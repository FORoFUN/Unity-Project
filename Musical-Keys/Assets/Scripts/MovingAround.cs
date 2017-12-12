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

    private float movingTime;
    private float distance;
    public float speed = 0.5f;
    private int waiting;

    private void Start()
    {
        targetposition = new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
        distance = Vector3.Distance(targetposition, transform.position);
        movingTime = distance / speed;
        waiting = 2;
    }

    void FixedUpdate()
    {

        if (waiting == 0)
        {
            StartCoroutine(SetNewPosition());
        }
        else if(waiting == 2)
        {
            LeanTween.move(gameObject, targetposition, movingTime);
            waiting = 0;
        }
    }


    IEnumerator SetNewPosition()
    {
        waiting = 1;
        yield return new WaitForSeconds(movingTime);
        targetposition = new Vector3(Random.Range(xmin, xmax), Random.Range(ymin, ymax));
        distance = Vector3.Distance(targetposition, transform.position);
        movingTime = distance / speed;
        waiting = 2;
    }
}
