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
        //get the instance of game manager
        gm = GameManager.Instance;

        winner = gm.Players[0];
        //Set where the object display(spawn)
        GameObject clone = Instantiate(winner, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);

        //Just deactivate extra stuff
        clone.transform.localScale = new Vector3(100.0f, 100.0f, 0.0f);
        clone.GetComponent<Player>().tm.gameObject.SetActive(false);
        clone.GetComponent<Player>().canvas.gameObject.SetActive(false);
        clone.GetComponent<Player>().spriteObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        clone.GetComponent<Player>().enabled = false;
        clone.GetComponent<MovingAround>().enabled = false;

    }

}
