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
        BeingShoved
    }
    public string PlayerNumber;
    public GameObject OtherPlayer;

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
    // Start is called before the first frame update
    void Start()
    {
        BoostCooldownTime = BoostCooldownTime * 50;
        JumpCooldownTime = JumpCooldownTime * 50;
    }

    // Fixed update is called 50 times a second
    void FixedUpdate()
    {
        if (CurrentAction == ActionType.None)
        {
            float zInput = Input.GetAxis(PlayerNumber + "_Vertical");
            float xInput = Input.GetAxis(PlayerNumber + "_Horizontal");
            rigidbody.AddForce(new Vector3(xInput * MovementSpeed, 0, zInput * MovementSpeed));
            if (Input.GetButton(PlayerNumber + "_Fire1"))
            {
                // Boost
                rigidbody.velocity = new Vector3(0, 0, 0);
                rigidbody.AddForce((OtherPlayer.transform.position - gameObject.transform.position).normalized * BoostSpeed);
                ActionTimeout = (int)BoostCooldownTime;
                CurrentAction = ActionType.Boosting;
            }
            if (Input.GetButton(PlayerNumber + "_Fire2"))
            {
                // Jump
                rigidbody.velocity = new Vector3(0, 0, 0);
                rigidbody.AddForce(new Vector3(0, JumpForce, 0));
                CurrentAction = ActionType.Jumping;
            }
            if(Input.GetButton(PlayerNumber+"_Fire3"))
            {
                // Shove
                rigidbody.AddForce((OtherPlayer.transform.position - gameObject.transform.position).normalized * ShoveForce);
                CurrentAction = ActionType.Shoving;
                ActionTimeout = 10;
            }
        }
        else
        {
            ActionTimeout--;
            if(CurrentAction == ActionType.Boosting && ActionTimeout == 0)
            {
                CurrentAction = ActionType.None;
            }
            else if(CurrentAction == ActionType.Shoving && ActionTimeout == 0)
            {
                CurrentAction = ActionType.None;
                rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }

        // Apply gravity affect. Allows for variable gravity
        rigidbody.AddForce(Physics.gravity * rigidbody.mass * GravityScale);
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("Yeet");
        }
        else if (col.gameObject.tag.Equals("Player"))
        {
            // colliding while boosting
            if (CurrentAction == ActionType.Boosting)
            {
                // Decide whether to push other player if they are boosting
                if (!(col.gameObject.GetComponent<PlayerControls>().CurrentAction == ActionType.Boosting))
                {
                    Debug.Log(PlayerNumber + " doin a boop");
                    col.rigidbody.velocity = new Vector3(0, 0, 0);
                    col.rigidbody.AddForce(rigidbody.velocity.normalized * BoostSpeed);
                }
                rigidbody.velocity = new Vector3(0, 0, 0);
            }
        }
        else if(CurrentAction == ActionType.Jumping && col.gameObject.tag.Equals("Arena"))
        {
            CurrentAction = ActionType.None;
            ActionTimeout = (int)JumpCooldownTime;
        }
    }
}
