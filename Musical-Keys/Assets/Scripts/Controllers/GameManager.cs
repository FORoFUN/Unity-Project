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

    private int maxSpawn = 30;
    public int currentNumSpawn = 0;
    private bool startNewRound = true;
    public int numberPlayerInactive = 0;
    public int currentRound = 1;
    private int roundMusic = 0;
    private bool roundTransition = false;
    private bool generatingObstacles = true;

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

        audio_source = GetComponent<AudioSource>();

        audio_source.clip = musics[roundMusic];

        spawnPoint = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (roundTransition)
        {
            StartCoroutine(NextRound());
            roundTransition = false;
        }

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
            if (curPlayer.GetComponent<Player>().wrong)
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
        if(Players.Count > 0 && numberPlayerInactive == currentPlayers.Count && currentRound < 3)
        {
            numberPlayerInactive = 0;
            StartCoroutine(GoToRoundTransition());
        }

        //If all players are either right or wrong and the current round is 3
        //then move to game over scene and reset variables
        if (numberPlayerInactive == currentPlayers.Count && currentRound == 3)
        {
            currentRound++;
            StartCoroutine(GoToEndGame());
        }

        //If all players are out and the current round is less than 3
        //then move to game over scene and reset variables
        if(Players.Count == 0 && currentRound < 3)  
        {
            currentRound = 4;
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
            if (currentRound == 1)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat1[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 1;
                    }
                }

            }

            else if (currentRound == 2)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat2[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 1;
                    }
                }

            }

            else if (currentRound == 3)
            {
                if (audio_source.time >= Single.Parse(arrayOfBeat3[beatIndex]))
                {
                    if (currentNumSpawn < maxSpawn)
                    {
                        Instantiate(ObjectToSpawn, new Vector3(0.0f, 0.0f), Quaternion.identity);
                        currentNumSpawn++;
                        beatIndex += 1;
                    }
                }

            }
        }
    }

    IEnumerator GoToEndGame()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Game Over");
        foreach (GameObject player in Players)
        {
            player.GetComponent<MovingAround>().movingTime = 2.0f;
        }

        ObjectToSpawn.GetComponent<MovingAround>().movingTime = 2.0f;
        currentPlayers = new List<GameObject>();
        keyIndexes = new List<int>();
        audio_source.Stop();
        Destroy(gameObject);
    }

    IEnumerator GoToRoundTransition()
    {
        yield return new WaitForSeconds(2.0f);
        SceneManager.LoadScene("Round Transition");
        //currentPlayers = new List<GameObject>();
        keyIndexes = new List<int>();
        generatingObstacles = false;
        roundTransition = true;
    }

    IEnumerator NextRound()
    {
        yield return new WaitForSeconds(5.0f);
        currentRound++;
        //int index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(currentRound);

        //Reset variables and increase current round #
        startNewRound = true;
        generatingObstacles = true;
        beatIndex = 0;
        currentNumSpawn = 0;
        roundMusic++;

        audio_source.clip = musics[roundMusic];
        audio_source.Play();

        currentPlayers = new List<GameObject>();

        //Increase movement speed
        foreach (GameObject player in Players)
        {
            player.GetComponent<MovingAround>().movingTime -= 0.5f;
        }

        ObjectToSpawn.GetComponent<MovingAround>().movingTime -= 0.5f;
    }
}