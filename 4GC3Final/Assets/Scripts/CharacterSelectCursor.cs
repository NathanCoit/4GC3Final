using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectCursor : MonoBehaviour
{
    public int PlayerNumber;
    public Rigidbody2D rigidbody;
    public float MovementSpeed;

    private SoundManager SoundMan;
    private MenuManager MenuMan;

    private GameObject currentlyHovered;
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
        float yInput = Input.GetAxis("P" + PlayerNumber + "_Vertical");
        float xInput = Input.GetAxis("P" + PlayerNumber + "_Horizontal");
        rigidbody.AddForce(new Vector3(xInput * MovementSpeed, yInput * MovementSpeed, 0));
    }

    private void Update()
    {
        if (Input.GetButtonDown("P" + PlayerNumber + "_Fire1"))
        {
            if (currentlyHovered.tag == "roundsUp")
            {
                MenuMan.numRounds++;
                SoundMan.playClick();
                MenuMan.updateRoundsText();
            }
            else if (currentlyHovered.tag == "roundsDown")
            {
                MenuMan.numRounds--;
                SoundMan.playClick();
                MenuMan.updateRoundsText();
            }
            else if(currentlyHovered.tag == "beginButton")
            {
                MenuMan.StartGame();
            }
            else if (selected == "")
            {
                //Check to see if the other player already picked that character
                bool validSelection = false;

                if(PlayerNumber == 1)
                {
                    if (MenuMan.player2Character != currentlyHovered.tag)
                        validSelection = true;
                }
                else
                {
                    if (MenuMan.player1Character != currentlyHovered.tag)
                        validSelection = true;
                }

                if (validSelection)
                {
                    selected = currentlyHovered.tag;
                    SoundMan.playCharSelect();

                    if (PlayerNumber == 1)
                        MenuMan.player1Character = selected;
                    else
                        MenuMan.player2Character = selected;

                    //Show who selected who
                    if (PlayerNumber == 1)
                        currentlyHovered.GetComponent<Image>().color = Color.red;
                    else
                        currentlyHovered.GetComponent<Image>().color = Color.blue;

                    Debug.Log("You selected " + selected + "!");
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "p1Cursor" && collision.gameObject.tag != "p2Cursor")
        {
            currentlyHovered = collision.gameObject;
            SoundMan.playHover();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        currentlyHovered = null;
    }
}
