using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectCursor : MonoBehaviour
{
    public int PlayerNumber;
    public Rigidbody2D rigidbody;
    public float MovementSpeed;

    private SoundManager SoundMan;
    private MenuManager MenuMan;

    private string currentlyHovered;
    public string selected;
    // Start is called before the first frame update
    void Start()
    {
        SoundMan = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>();
        MenuMan = GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (selected == "")
        {
            float yInput = Input.GetAxis("P" + PlayerNumber + "_Vertical");
            float xInput = Input.GetAxis("P" + PlayerNumber + "_Horizontal");
            rigidbody.AddForce(new Vector3(xInput * MovementSpeed, yInput * MovementSpeed, 0));
        }
    }

    private void Update()
    {
        if (Input.GetButton("P" + PlayerNumber + "_Fire1") && selected == "")
        {
            selected = currentlyHovered;
            SoundMan.playCharSelect();

            if (PlayerNumber == 1)
                MenuMan.player1Character = selected;
            else
                MenuMan.player2Character = selected;
            Debug.Log("You selected " + selected + "!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "p1Cursor" || collision.gameObject.tag != "p2Cursor")
        {
            currentlyHovered = collision.gameObject.tag;
            SoundMan.playHover();
        }
    }
}
