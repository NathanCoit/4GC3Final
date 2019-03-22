using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public string PlayerNumber;
    public GameObject OtherPlayer;

    public float MovementSpeed;
    public float BoostSpeed;
    public Rigidbody rigidbody;
    public float BoostCooldownTime;
    private int BoostTimeOut = 0; //Timeout time in 1/50's of a second
    // Start is called before the first frame update
    void Start()
    {
        BoostCooldownTime = BoostCooldownTime * 50;
    }

    // Fixed update is called 50 times a second
    void FixedUpdate()
    {
        if (BoostTimeOut <= 0)
        {
            float zInput = Input.GetAxis(PlayerNumber + "_Vertical");
            float xInput = Input.GetAxis(PlayerNumber + "_Horizontal");
            rigidbody.AddForce(new Vector3(xInput * MovementSpeed, 0, zInput * MovementSpeed));
            if(Input.GetButton(PlayerNumber + "_Fire1"))
            {
                // Boost
                rigidbody.velocity = new Vector3(0, 0, 0);
                rigidbody.AddForce( (OtherPlayer.transform.position - gameObject.transform.position).normalized * BoostSpeed);
                BoostTimeOut = (int)BoostCooldownTime;

            }
        }
        else
        {
            BoostTimeOut--;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("Yeet");
        }
        else if(col.gameObject.tag.Equals("Player"))
        {
            // colliding while boosting
            if(BoostTimeOut > 0)
            {
                rigidbody.velocity = new Vector3(0, 0, 0);
                // Decide whether to push other player if they are boosting
                if(col.gameObject.GetComponent<PlayerControls>().BoostTimeOut <= 0)
                {
                    col.rigidbody.AddForce(-rigidbody.velocity.normalized * BoostSpeed);
                }
            }
        }
    }
}
