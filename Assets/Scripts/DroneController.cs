using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class DroneController : NetworkBehaviour
{
    [SyncVar]public string PlayerName;
    Rigidbody droneBody;
    public float upForce;
    public float forwardSpeed;
    public float maxSpeed;
    public float tiltAmount;
    public float tiltVelocity;

    float wantedYRotation;
    float currentYRotation;
    float rotationYVelocity;
    public float rotationSpeed = 2.5f;

    Vector3 velocityClamp;

    public float sideSpeed = 300.0f;
    float tiltAmountSide;
    float tiltAmountVelocity;

    public Transform boxHealthPanel;
    public Slider healthSlider;
    public Text abilityText;
    GameObject box;
    BoxData boxData;
    float boxHealth;
    float damageScale = 1;
    AbilityType ability = AbilityType.None;
    public override void OnStartAuthority()
    {
        droneBody = GetComponent<Rigidbody>();
        enabled = true;
        droneBody.useGravity = true;
        droneBody.isKinematic = false;
    }


    [ClientCallback]
    void FixedUpdate()
    {
        Movement();
        Forward();
        Rotation();
        ClampingSpeedValues();
        SideWays();
        droneBody.AddRelativeForce(Vector3.up * upForce);
        droneBody.rotation = Quaternion.Euler(new Vector3(tiltAmount, currentYRotation, tiltAmountSide));
        if(Input.GetKey(KeyCode.F))
            cmdDropOff();
        if(boxData != null)
        {
            healthSlider.value = boxData.boxHealth/100.0f;
        }
        if(Input.GetKeyDown(KeyCode.R))
        {
            if(ability != AbilityType.None)
            {
                StartCoroutine("UseAbility");
            }
        }
        abilityText.text = "Ability Type: " + ability.ToString();
    }

    IEnumerator UseAbility()
    {
        if(ability == AbilityType.Speed)
        {
            forwardSpeed = 200;
            yield return new WaitForSeconds(5.0f);
            forwardSpeed = 100;
        }
        if(ability == AbilityType.Invunrable)
        {
            damageScale = 0;
            yield return new WaitForSeconds(5.0f);
            damageScale = 1;
        }
        ability = AbilityType.None;
        yield return null;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if(boxData !=null)
        boxData.boxHealth -= other.relativeVelocity.magnitude * boxData.boxFragility* damageScale;
        if(other.collider.tag == "DeliveryPlatform" && boxData != null)
        {
            float value = boxData.boxHealth/100.0f;
            value = Mathf.Clamp(value,0, 1);
            CmdDestroyBox(boxData.gameObject, (int)value);
            boxData = null;
            boxHealthPanel.gameObject.SetActive(false);

        }
    }
    [Command] void CmdDestroyBox(GameObject box, int value)
    {
        GetComponent<Score>().points+= (int)value + 5;
        NetworkServer.Destroy(box);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "box")
        {
            cmdPickUp(other.gameObject);
            boxHealthPanel.gameObject.SetActive(true);
            boxData = other.GetComponent<BoxData>();
            box = other.gameObject;
            boxData.player = PlayerName;
        }
        if(other.tag == "Ability")
        {
            Abilities abilities = new Abilities();
            ability = abilities.GetAbility();
            DestroyAbility(other.gameObject);
        }
    }
    [Command] void DestroyAbility(GameObject ability)
    {
        NetworkServer.Destroy(ability);
    }

    [Command] void cmdPickUp(GameObject obj)
    {
        box = obj;
        box.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        boxData =  box.GetComponent<BoxData>();
        boxData.Parent = gameObject;
        boxData.player = PlayerName;     
    }

    [Command] void cmdDropOff()
    {
        box.GetComponent<BoxData>().Parent = null;
        box = null;
        //box.GetComponent<Rigidbody>().isKinematic = false;
    }


    [Client]
    void Movement()
    {

        if(Input.GetKey(KeyCode.Space))
        {
            upForce = 450;
            //if(hValue > 2.0f)
            {
                upForce = 500;
            }
        }
        else if(Input.GetKey(KeyCode.LeftControl))
        {
            upForce = -200;
        } else if(!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.LeftControl))
        {
            upForce = 98.1f;
        }
    }
    [Client]

    void Forward()
    {
        float value = Input.GetAxis("Vertical");
        if( value != 0)
        {
            droneBody.AddRelativeForce(Vector3.forward * value * forwardSpeed);
            tiltAmount = Mathf.SmoothDamp(tiltAmount, 20 * value, ref tiltVelocity, 0.1f);
        }
        else{
            tiltAmount = Mathf.SmoothDamp(tiltAmount, 0, ref tiltVelocity, 0.1f);

        }
    }
    [Client]

    void Rotation()
    {
        if(Input.GetKey(KeyCode.Q))
        {
            wantedYRotation -= rotationSpeed;
        }
        if(Input.GetKey(KeyCode.E))
        {
            wantedYRotation += rotationSpeed;
        }

        currentYRotation = Mathf.SmoothDamp(currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);
    }
    [Client]

    void ClampingSpeedValues()
    {
        float verticalValue = Mathf.Abs(Input.GetAxis("Vertical"));
        float horizontalValue = Mathf.Abs(Input.GetAxis("Horizontal"));
        if( verticalValue > 0.2f &&  horizontalValue > 0.2f)
        {
            droneBody.velocity = Vector3.ClampMagnitude(droneBody.velocity, Mathf.Lerp(droneBody.velocity.magnitude, maxSpeed, Time.fixedDeltaTime * 5f));
        }
        if(verticalValue > 0.2f && horizontalValue < 0.2f)
        {
            droneBody.velocity = Vector3.ClampMagnitude(droneBody.velocity, Mathf.Lerp(droneBody.velocity.magnitude, maxSpeed, Time.fixedDeltaTime * 5f));
        } 
        if(verticalValue < 0.2f && horizontalValue > 0.2f)
        {
            droneBody.velocity = Vector3.ClampMagnitude(droneBody.velocity, Mathf.Lerp(droneBody.velocity.magnitude, maxSpeed, Time.fixedDeltaTime * 5f));
        }
        if(verticalValue < 0.2f && horizontalValue < 0.2f)
        {
            droneBody.velocity = Vector3.SmoothDamp(droneBody.velocity, Vector3.zero, ref velocityClamp, 0.95f);
        }
    }
    [Client]

    void SideWays()
    {
        float value = Mathf.Abs(Input.GetAxis("Horizontal"));
        if(value > 0.2f)
        {
            droneBody.AddRelativeForce(Vector3.right * Input.GetAxis("Horizontal") * sideSpeed);
            tiltAmountSide = Mathf.SmoothDamp(tiltAmountSide, -20 * Input.GetAxis("Horizontal"), ref tiltVelocity,0.1f);
        }
        else
        {
            tiltAmountSide = Mathf.SmoothDamp(tiltAmountSide, 0, ref tiltAmountVelocity, 0.1f);
        }
    }
}
