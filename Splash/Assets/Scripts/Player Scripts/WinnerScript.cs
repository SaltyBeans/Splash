using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class WinnerScript : NetworkBehaviour {
    
    public Text winText;
    public Text shutText;
    private bool loopBreak = true;
    public string winner;

    private bool gameOver = false;

    [SerializeField]
    private float countDownForShutdown=5;

    private float displayCountDown;
    private float timer;

	void Start () {
        winText.text = "";
        displayCountDown = countDownForShutdown;
	}
	
	void Update () {
        
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name.Contains("Score Text"))
            {
                if (obj.GetComponent<TextBehaviour>().LPScore >= 30 && gameOver == false)
                {
                    winText.text = obj.GetComponent<TextBehaviour>().PlayerName + " Wins!";
                    gameOver = true;
                    timer = Time.time;
                }
            }
        }
        if (gameOver == true)
        {
            if (loopBreak == true)
            {
                GetComponent<PlayerKill>().respawnTime = 100f;
                GetComponent<PlayerKill>().Die(false);
                GetComponentInChildren<Camera>().gameObject.SetActive(false);
                GetComponent<PlayerSetup>().sceneCamera.gameObject.SetActive(true);
                loopBreak = false;
            }

            GameObject.Find("GUIWinnerText").GetComponent<Text>().text = winText.text;
            
        }
    }

    void FixedUpdate()
    {
        if (gameOver == true)
        {
            displayCountDown -= 1.0f * Time.deltaTime;
            shutText.text = "Shutting down in " + displayCountDown.ToString("F0");
            if (Time.time - timer > countDownForShutdown)
            {
                Application.Quit();
            }
            GameObject.Find("GUIShutText").GetComponent<Text>().text = shutText.text;
        }
    }
}
