using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
public class ScoreTextScript : NetworkBehaviour {

    public Text[] textArray;
    private int i = 1;

	void Start () {
        textArray[0].color = Color.red;
	}
	
	void Update () {
        textArray[0].text = GameObject.Find("Player " + GetComponent<NetworkIdentity>().netId + " Score Text").GetComponent<TextMesh>().text;

        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.name.Contains("Score Text") && !obj.name.Contains("" + GetComponent<NetworkIdentity>().netId))
            {
                textArray[i].text = obj.GetComponent<TextMesh>().text;
                i++;                
            }
            else
            {
                textArray[i].text = "";
            }
        }
        i = 1;
    }
}
