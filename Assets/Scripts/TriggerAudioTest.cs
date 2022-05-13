using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAudioTest : MonoBehaviour
{
    public AudioClip clipToPlay;

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player"))
            Vocals.instance.Say(clipToPlay);
    }
}
