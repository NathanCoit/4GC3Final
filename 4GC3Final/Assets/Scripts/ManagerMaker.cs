using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerMaker : MonoBehaviour
{
    //Makes persistant managers if they dont exsist.
    //Mostly for testing from not the title screen

    public GameObject SoundMan;
    public GameObject MenuMan;

    // Start is called before the first frame update
    void Awake()
    {
        if(GameObject.FindGameObjectWithTag("SoundManager") == null)
        {
            Instantiate(SoundMan);
        }
        if (GameObject.FindGameObjectWithTag("MenuManager") == null)
        {
            Instantiate(MenuMan);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
