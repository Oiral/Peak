using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool keepShowingGizmos;


    public Vector3 startingPosOffset;
    public Vector3 finishPosOffset;

    private Vector3 initalPos;

    public float speed;


    float positionAlongLine;

    public GameObject marker;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        if (Application.isPlaying)
        {

            Gizmos.DrawLine(initalPos + startingPosOffset, initalPos + finishPosOffset);
            Gizmos.DrawWireCube(initalPos + startingPosOffset, Vector3.one);
            Gizmos.DrawWireCube(initalPos + finishPosOffset, Vector3.one);
        }
        else
        {
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawLine(transform.position + startingPosOffset, transform.position + finishPosOffset);
            Gizmos.DrawWireCube(transform.position + startingPosOffset, Vector3.one);
            Gizmos.DrawWireCube(transform.position + finishPosOffset, Vector3.one);
        }
    }

    private void OnDrawGizmos()
    {
        if (keepShowingGizmos)
        {
            Gizmos.color = Color.green;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.lossyScale);
            Gizmos.DrawLine(transform.position + startingPosOffset, transform.position + finishPosOffset);
            Gizmos.DrawWireCube(transform.position + startingPosOffset, Vector3.one);
            Gizmos.DrawWireCube(transform.position + finishPosOffset, Vector3.one);
        }
    }

    private void Start()
    {
        initalPos = marker.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        positionAlongLine = (Mathf.Sin(Time.time * speed) + 1) / 2;

        marker.transform.position = Vector3.Lerp(initalPos + startingPosOffset, initalPos + finishPosOffset, positionAlongLine);
    }
}
