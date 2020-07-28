using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleGenerator : MonoBehaviour
{
    public GameObject toSpawnPrefab;

    public int amountToSpawn;

    public float circleSize;

    public Vector3 calculatePointOnCircle(float degree)
    {
        Vector3 positionOnCircle = Vector3.zero;

        positionOnCircle.x = Mathf.Sin(Mathf.Deg2Rad * degree);
        positionOnCircle.z = Mathf.Cos(Mathf.Deg2Rad * degree);

        positionOnCircle *= circleSize;

        return positionOnCircle;
    }

    private void OnDrawGizmos()
    {
        float division = 360f / (float)amountToSpawn;
        for (int i = 0; i < amountToSpawn; i++)
        {
            Gizmos.DrawSphere(calculatePointOnCircle(((float)i * (float)division)), 2f);
        }

        Gizmos.DrawWireSphere(Vector3.zero, circleSize);
    }

    public void Start()
    {
        float division = 360f / (float)amountToSpawn;
        for (int i = 0; i < amountToSpawn; i++)
        {
            Instantiate(toSpawnPrefab, calculatePointOnCircle(((float)i * (float)division)), Quaternion.identity, transform);
        }
    }
}
