using UnityEngine;
using System.Collections;

public class RandomObject : MonoBehaviour
{
    private Color color;
    private SpriteRenderer sr;
    public TextMesh tm;
    //public GameManager gm;
    //public ParticleSystem ps;
    public Sprite[] spriteList;
    private int spriteIndex;
    private bool destroy;
    private bool startPressing;
    private bool startChanging;
    private int keyIndex;
    private char[] possibleKeys = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I',
        'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W',
        'X', 'Y', 'Z', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    // Use this for initialization
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = new Color(Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, 150.0f / 255.0f);
        sr.color = color;
        destroy = true;
        startPressing = false;
        startChanging = true;
        spriteIndex = Random.Range(0, spriteList.Length - 1);
        sr.sprite = spriteList[spriteIndex];
        keyIndex = Random.Range(0, possibleKeys.Length - 1);
        tm.text = possibleKeys[keyIndex].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (startChanging)
        {
            StartCoroutine(ChangeSizeAndBrightness());
        }
        if (startPressing)
        {
            if (Input.inputString == possibleKeys[keyIndex].ToString().ToLower())
            {
                //ps.Play();
                gameObject.SetActive(false);
            }
        }
    }

    public IEnumerator ChangeSizeAndBrightness()
    {
        startChanging = false;
        yield return new WaitForSeconds(2.5f);
        startPressing = true;
        color.a = 1.0f;
        sr.color = color;
        transform.localScale = transform.localScale * 1.5f;
        StartCoroutine(DestroyPlayer());
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(1);
        if (destroy)
        {
            //gm.currentNumSpawn--;
            gameObject.SetActive(false);
        }
    }
}
