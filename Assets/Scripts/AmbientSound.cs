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


    public bool showDebug;

    public int debugLines = 100;

    private void OnDrawGizmos()
    {
        //int gizmoTrackingNumber = 0;

        //Draw a box for each colour
        for (int i = -100; i < debugLines; i++)
        {
            for (int a = 0; a < trackedAudio.Count; a++)
            {
                Vector3 gizmoSize = new Vector3(1, 0, 1);

                gizmoSize = gizmoSize * trackedAudio[a].volumeOverHeight.Evaluate(i);

                Gizmos.DrawCube(Vector3.up * (i + (a-0.5f) / trackedAudio.Count), gizmoSize);

                Gizmos.color = Color.HSVToRGB((float)a / (float)trackedAudio.Count, 1f, 1f);
            }

            
            /*
            gizmoTrackingNumber += 1;
            if (gizmoTrackingNumber >= trackedAudio.Count)
            {
                gizmoTrackingNumber = 0;
            }
            */
        }
    }


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