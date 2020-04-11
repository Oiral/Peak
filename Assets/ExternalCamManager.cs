using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExternalCamManager : MonoBehaviour
{
    public GameObject externalCamera;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            externalCamera.SetActive(!externalCamera.activeInHierarchy);
        }
    }
}
