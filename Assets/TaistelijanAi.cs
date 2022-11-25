using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaistelijanAi 
{
    public Vector3 pelaajanSijainti;
    public taistelija taistelija;
    float lyontiEtaisyys = 1.6f;

    public void Mieti() {
        Vector3 suuntapelaajaan = taistelija.peliolio.transform.position - pelaajanSijainti;
        float kulmaPelaajaan = Vector2.SignedAngle(new Vector2(0, -1), new Vector2(suuntapelaajaan.x, suuntapelaajaan.y));
        float omakulmaPelaajaan = Vector2.SignedAngle(taistelija.peliolio.transform.up*(-1f), new Vector2(suuntapelaajaan.x, suuntapelaajaan.y));
        float etaisyys = suuntapelaajaan.magnitude;
        
        if (kulmaPelaajaan < 0)
        {
            kulmaPelaajaan = 180 + (180 + kulmaPelaajaan);
        }
        //Debug.Log(omakulmaPelaajaan);
        taistelija.Haluttukulma = kulmaPelaajaan;
        if (Mathf.Abs(omakulmaPelaajaan) < 25 && etaisyys < lyontiEtaisyys)
        {
            taistelija.eteen = false;
            if (Random.value > 0.5f)
            {
                taistelija.oikeaLyonti = true;
                taistelija.vasenLyonti = false;
            }
            else {
                taistelija.oikeaLyonti = false;
                taistelija.vasenLyonti = true;
            }
        }
        else {
            if (Mathf.Abs(omakulmaPelaajaan) < 25 && etaisyys > lyontiEtaisyys) {
                taistelija.eteen = true;
            }
        }
        
    }
  

}
