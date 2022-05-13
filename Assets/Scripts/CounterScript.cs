using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CounterScript : MonoBehaviour
{
    public GameObject PlayerController;
    public Text messageCanText;
    public Text messageMoneyText;
    public PlayerControllerScript player_script;

    private void Start()
    {
        player_script = PlayerController.GetComponent<PlayerControllerScript>();
    }
    private void Update() {
        messageCanText = transform.Find("CanNumber").GetComponent<Text>();
        messageMoneyText = transform.Find("MoneyNumber").GetComponent<Text>();

        if(player_script.currentCanAmount == 1){
            messageCanText.text = player_script.currentCanAmount + " Can";
        } else {
            messageCanText.text = player_script.currentCanAmount + " Cans";
        }

        messageMoneyText.text = player_script.currentMoneyAmount.ToString("#.##") + " Yen";
    }
}
