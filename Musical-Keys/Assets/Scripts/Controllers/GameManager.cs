using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> Players;
    public GameObject ObjectToSpawn;
    public AudioClip[] musics;

    private int maxSpawn = 30;
    public int currentNumSpawn = 0;
    private bool startNewRound = true;
    public int numberPlayerInactive = 0;
    public int currentRound = 1;
    private int roundMusic = 0;
    private Vector3 spawnPoint;

    private Player[] playerScripts;
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
        DontDestroyOnLoad(gameObject);

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
        //Read the beat timing files
        StreamReader reader = new StreamReader(path1);
        string fileline;
        beatTiming1 = new List<string>{};
        beatTiming2 = new List<string>{};
        beatTiming3 = new List<string>{};

        do
        {
            fileline = reader.ReadLine();
            beatTiming1.Add(fileline);
        } while (fileline != null);

        beatTiming1.RemoveAt(beatTiming1.Count - 1);
        arrayOfBeat1 = beatTiming1.ToArray();

        reader = new StreamReader(path2);

        do
        {
            fileline = reader.ReadLine();
            beatTiming2.Add(fileline);
        } while (fileline != null);

        beatTiming2.RemoveAt(beatTiming2.Count - 1);
        arrayOfBeat2 = beatTiming2.ToArray();

        reader = new StreamReader(path3);

        do
        {
            fileline = reader.ReadLine();
            beatTiming3.Add(fileline);
        } while (fileline != null);

        beatTiming3.RemoveAt(beatTiming3.Count - 1);
        arrayOfBeat3 = beatTiming3.ToArray();

        reader.Close();

        beatIndex = 0;

        audio_source = GetComponent<AudioSource>();

        audio_source.clip = musics[roundMusic];

        spawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (startNewRound)
        {
            foreach(GameObject player in Players)
            {
                Instantiate(player, spawnPoint, Quaternion.identity);
            }
            startNewRound = false;
        }

        /*foreach(GameObject player in Players)
        {
            if (player.GetComponent<Player>().wrong)
            {
                Players.Remove(player);
            }
        }*/

        if(numberPlayerInactive == Players.Count && currentRound < 3)
        {
            numberPlayerInactive = 0;
            StartCoroutine(NextRound());
        }
    }

    void FixedUpdate()
    {
        if (currentRound == 1)
        {
            if (audio_source.time >= Single.Parse(arrayOfBeat1[beatIndex]))
            {
                if (currentNumSpawn < maxSpawn)
                {
                    GameObject clone = Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                    currentNumSpawn++;
                    beatIndex += 2;
                }
            }

        }

        else if (currentRound == 2)
        {
            if (audio_source.time >= Single.Parse(arrayOfBeat2[beatIndex]))
            {
                if (currentNumSpawn < maxSpawn)
                {
                    GameObject clone = Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                    currentNumSpawn++;
                    beatIndex += 2;
                }
            }

        }

        else if (currentRound == 3)
        {
            if (audio_source.time >= Single.Parse(arrayOfBeat3[beatIndex]))
            {
                if (currentNumSpawn < maxSpawn)
                {
                    GameObject clone = Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                    currentNumSpawn++;
                    beatIndex += 2;
                }
            }

        }
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(2.0f);
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(index);
        startNewRound = true;
        beatIndex = 0;
        currentNumSpawn = 0;
        currentRound++;
        roundMusic++;
        audio_source.clip = musics[roundMusic];
        audio_source.Play();
    }
}