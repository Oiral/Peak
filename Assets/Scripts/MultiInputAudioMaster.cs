using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiInputAudioMaster : MonoBehaviour
{
    public float delayTime;
    float delayTimer;

    AudioSource source;

    float volumeAverage;
    float averageCount;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            source.volume = volumeAverage / averageCount;

            if (delayTimer > 0)
            {
                averageCount = 0;
                volumeAverage = 0;
            }
        }
    }

    public void PlaySound(float volume)
    {
        volumeAverage += volume;
        averageCount += 1;
        if (delayTimer <= 0)
        {
            source.Play();
            delayTimer = delayTime;
        }
    }
}
