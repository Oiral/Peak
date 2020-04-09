using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMaterial : MonoBehaviour
{

public Material[] randomMaterials;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Awake ()
    {
       // gameObject.GetComponent<Renderer>().material = randomMaterials[Random.Range(0, randomMaterials.Length)];
    }
}
