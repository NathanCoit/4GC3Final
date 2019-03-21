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
    private int Timeout = 0; //Timeout time in 1/50's of a second
    // Start is called before the first frame update
    void Start()
    {
        BoostCooldownTime = BoostCooldownTime * 50;
    }

    // Fixed update is called 50 times a second
    void FixedUpdate()
    {
        if (Timeout <= 0)
        {
            float zInput = Input.GetAxis(PlayerNumber + "_Vertical");
            float xInput = Input.GetAxis(PlayerNumber + "_Horizontal");
            rigidbody.AddForce(new Vector3(xInput * MovementSpeed, 0, zInput * MovementSpeed));
            if(Input.GetButton(PlayerNumber + "_Fire1"))
            {
                // Boost
                rigidbody.AddForce( (OtherPlayer.transform.position - gameObject.transform.position) * BoostSpeed);
                Timeout = (int)BoostCooldownTime;

            }
        }
        else
        {
            Timeout--;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag.Equals("Ground"))
        {
            Debug.Log("Yeet");
        }
    }
}
