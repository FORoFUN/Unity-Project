using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Variables
    private Color color;
    private ParticleSystem ps;

    public GameObject slash;
    public TextMesh tm;
    public GameObject spriteObject;

    private bool startPressing;
    private bool changeSize;

    public bool right;
    public bool wrong;

    private int keyPressed; // 0 = user has not pressed key, 1 = user pressed the right key, 2 = user has not pressed the right key in the given time

    private GameManager gm;

    // Use this for initialization
    void Start () {
        //Initialize variables
        ps = GetComponent<ParticleSystem>();

        color = new Color(1.0f, 1.0f, 1.0f, 150.0f/255.0f);
        spriteObject.GetComponent<SpriteRenderer>().color = color;

        startPressing = false;
        changeSize = false;
        right = false;
        wrong = false;

        keyPressed = 0;

        StartCoroutine(ChangeSizeAndBrightness());

        gm = GameManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {

        //If user did not press the right key in time put a red slash on the sprite
        if(keyPressed == 2)
        {
            slash.SetActive(true);
        }

        //Changing size to notice player to press key
        if (changeSize)
        {
            LeanTween.scale(spriteObject, spriteObject.transform.localScale * 1.5f, 0.5f).setEase(LeanTweenType.easeOutBounce);
            changeSize = false;
        }

        //If player press the right key no slash will appear and some particles will appear and call the SetInactive function
        if (startPressing)
        {
            if (Input.anyKeyDown)
            {
                if (Input.inputString == tm.text.ToLower())
                {
                    ps.Play();
                    keyPressed = 1;
                    right = true;
                    startPressing = false;
                    spriteObject.SetActive(false);
                    StartCoroutine(SetInactive());
                }
            }
        }
	}

    IEnumerator ChangeSizeAndBrightness()
    {
        //After a random time between 30 to 50 seconds after the player spawn the user will have to press the key

        //yield return new WaitForSeconds(5);
        yield return new WaitForSeconds(Random.Range(10, 20));
        startPressing = true;
        changeSize = true;

        color.a = 1.0f;
        LeanTween.color(spriteObject, color, 1f);

        //If key not pressed after 2 seconds then put slash on sprite and call SetInactive
        yield return new WaitForSeconds(2.0f);

        if(keyPressed == 0)
        {
            wrong = true;
            startPressing = false;
            keyPressed = 2;
            StartCoroutine(SetInactive());
        }
    }

    IEnumerator SetInactive()
    {
        //Increase the inactive player # in GM and remove player from the scene
        yield return new WaitForSeconds(2.0f);
        keyPressed = 0;
        gm.numberPlayerInactive++;
        slash.SetActive(false);
        spriteObject.SetActive(false);
    }
}
