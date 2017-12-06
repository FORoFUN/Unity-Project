using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleFx : MonoBehaviour {

    public float scaleAmt;
    public float lerpTime;

    private Vector3 originalScale;

    public bool autoPlay = false;
	// Use this for initialization
	void Start () {
        originalScale = transform.localScale;
        if (autoPlay)
        {
            Scale();
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale = Vector3.Lerp(transform.localScale, originalScale, lerpTime);
	}

    public void Scale()
    {
        transform.localScale *= scaleAmt;
    }
}
