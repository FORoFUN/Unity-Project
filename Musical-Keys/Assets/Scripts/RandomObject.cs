using UnityEngine;
using System.Collections;

public class RandomObject : MonoBehaviour
{
    private Color color;
    private SpriteRenderer sr;
    public TextMesh tm;
    private Camera cam;
    public Sprite[] spriteList;
    private int spriteIndex;
    private bool destroy;
    private bool startPressing;
    private bool startChanging;
    private bool changeSize = false;
    private int keyIndex;
    private string[] possibleKeys = {"q", "w", "e", "r", "t", "a", "d", "g", "j", "l", "z", "x", "v", "m"};

    private GameManager gm;

    // Use this for initialization
    private void Awake()
    {
        gm = GameManager.Instance;
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = new Color(Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, 50.0f / 255.0f);
        sr.color = color;

        destroy = true;
        startPressing = false;
        startChanging = true;

        spriteIndex = Random.Range(0, spriteList.Length - 1);
        sr.sprite = spriteList[spriteIndex];

        keyIndex = Random.Range(0, possibleKeys.Length - 1);
        tm.text = possibleKeys[keyIndex].ToUpper();

        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (startChanging)
        {
            StartCoroutine(ChangeSizeAndBrightness());
        }
        if (changeSize)
        {
            //transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 1.5f, Time.deltaTime);
            //LeanTween.scale(gameObject, transform.localScale * 1.5f, 1f).setEase(LeanTweenType.easeOutBounce);
        }
        if (startPressing)
        {
            if (Input.inputString == tm.text.ToLower())
            {
                cam.GetComponent<CameraShake>().Shake();
                gm.currentNumSpawn--;
                Destroy(gameObject);
            }
        }
    }

    public IEnumerator ChangeSizeAndBrightness()
    {
        startChanging = false;
        yield return new WaitForSeconds(2.5f);
        startPressing = true;
        changeSize = true;

        color.a = 1.0f;
        LeanTween.color(gameObject, color, 1f);

        //StartCoroutine(DestroyPlayer());
    }

    IEnumerator DestroyPlayer()
    {
        yield return new WaitForSeconds(1.5f);
        if (destroy)
        {
            //gm.currentNumSpawn--;
            Destroy(gameObject);
        }
    }
}
