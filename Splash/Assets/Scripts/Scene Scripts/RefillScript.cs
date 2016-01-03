using UnityEngine;
using System.Collections;

public class RefillScript : MonoBehaviour
{
    [Range(0.1f, 10.0f)]
    [SerializeField]
    private float refillCD;
    private float bufferCD;


    // Use this for initialization
    void Start()
    {
        bufferCD = refillCD;
    }
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.GetComponent<PlayerShoot>().getAmmo() == 10)
            {
                other.GetComponent<PlayerShoot>().setRefillText("Ammo Full");
                refillCD = bufferCD;
            }
            else
            {
                other.GetComponent<PlayerShoot>().setRefillText("[E] Refill");

                if (Input.GetKey(KeyCode.E))
                {
                    refillCD -= 1.0f * Time.deltaTime;
                    other.GetComponent<PlayerShoot>().setRefillText("Refilling in " + refillCD.ToString("F0"));
                    if (refillCD < 0.5f)
                    {
                        other.GetComponent<PlayerShoot>().refilAmmo();
                    }
                }
                else { refillCD = bufferCD; }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerShoot>().setRefillText(null);
        }
        refillCD = bufferCD;
    }





}
