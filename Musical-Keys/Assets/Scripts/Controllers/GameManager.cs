using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //Variables
    public List<GameObject> Players;
    public List<GameObject> currentPlayers;
    public GameObject ObjectToSpawn;
    public AudioClip[] musics;


    // winner GameObject
    public GameObject winner = null;

    public int maxSpawn = 25;
    public int currentNumSpawn = 0;
    public bool startNewRound = true;
    public bool gameOver = false;
    public int roundMusic;
    public bool generatingObstacles = true;

    private Vector3 spawnPoint;

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

    public List<int> keyIndexes;

    private string[] playerKeys = { "H", "B", "C", "N", "O", "F", "P", "S", "K", "V", "I", "Y", "U" };

    //Creating a singleton of Game Manager
    private static GameManager _instance;

    public static GameManager Instance {
        get {
            return _instance;
        }
    }

    //The Game Manager will not be destroy between scenes
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

        //Initialize some variables
        beatIndex = 0;

        roundMusic = UnityEngine.Random.Range(0, 2);

        audio_source = GetComponent<AudioSource>();

        audio_source.clip = musics[roundMusic];

        audio_source.Play();

        spawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        //When a new round start, create the players on screen and assign an unique key to them from the list of possible keys
        if (startNewRound)
        {
            foreach (GameObject player in Players)
            {
                GameObject clone = Instantiate(player, spawnPoint, Quaternion.identity);

                //This will ensure each has an unique key
                int randomNumber = UnityEngine.Random.Range(0, playerKeys.Length - 1);
                while (keyIndexes.Contains(randomNumber)) {
                    randomNumber = UnityEngine.Random.Range(0, playerKeys.Length - 1);
                }
                keyIndexes.Add(randomNumber);

                //Assign the key from the list of key indexes to the player
                clone.GetComponent<Player>().tm.text = playerKeys[keyIndexes[keyIndexes.Count - 1]];

                //Add those to current players list
                currentPlayers.Add(clone);
            }
            startNewRound = false;
        }

        //Look for players who did not press the right key on time and remove them from the game
        //Compare tag of the prefabs to the actual object in the scene to remove properly
        foreach (GameObject curPlayer in currentPlayers)
        {
            if (curPlayer.GetComponent<Player>().changeKey)
            {
                int randomNumber = UnityEngine.Random.Range(0, playerKeys.Length - 1);
                while (keyIndexes.Contains(randomNumber))
                {
                    randomNumber = UnityEngine.Random.Range(0, playerKeys.Length - 1);
                }
                keyIndexes[currentPlayers.IndexOf(curPlayer)] = randomNumber;
                curPlayer.GetComponent<Player>().tm.text = playerKeys[keyIndexes[currentPlayers.IndexOf(curPlayer)]];
                curPlayer.GetComponent<Player>().changeKey = false;
            }

            if (curPlayer.GetComponent<Player>().dead)
            {
                foreach (GameObject player in Players)
                {
                    if (player.tag == curPlayer.tag)
                    {
                        Players.Remove(player);
                    }
                }
            }
        }

        //If there are player in the scene and all players are either right or wrong and the current round is less than 3
        //then go to the next round and reset variables
        if(Players.Count == 1 && !gameOver)
        {
            gameOver = true;
            StartCoroutine(GoToEndGame());
        }

        if(SceneManager.GetActiveScene().name == "Main Menu")
        {
            Destroy(gameObject);
        }
    }

    //Spawn obstacles according to the beat of the music
    void FixedUpdate()
    {
        if (generatingObstacles)
        {
            if (roundMusic == 0)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat1[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 2;
                    }
                }

            }

            else if (roundMusic == 1)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat2[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 2;
                    }
                }

            }

            else if (roundMusic == 2)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat3[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 2;
                    }
                }

            }
        }
    }

    IEnumerator GoToEndGame()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Round Transition");
        audio_source.Stop();
        // set winner here?
        currentPlayers = new List<GameObject>();
        keyIndexes = new List<int>();
        audio_source.Stop();
        generatingObstacles = false;
        //Destroy(gameObject);
    }
}