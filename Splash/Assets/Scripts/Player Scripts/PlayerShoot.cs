using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{
    public const string PLAYER_TAG = "Player";
    public TextMesh ammoText;
    public TextMesh refillText;
    
    RaycastHit hit;

    [SerializeField]
    private int Ammo = 10;

    private Vector3 pos;
    private Vector3 campos;
    [SerializeField]
    private Camera cam;         //Player's camera.

    [SerializeField]
    private GameObject gunTip;  //Location of the gun's tip.

    [SyncVar]
    public GameObject energyBall;   //Energy ball prefab.
    void Start()
    {
        if (cam == null)    //If the referenced camera is null, disable this component.
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false;
        }
       
    }
    [Client]
    void Update()
    {

        if (!isLocalPlayer)
        {
            setRefillText(null);
            return;
        }

        pos = cam.transform.forward * 0.6f + gunTip.transform.position;
        campos = cam.transform.forward;
        ammoText.text = Ammo + "/10";

        if (Input.GetMouseButtonDown(0))
        {
            if (Ammo > 0)
            {
                CmdShoot(pos, campos);
                Ammo -= 1;
            }
        }

    }

    [Command]
    public void CmdShoot(Vector3 position, Vector3 direction)
    {
        GameObject enBall = Instantiate(energyBall, position, Quaternion.identity) as GameObject;
        //Vector3 movHorizontal = Input.GetAxisRaw("Horizontal") * cam.transform.right;   //Get the horizontal mouse input with the horizontal movement.
        enBall.GetComponent<Rigidbody>().velocity = (direction * 25f);                   //Set the energy ball spawn to guntip.
        NetworkServer.Spawn(enBall);        //Spawn the ball to the game scene.            
        enBall.GetComponent<EnergyBallCollision>().shooter = gameObject;
    }

    public int getAmmo()
    {
        return Ammo;
    }

    public void refilAmmo()
    {
        Ammo = 10;
    }

    public void setRefillText(string a)
    {
        refillText.text = a;
    }
    public void addAmmo(int amount)
    {
        if (Ammo + amount >= 10)
        {
            Ammo = 10;
        }
        else
        {
            Ammo += amount;
        }
    }
}











