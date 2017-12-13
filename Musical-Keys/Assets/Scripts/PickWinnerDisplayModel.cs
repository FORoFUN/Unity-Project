using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickWinnerDisplayModel : MonoBehaviour
{

    private GameManager gm;
    public GameObject[] playerModels;
    private GameObject winner;

    // Use this for initialization
    void Start()
    {

        // Find winner from game
        for(int i = 0; i < gm.currentPlayers.Count; i++)
        {
            if (gm.currentPlayers[i].GetComponent<Player>().dead == false)
            {
                winner = gm.currentPlayers[i];
            }
        }
        // Deactivate the losers
        // (Automatic)

        // Display the winner
        winner.GetComponent<Player>().spriteObject.SetActive(true);
    }

}
