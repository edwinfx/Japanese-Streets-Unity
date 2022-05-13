using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup myUIGroup;
    [SerializeField] private bool fadeIn = false;
    [SerializeField] private bool fadeOut = false;
    [SerializeField] AudioSource openingSoundSource = default;
    [SerializeField] AudioClip closingSound = default;

    // private float stage = 0;

    public void ShowUI(){
        fadeIn = true;
    }

    public void HideUI(){
        fadeOut = true;
    }

    private void Update()
    {
        if(Input.GetButton("TestShowUI")){
            ShowUI();
        }

        if(Input.GetButton("TestHideUI")){
            HideUI();
        }

        if(fadeIn){
            if(myUIGroup.alpha < 1){
                myUIGroup.alpha += Time.deltaTime;
                if(myUIGroup.alpha >= 1){
                    fadeIn = false;
                }
            }
        }

        if(fadeOut){
            if(myUIGroup.alpha >= 0){
                myUIGroup.alpha -= Time.deltaTime;
                if(myUIGroup.alpha == 0){
                    fadeOut = false;
                }
            }
        }
    }

    // private void OnTriggerEnter(Collider other){
    //     if(other.CompareTag("Player"))
    //         if(stage == 0){
    //             if (other.gameObject.tag == "Trigger1")
    //                 Destroy(other.gameObject);
    //             Debug.Log("stage" + (stage + 1) );
    //             stage += 1;
    //             Stage1Screen();
    //         }
    // }

    // private void Stage1Screen(){
    //     ShowUI();
    //     openingSoundSource.PlayOneShot(openingSound);
    // }
}
