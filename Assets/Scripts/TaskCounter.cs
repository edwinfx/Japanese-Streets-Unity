using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskCounter : MonoBehaviour
{
    public GameObject PlayerController;
    public Text messageText;
    public PlayerControllerScript player_script;

    private void Start()
    {
        player_script = PlayerController.GetComponent<PlayerControllerScript>();
    }
    private void Update() {
        messageText = transform.Find("TaskText").GetComponent<Text>();

        switch(player_script.stageTask){
            //startup
            case 0: messageText.text = "Most homeless people in Japan \nresort to working for their keep";break;
            //after 10 seconds
            case 1: messageText.text = "Try to find some cans on the \nfloor  to exchange for some cash \nuse the left mouse button";break;
            //after finding can
            case 2: messageText.text = "Great, you found a can \nkeep going, look in any crack";break;
            //after finding 5 cans
            case 3: messageText.text = "That should be enough to \nexchange for at least 750 yen. \nGo to the recycling machine";break;
            //after exchanging for cash
            case 4: messageText.text = "Now that you exchanged the \nthe cans, go grab a meal";break;
            //after getting a meal
            case 5: messageText.text = "This is how many homeless \nlive in Japan, but in reality it's \n x50 harder";break;
            default: messageText.text = "default text";break;
        }
    }
}
