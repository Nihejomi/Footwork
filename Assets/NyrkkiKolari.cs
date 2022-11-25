using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NyrkkiKolari : MonoBehaviour
{
    public Collider ToisenNyrkinKolari;
    void Awake()
    {
        Physics.IgnoreCollision(ToisenNyrkinKolari,gameObject.GetComponent<Collider>());
    }
        private void OnTriggerEnter(Collider other) {
       // Debug.Log("osuma");
    }
}
