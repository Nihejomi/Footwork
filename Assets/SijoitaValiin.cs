using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SijoitaValiin : MonoBehaviour
{
    public float suhde;
    public GameObject peliolio1;
    public GameObject peliolio2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        transform.position = peliolio1.transform.position + ( peliolio2.transform.position- peliolio1.transform.position) * suhde;

    }
}
