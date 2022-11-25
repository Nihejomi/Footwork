using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valikot : MonoBehaviour
{
    public main paalooppi;
    public GameObject aloitusmenu;
    public GameObject taukomenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    /// <summary>
    /// sulkee pelin
    /// </summary>
    public void suljeNappiPainettu() {
        Application.Quit();
    }
    public void nappiaPainetu() {
        paalooppi.startGame();
        aloitusmenu.SetActive(false);
        //this.gameObject.SetActive(false);
    }
    /// <summary>
    ///lopettaan tauon ja jatkaa pelia 
    /// </summary>
    public void jatka() {
        taukomenu.SetActive(false);
        paalooppi.peliPaalla = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel")) {
            //Debug.Log("esc painettu");
            if (paalooppi.peliPaalla)
            {
                taukomenu.SetActive(true);
                paalooppi.peliPaalla = false;
            }
            else if (taukomenu.activeSelf) {
                jatka();
            }
            } 
    }
}
