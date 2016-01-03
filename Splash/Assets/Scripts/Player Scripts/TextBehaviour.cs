using UnityEngine;
using UnityEngine.Networking;

public class TextBehaviour : NetworkBehaviour
{

    Vector3 text3dlocation = new Vector3(-50f, 25f, 100f);

    GameObject localPlayer; //Player that the text is representing.
    
    int localPlayerScore;
    public int LPScore;
    public string PlayerName;
    // Use this for initialization
    void Start()
    {
        text3dlocation += (Vector3.up * 5);                  //Move the text right by 5 units.
        gameObject.GetComponent<TextMesh>().fontSize = 0;      //Determine the text font.
        GetComponent<MeshRenderer>().enabled = false;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Player"))
        {   //Foreach object in the hierarchy check if the text is for that object(Player).
            Debug.Log(obj.name+" Player found");
            Debug.Log(gameObject.name.Substring(0, 8));
            if (obj.name == gameObject.name.Substring(0, gameObject.name.Length-11))
            {
                localPlayer = obj;
                localPlayerScore = obj.GetComponent<PlayerSetup>().playerScore;
            }
            //else if (obj.name == gameObject.name.Substring(0, 9))
            //{
            //    localPlayer = obj;
            //    localPlayerScore = obj.GetComponent<PlayerSetup>().playerScore;
            //}
        }
        Vector3 offsetVector = new Vector3(localPlayer.GetComponent<NetworkIdentity>().netId.Value * 50, 0, 0); //Move the text according to the player's NetworkID.
        gameObject.transform.localPosition = (text3dlocation + offsetVector);

    }

    void Update()
    {
        localPlayerScore = localPlayer.GetComponent<PlayerSetup>().playerScore;   //Get the player's hit number.
        gameObject.GetComponent<TextMesh>().text = localPlayer.name + " Score: " + localPlayerScore; //Update the text with that player's hit number.

        PlayerName = localPlayer.name;
        LPScore = localPlayerScore;
        int max = -1;
        string highestPlayerName = "asdasd";


        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))    //Check if any of the other player's score is lower than the local player.
        {
            if (obj.name.Contains("Score Text"))
            {
                obj.GetComponent<TextMesh>().color = Color.white;
                obj.GetComponent<TextBehaviour>().localPlayer.GetComponentInChildren<Renderer>().material.color = Color.white;

                if (obj.GetComponent<TextBehaviour>().localPlayerScore > max)
                {
                    highestPlayerName = obj.name;
                    max = obj.GetComponent<TextBehaviour>().localPlayerScore;
                }
            }
        }

        GameObject.Find(highestPlayerName).GetComponent<TextMesh>().color = Color.blue;      //Color the lowest hit player blue.
        GameObject.Find(highestPlayerName).GetComponent<TextBehaviour>().localPlayer.GetComponentInChildren<Renderer>().material.color = Color.blue; //Color the lowest hit player's text blue.

    }

}