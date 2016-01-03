using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkIdentity))]
[RequireComponent(typeof(SphereCollider))]
public class EnergyBallCollision : NetworkBehaviour
{
    int colCount = 0;

    public GameObject shooter;

    AudioSource audSource;
    [SerializeField]
    AudioClip splashSound;
    [SerializeField]
    AudioClip BallCollisionSound;

    [SerializeField]
    int BallDamage;
    [SerializeField]
    GameObject explosionPrefab;
    void Start()
    {
        string _ID = "Ball " + GetComponent<NetworkIdentity>().netId;    // Get the network Identity.
        transform.name = _ID;                                            // and change the ball to a unique handle.
        audSource = GetComponent<AudioSource>();
    }

    //Enter this method whenever there is a collision
    void OnCollisionEnter(Collision collision)
    {

        AudioSource.PlayClipAtPoint(BallCollisionSound, collision.transform.position);

        if (collision.gameObject.tag == "Player" && collision.gameObject != shooter)   //If the object the ball collided with, a remotePlayer, enter.
        {
            CmdPlayerShot(collision.gameObject.GetComponent<Collider>().name);  //Send the name of the player to the server.
            shooter.GetComponent<PlayerSetup>().playerScore += colCount + 1; //Increment the score of the player.
            collision.gameObject.GetComponent<PlayerKill>().RpcTakeDamage(BallDamage);
            Destroy(gameObject);                                                //Destroy the ball.
        }

        if (colCount > 2)
            Destroy(gameObject);   //If the ball collided with anything, 3 times. Destroy the ball.

        colCount++;               //With each collision, increment the collision count.
    }

    void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(splashSound, gameObject.transform.position);
        GameObject explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
        NetworkServer.Spawn(explosion);
        explosion.GetComponent<ParticleSystem>().Play();
        Destroy(explosion, explosion.GetComponent<ParticleSystem>().duration);
    }


    [Command]
    public void CmdPlayerShot(string _ID)
    {
        Debug.Log("Hit!!! " + _ID + " has been hit!");
    }

    AudioSource PlayClipAt(AudioClip clip, Vector3 pos)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.transform.position = pos;
        AudioSource aSource = tempGO.AddComponent<AudioSource>();
        aSource.clip = clip;
        // set other aSource properties here, if desired
        aSource.Play();
        aSource.tag = "oneshotaudio"; //Adding a tag so we can find the object if not destroyed properly
        Destroy(tempGO, clip.length);
        return aSource;
    }

}
