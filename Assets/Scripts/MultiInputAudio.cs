using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInputAudio : MonoBehaviour
{

    public MultiInputAudioMaster audioMaster;

    public float minHit;
    public float maxHit;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > minHit)
        {
            audioMaster.PlaySound(Mathf.Clamp((collision.relativeVelocity.magnitude - minHit)/ maxHit , 0, 1));
        }
    }
}
