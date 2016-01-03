using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    Behaviour[] componentsToDisable;
    public GameObject thrusterSelf;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    //[SerializeField]
    //private Transform text3dlocation;

    [SyncVar]
    public int playerScore = 0;

    [SerializeField]
    public Camera sceneCamera; //Splash screen camera.
    
    AudioSource sceneAudio;

    [SerializeField]
    [SyncVar]
    private GameObject textObject;

    GameObject textHolderObject;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();    //If it's not the local player disable the components for the remote players.
            AssignRemoteLayer();    //Assign remote layers for the remote players.
        }
        else
        {
            sceneCamera = Camera.main;  //Set the splash screen main camera.
            if (sceneCamera != null)
            {
                int rnd = Random.Range(0, sceneCamera.GetComponent<EntryMusic>().entryMusic.Length);
                sceneAudio = GameObject.Find("MainCamera").GetComponent<AudioSource>();
                sceneAudio.clip = sceneAudio.GetComponent<EntryMusic>().entryMusic[rnd];
                sceneAudio.gameObject.SetActive(false);
                sceneCamera.gameObject.SetActive(false); //If there is no splash scwsreen camera, disable the gameObject.
            }
            thrusterSelf.SetActive(false);
        }

        RegisterPlayer();   //Give the local player a unique ID with NetworkID.

        GameObject textHolderObject = Instantiate(textObject) as GameObject;
        textHolderObject.name = gameObject.name + " Score Text";
        textHolderObject.tag = "scoreText";
        NetworkServer.Spawn(textHolderObject);  //Create the score text for the player.

        GetComponent<PlayerKill>().Setup();

    }

    void RegisterPlayer()
    {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;

    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()    //On exit change the camera to splash screen camera and delete the corresponding score text object.
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        Destroy(GameObject.Find(gameObject.name + " Score Text"));
    }
}
