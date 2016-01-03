using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PickupScript : NetworkBehaviour
{

    public float pickUpTime;


    [SerializeField]
    private float pickupWaitSeconds;

    [SyncVar]
    public bool isOn;

    void Start()
    {
        pickUpTime = Time.time;
    }


    void FixedUpdate()
    {
        if (Time.time - pickUpTime > pickupWaitSeconds)
        {
            SetState(true);
            isOn = true;
        }
    }


    void Update()
    {
        gameObject.transform.Rotate(0.0f, 90.0f * Time.deltaTime, 0.0f);

        if (isOn == true)
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<Collider>().enabled = true;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerShoot>().getAmmo() < 10)
            {
                other.GetComponent<PlayerShoot>().addAmmo(5);
                isOn = false;
                SetState(false);
                setPickUpTime();
                pickUpTime = Time.time;
            }
        }
    }


    public void SetState(bool state)
    {
        isOn = state;
    }


    public void setPickUpTime()
    {
        pickUpTime = Time.time;
    }

}
