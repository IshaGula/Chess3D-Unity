using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource

            walkA,
            attackA,
            hurtA;

    bool playedOnce = false;

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad (gameObject);
        }
        else{
            DestroyImmediate (gameObject);
        }
    }

    public void PlayWalk(){
        if (!walkA.isPlaying && !playedOnce){
            walkA.Play();
            playedOnce = true;
        }
    }

    public void ResetPlayWalk(){
        playedOnce = false;
    }

    public void PlayAttack(){
        if (!attackA.isPlaying)
        {
            attackA.Play();
        }
    }

    public void PlayHurt(){
        if (!hurtA.isPlaying)
        {
            hurtA.Play();
        }
    }
}
