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
    private string[] possibleKeys = {"q w e", "w e r", "e r t", "r t y", "t y u", "y u i", "u i o", "i o p"
            , "a s d", "s d f", "d f g", "f g h", "g h j", "h j k", "j k l"
            , "z x c", "x c v", "c v b", "v b n", "b n m"};
    private string[] assignedKeys;

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
        assignedKeys = possibleKeys[keyIndex].Split();
        tm.text = assignedKeys[Random.Range(0, 2)].ToUpper();

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
            transform.localScale = Vector3.Lerp(transform.localScale, transform.localScale * 1.5f, Time.deltaTime);
        }
        if (startPressing)
        {
            if (Input.inputString == tm.text.ToLower())
            {
                cam.GetComponent<CameraShake>().Shake();
                gameObject.SetActive(false);
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
            Destroy(gameObject);
        }
    }
}
