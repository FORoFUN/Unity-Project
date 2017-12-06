using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    private Button restartButton;
    // Use this for initialization
    void Start()
    {
        restartButton = GetComponent<Button>();
        restartButton.onClick.AddListener(RestartScene);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void RestartScene()
    {
        SceneManager.LoadScene("Minh's Scene");
    }
}
