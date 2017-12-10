using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public GameObject[] Player;
    public GameObject ObjectToSpawn;
    public AudioClip[] music;

    private int maxSpawn = 30;
    public int currentNumSpawn = 0;

    private AudioSource audio_source;

    private int beatIndex;
    private List<string> beatTiming1;
    private List<string> beatTiming2;
    private List<string> beatTiming3;
    private string[] arrayOfBeat1;
    private string[] arrayOfBeat2;
    private string[] arrayOfBeat3;

    private string path1 = "Assets/Music/1.txt";
    private string path2 = "Assets/Music/2.txt";
    private string path3 = "Assets/Music/3.txt";

    private static GameManager _instance;

    public static GameManager Instance {
        get {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Use this for initialization
    void Start()
    {
        StreamReader reader = new StreamReader(path1);
        string fileline;
        beatTiming1 = new List<string>{};

        do
        {
            fileline = reader.ReadLine();
            beatTiming1.Add(fileline);
        } while (fileline != null);

        beatTiming1.RemoveAt(beatTiming1.Count - 1);
        arrayOfBeat1 = beatTiming1.ToArray();
        beatIndex = 0;

        audio_source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        if (audio_source.time >= Single.Parse(arrayOfBeat1[beatIndex]))
        {
            if (currentNumSpawn < maxSpawn)
            {
                GameObject clone = Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                currentNumSpawn++;
                beatIndex += 4;
            }
        }
    }
}