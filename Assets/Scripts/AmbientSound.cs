using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AmbientSound : MonoBehaviour
{
    public List<AudioTrackWithComponent> trackedAudio;

    public Transform trackedItem;

#if UNITY_EDITOR
    #region context menu items
    [ContextMenu("Create a new tracked Audio")]
    public void CreateTrackedAudio()
    {
        //Creating the game object
        GameObject newObject = new GameObject();
        newObject.name = "New Audio";
        newObject.transform.parent = transform;
        newObject.transform.localPosition = Vector3.zero;


        newObject.AddComponent<AudioSource>();
        AudioTrackWithComponent newAudio = new AudioTrackWithComponent();
        newAudio.source = newObject.GetComponent<AudioSource>();
    }

    [ContextMenu("Create sources from list")]
    public void CreateFromList()
    {
        foreach (AudioTrackWithComponent item in trackedAudio)
        {
            //If this is not connected to a source. Lets change that
            if (item.source == null)
            {
                //Creating the game object
                GameObject newObject = new GameObject();
                newObject.name = item.name;
                newObject.transform.parent = transform;
                newObject.transform.localPosition = Vector3.zero;
                newObject.AddComponent<AudioSource>();


                item.source = newObject.GetComponent<AudioSource>();

                if (item.clip != null)
                {
                    newObject.GetComponent<AudioSource>().clip = item.clip;
                }
            }
        }
    }
    #endregion
#endif

    //Lets update 1 audio on each frame
    //Will do this with a number incrementing every frame and if it gets too big for it, reset the number
    int trackingNumber;


    private void Update()
    {
        AudioTrackWithComponent audioToUpdate = trackedAudio[trackingNumber];

        float volume = audioToUpdate.volumeOverHeight.Evaluate(trackedItem.position.y);

        //Lets clamp the value to make sure noting happens wrongly
        volume = Mathf.Clamp(volume, 0, 1);

        audioToUpdate.source.volume = volume;


        trackingNumber += 1;
        if (trackingNumber >= trackedAudio.Count)
        {
            trackingNumber = 0;
        }
        
    }

}

[System.Serializable]
public class AudioTrackWithComponent
{
    public string name;
    public AudioSource source;
    public AudioClip clip;
    public AnimationCurve volumeOverHeight = AnimationCurve.Linear(0,0,100,1);
}