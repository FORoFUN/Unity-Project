using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    //Variables
    private Color color;
    private Color wrongColor;
    private ParticleSystem ps;

    public TextMesh tm;
    public GameObject spriteObject;
    public RectTransform bar;
    public RectTransform canvas;

    public bool startPressing;
    private bool changeSize;
    private bool startMissTimer;
    public bool changeKey;
    private bool pressedRightKey;

    private Vector3 originalSize;

    public int hp;

    public bool dead;

    private int keyPressed; // 0 = user has not pressed key, 1 = user pressed the right key, 2 = user has not pressed the right key in the given time

    private GameManager gm;

    // Use this for initialization
    void Start () {
        //Initialize variables
        ps = GetComponent<ParticleSystem>();

        color = new Color(1.0f, 1.0f, 1.0f, 150.0f/255.0f);
        wrongColor = new Color(1.0f, 0.0f, 0.0f, 150.0f/255.0f);
        spriteObject.GetComponent<SpriteRenderer>().color = color;

        startPressing = false;
        changeSize = false;
        startMissTimer = false;
        pressedRightKey = false;
        changeKey = false;

        originalSize = spriteObject.transform.localScale;
        hp = 3;

        dead = false;

        keyPressed = 0;

        StartCoroutine(ChangeSizeAndBrightness());

        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update() {

        //Change HP bar based on size
        if (hp == 3)
        {
            bar.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
        }

        else if (hp == 2)
        {
            bar.localScale = new Vector3 (0.6f, 1.0f, 1.0f);
        }

        else if (hp == 1)
        {
            bar.localScale = new Vector3 (0.3f, 1.0f, 1.0f);
        }

        else if (hp == 0)
        {
            bar.localScale = new Vector3 (0.0f, 1.0f, 1.0f);
            if (!dead)
            {
                StartCoroutine(SetInactive());
            }
        }

        //Changing size to notice player to press key
        if (changeSize)
        {
            LeanTween.scale(spriteObject, spriteObject.transform.localScale * 1.7f, 0.25f).setEase(LeanTweenType.easeOutBounce);
            LeanTween.moveLocalY(canvas.gameObject, -1.2f, 0.25f).setEase(LeanTweenType.easeOutBounce);
            changeSize = false;
        }

        //If player press the right key no slash will appear and some particles will appear and call the SetInactive function
        if (startPressing)
        {
            if (!startMissTimer)
            {
                StartCoroutine(SetMiss());
                startMissTimer = true;
            }

            if (Input.anyKeyDown)
            {
                if (Input.inputString == tm.text.ToLower())
                {
                    ps.Play();
                    LeanTween.scale(spriteObject, originalSize, 0.25f);

                    LeanTween.moveLocalY(canvas.gameObject, -0.75f, 0.25f).setEase(LeanTweenType.easeOutBounce);

                    color.a = 150.0f / 255.0f;
                    LeanTween.color(spriteObject, color, 0.25f);

                    startPressing = false;
                    pressedRightKey = true;
                    changeKey = true;
                }
            }
        }

        else
        {
            if (Input.anyKeyDown)
            {
                if (Input.inputString == tm.text.ToLower())
                {
                    Input.ResetInputAxes();
                    StartCoroutine(MarkWrong());
                    if (hp > 0)
                    {
                        hp--;
                    }
                }
            }
        }
	}

    IEnumerator MarkWrong()
    {
        LeanTween.color(spriteObject, wrongColor, 0.25f);
        yield return new WaitForSeconds(0.25f);
        LeanTween.color(spriteObject, color, 0.25f);
    }

    IEnumerator ChangeSizeAndBrightness()
    {
        //After a random time between 5 to 10 seconds after the player spawn the user will have to press the key
        yield return new WaitForSeconds(Random.Range(5, 10));
        startPressing = true;
        changeSize = true;

        color.a = 1.0f;
        LeanTween.color(spriteObject, color, 0.25f);
    }

    IEnumerator SetMiss()
    {
        yield return new WaitForSeconds(1.0f);
        if (!pressedRightKey)
        {
            LeanTween.scale(spriteObject, originalSize, 0.25f);
            LeanTween.moveLocalY(canvas.gameObject, -0.75f, 0.25f).setEase(LeanTweenType.easeOutBounce);
            color.a = 150.0f / 255.0f;
            LeanTween.color(spriteObject, color, 0.25f);

            hp--;
            startMissTimer = false;
            startPressing = false;
        }

        else
        {
            pressedRightKey = false;
            startMissTimer = false;
        }

        if (hp > 0)
        {
            StartCoroutine(ChangeSizeAndBrightness());
        }
    }

    IEnumerator SetInactive()
    {
        dead = true;
        //Increase the inactive player # in GM and remove player from the scene
        yield return new WaitForSeconds(2.0f);
        //gm.numberPlayerInactive++;
        spriteObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }
}