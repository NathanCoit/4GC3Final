using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public enum ActionType
    {
        None,
        Boosting,
        Shoving,
        Jumping,
        BeingShoved,
        Dead,
        TimedOut
    }
    public string PlayerNumber;
    public GameObject OtherPlayer;
    public GameManager gameManager;

    public float MovementSpeed;
    public float BoostSpeed;
    public float JumpForce;
    public float ShoveForce;
    public float GravityScale;
    public Rigidbody rigidbody;
    public float BoostCooldownTime;
    public float JumpCooldownTime;

    private int ActionTimeout = 0; //Timeout time in 1/50's of a second
    public ActionType CurrentAction = ActionType.None;

    private SoundManager SoundMan;

    void Start()
    {
        BoostCooldownTime = BoostCooldownTime * 50;
        JumpCooldownTime = JumpCooldownTime * 50;

        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
    }

    // Fixed update is called 50 times a second
    void FixedUpdate()
    {
        Vector3 forceVector;
        if (CurrentAction == ActionType.None)
        {
            float zInput = Input.GetAxis(PlayerNumber + "_Vertical");
            float xInput = Input.GetAxis(PlayerNumber + "_Horizontal");
            rigidbody.AddForce(new Vector3(xInput * MovementSpeed, 0, zInput * MovementSpeed));
            if (Input.GetButton(PlayerNumber + "_Fire1"))
            {
                // Boost
                rigidbody.velocity = new Vector3(0, 0, 0);
                forceVector = (OtherPlayer.transform.position - gameObject.transform.position);
                forceVector.Scale(new Vector3(1, 0, 1));
                rigidbody.AddForce(forceVector.normalized * BoostSpeed);
                ActionTimeout = (int)BoostCooldownTime;
                CurrentAction = ActionType.Boosting;
            }
            else if (Input.GetButton(PlayerNumber + "_Fire2"))
            {
                // Jump
                rigidbody.velocity = new Vector3(0, 0, 0);
                rigidbody.angularVelocity = new Vector3(0, 0, 0);
                rigidbody.AddForce(new Vector3(0, JumpForce, 0));
                CurrentAction = ActionType.Jumping;
            }
            else if(Input.GetButton(PlayerNumber+"_Fire3"))
            {
                // Shove
                forceVector = (OtherPlayer.transform.position - gameObject.transform.position);
                forceVector.Scale(new Vector3(1, 0, 1));
                rigidbody.AddForce(forceVector.normalized * ShoveForce);
                CurrentAction = ActionType.Shoving;
                ActionTimeout = 25;
            }
        }
        else
        {
            ActionTimeout--;
            if(ActionTimeout == 0 && CurrentAction != ActionType.None && CurrentAction != ActionType.Dead)
            {
                if (CurrentAction == ActionType.Shoving)
                {
                    rigidbody.velocity = new Vector3(0, 0, 0);
                    rigidbody.angularVelocity = new Vector3(0, 0, 0);
                }
                CurrentAction = ActionType.None;
            }
        }

        // Apply gravity affect. Allows for variable gravity
        rigidbody.AddForce(Physics.gravity * rigidbody.mass * GravityScale);

        //Rolling sound
        if (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z) > 0.1f)
            SoundMan.playRolling(PlayerNumber, (Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z)) / 5.95f);
        else
            SoundMan.stopRolling(PlayerNumber);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Ground"))
        {
            CurrentAction = ActionType.Dead;
            gameManager.PlayerLost(PlayerNumber);
        }
        else if (col.gameObject.tag.Equals("Player"))
        {
            //Play sound
            SoundMan.playCollision((Mathf.Abs(rigidbody.velocity.x) + Mathf.Abs(rigidbody.velocity.z)) / 20.0f);
            Debug.Log("Boop");

            // colliding while boosting
            if (CurrentAction == ActionType.Boosting)
            {
                // Decide whether to push other player if they are boosting
                if (!(col.gameObject.GetComponent<PlayerControls>().CurrentAction == ActionType.Boosting))
                {
                    Debug.Log(PlayerNumber + " doin a boop");
                    col.gameObject.GetComponent<PlayerControls>().GetShoved(BoostCooldownTime, BoostSpeed, rigidbody.velocity.normalized);

                }
                rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
        else if(CurrentAction == ActionType.Jumping && col.gameObject.tag.Equals("Arena"))
        {
            CurrentAction = ActionType.TimedOut;
            ActionTimeout = (int)JumpCooldownTime;
        }
    }

    public void GetShoved(float ShoveTime, float ShoveForce, Vector3 ShoveVector)
    {
        rigidbody.velocity = new Vector3(0, 0, 0);
        rigidbody.angularVelocity = new Vector3(0, 0, 0);
        rigidbody.AddForce(ShoveVector * ShoveForce);
        CurrentAction = ActionType.BeingShoved;
        ActionTimeout = (int)ShoveTime;

        //Checking if hit is lethal (by seeing if they're withing 2 units of edge) yes this is ugly, deal with it
        if (lethalCheck())
        {
            SoundMan.playBwah();
            Camera.main.GetComponent<CombatCam>().lookAt(new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z));
            Camera.main.GetComponent<CombatCam>().setTarget(this.gameObject);
        }
    }

    //Function for figuring out if a dash will end the round
    private bool lethalCheck()
    {
        //Get the arena
        GameObject Arena = GameObject.FindGameObjectWithTag("Arena");

        //Trig to find how far we are from falling on the vector we've been pushed on. This took forever.
        float theta = 0.0f;
        float angleBetweenArenaAndHit = Mathf.Atan2(Arena.transform.position.z - transform.position.z, Arena.transform.position.x - transform.position.x);

        //Accounting for quadrant
        if (transform.position.x < 0)
        {
            if(OtherPlayer.transform.position.x > transform.position.x)
                theta = Mathf.PI - Mathf.Abs(angleBetweenArenaAndHit);
            else
                theta = Mathf.Abs(angleBetweenArenaAndHit);
        }
        else
        {
            if (OtherPlayer.transform.position.x < transform.position.x)
                theta = Mathf.Abs(angleBetweenArenaAndHit);
            else
                theta = Mathf.PI - Mathf.Abs(angleBetweenArenaAndHit);
        }

        //Two known sides
        float side1 = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), Arena.transform.position);
        float side2 = Arena.transform.localScale.x / 2;

        //Sin law to find first missing angle
        float gamma = Mathf.Asin((side1 * Mathf.Sin(theta)) / side2);

        //Sum of angles to find other missing angle
        float phi = Mathf.PI - theta - gamma;

        //Cosine law to find our distance
        float distanceToEdge = 1.0f * Mathf.Sqrt(Mathf.Pow(side1, 2) + Mathf.Pow(side2, 2) - (2 * side1 * side2 * Mathf.Cos(phi)));

        Debug.Log(distanceToEdge);

        if (distanceToEdge <= gameManager.lethalDistance)
            return true;

        return false;
    }
}
