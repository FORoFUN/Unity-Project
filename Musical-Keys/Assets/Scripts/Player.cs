using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private Color color;
    private ParticleSystem ps;

    public GameObject slash;
    public TextMesh tm;
    public GameObject spriteObject;

    private bool startPressing;
    private bool changeSize;

    public bool right;
    public bool wrong;

    private int keyPressed; // 0 = user has not pressed key, 1 = user pressed the right key, 2 = user pressed the wrong key

    private string[] playerKeys = { "H", "B", "C", "N", "O", "F", "P", "S", "K", "V", "I", "Y", "U" };
    private int keyIndex;

    private GameManager gm;

    // Use this for initialization
    void Start () {
        ps = GetComponent<ParticleSystem>();

        color = new Color(1.0f, 1.0f, 1.0f, 150.0f/255.0f);
        spriteObject.GetComponent<SpriteRenderer>().color = color;

        startPressing = false;
        changeSize = false;
        right = false;
        wrong = false;

        keyPressed = 0;

        keyIndex = Random.Range(0, playerKeys.Length-1);
        tm.text = playerKeys[keyIndex];

        StartCoroutine(ChangeSizeAndBrightness());

        gm = GameManager.Instance;
    }
	
	// Update is called once per frame
	void Update () {

        if(keyPressed == 2)
        {
            slash.SetActive(true);
        }

        if(keyPressed != 0)
        {
            StartCoroutine(DestroyPlayer());
            keyPressed = 0;
        }

        if (changeSize)
        {
            LeanTween.scale(spriteObject, spriteObject.transform.localScale * 2.0f, 1f).setEase(LeanTweenType.easeOutBounce);
            changeSize = false;
        }

        if (startPressing)
        {
            if (Input.anyKeyDown)
            {
                if (Input.inputString == tm.text.ToLower())
                {
                    ps.Play();
                    keyPressed = 1;
                    right = true;
                    spriteObject.SetActive(false);
                }

                else
                {
                    wrong = true;
                    gm.Players.Remove(gameObject);
                    keyPressed = 2;
                }
            }
        }
	}

    IEnumerator ChangeSizeAndBrightness()
    {
        yield return new WaitForSeconds(5);
        //yield return new WaitForSeconds(Random.Range(30, 50));
        startPressing = true;
        changeSize = true;

        color.a = 1.0f;
        LeanTween.color(spriteObject, color, 1f);

        yield return new WaitForSeconds(2.0f);
        if(keyPressed == 0)
        {
            wrong = true;
            gm.Players.Remove(gameObject);
            keyPressed = 2;
        }
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(1.0f);
        gm.numberPlayerInactive++;
        slash.SetActive(false);
        spriteObject.SetActive(false);
    }
}
