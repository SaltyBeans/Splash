using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : NetworkBehaviour
{


    public Image fuelBar;
    public GameObject Thruster;
    public Component thrusterParticle;

    private bool thrusterOn;

    private float distToGround;
    private float t;

    [SyncVar]
    Color col;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float lookSensitivity = 3f;

    private bool recharging = true;

    [SerializeField]
    private float thrusterForce = 1000f;

    [SerializeField]
    private float thrusterFuel = 100;

    private PlayerMotor motor;
    public GameObject HoverPoint;

    private Rigidbody body;

    private float hoverForce = 1000.0f;

    [SerializeField]
    private float crouchHeight = 0.5f;
    private float maxHoverHeight = 1.5f;
    private float hoverHeight;
    //public LayerMask ignoreMasks;

    private AudioSource AudSource;
    private bool lightUp;

    void FixedUpdate()
    {

        RaycastHit hit;

        if (Physics.Raycast(HoverPoint.transform.position, -Vector3.up, out hit, hoverHeight))
        {
            body.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), HoverPoint.transform.position);
        }

    }
    void Start()
    {
        lightUp = false;
        motor = GetComponent<PlayerMotor>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        body = GetComponent<Rigidbody>();
        hoverHeight = maxHoverHeight;

        AudSource = GetComponent<AudioSource>();
        //AudSource.clip = Resources.Load("torch_loop") as AudioClip;
        AudSource.clip = Resources.Load("torch_loop") as AudioClip;
        AudSource.loop = true;
        AudSource.Play();
        AudSource.volume = 0.06f;
        
    }

    [Client]
    void Update()
    {
        thrusterParticle.GetComponent<ParticleSystem>().startColor = col;
        AudSource.pitch = 1;

        if (!isLocalPlayer)
        {
            return;
        }

        fuelBar.fillAmount = thrusterFuel / 100;
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");
        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        if (Input.GetKey(KeyCode.LeftControl))
        {
            hoverHeight = crouchHeight;
            AudSource.pitch = 0.5f;
        }
        else
        {
            hoverHeight = maxHoverHeight;
        }

        if (!Grounded() && thrusterOn==false)
        {
            GetComponent<Rigidbody>().AddForce(-Vector3.up * 250);
        }

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized;

        //Apply movement
        motor.Move(_velocity, speed);

        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * lookSensitivity;

        //Apply rotation
        motor.Rotate(_rotation);

        //Calculate camera rotation as a 3D vector (turning around)
        float _xRot = Input.GetAxisRaw("Mouse Y");

        float _cameraRotationX = _xRot * lookSensitivity;

        //Apply camera rotation
        motor.RotateCamera(_cameraRotationX);

        // Calculate the thrusterforce based on player input
        Vector3 _thrusterForce = Vector3.zero;

        if (thrusterFuel > 0)
        {
            if (Input.GetButton("Jump"))
            {
                _thrusterForce = Vector3.up * thrusterForce;
                SetThrusterColor(Color.red);
                thrusterFuel -= 30 * Time.deltaTime;
                thrusterOn = true;
                AudSource.pitch = 1.6f;
            }
            else
            {
                thrusterOn = false;
                SetThrusterColor(Color.blue);
            }
        }
        else
        {
            thrusterOn = false;
            SetThrusterColor(Color.blue);
        }

        if (Input.GetKey(KeyCode.LeftShift) && thrusterFuel > 0)
        {
            recharging = false;
            speed = 10f;
            motor.Move(_velocity, speed);
            thrusterFuel -= 30 * Time.deltaTime;
            SetThrusterColor(Color.red);
            t = Time.time;
        }
        else
        {
            if (recharging == false)
            {
                if (Time.time - t > 2.0f)
                {
                    recharging=true;
                }
            }
        }        

        speed = 5f;

        // Apply the thruster force
        motor.ApplyThruster(_thrusterForce);

        if (thrusterFuel < 100.0f && Grounded() == true && recharging == true)
        {
            thrusterFuel += 25 * Time.deltaTime;
        }
    }

    public bool Grounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 1.6f);
    }
    public void SetThrusterColor(Color c)
    {
        col = c;
    }

    [Command]
    public void CmdResetPickup(GameObject _pickup)
    {
        _pickup.GetComponent<PickupScript>().isOn = false;
        _pickup.GetComponent<PickupScript>().pickUpTime = Time.time;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ammoPickup")
        {
            other.GetComponent<PickupScript>().isOn = false;
            other.GetComponent<PickupScript>().pickUpTime = Time.time;
            CmdResetPickup(other.gameObject);
        }


    }
}
