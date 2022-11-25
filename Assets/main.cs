using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class main : MonoBehaviour
{
    public Sprite laatikkoVihrea;
    public float nopeuskerroin = 1.2f;
    public int nyrkkivahinko = 2;
    public int jalkavahinko = 6;
    public Camera kamera;
    public RectTransform pelaajanKunto;
    //pelaajan komennot
    public bool potkuValmius;
    public bool torju;
    public bool hiirenOikea;
    public bool hiirenVasen;
    public bool vasen;
    public bool oikea;
    public bool eteen;
    public bool taakse;
    public float haluttuKulma;
    public Vector3 hiirenSijaintiRuudulla;
    ///////////////////////////
    public Vector3 pelaajanSijaintiRuudulla; 
    public float peuler; //pelaajan kulma z-akselilla
    public GameObject pelaajanPeliolio;
    public taistelija pelaajanTaistelija;
    public List<taistelija> KaikkiTaistelijat = new List<taistelija>();
    public List<TaistelijanAi> vihollisenAIt = new List<TaistelijanAi>();
    public GameObject aloitusmenu;
    List<GameObject> ruumiit = new List<GameObject>();
    
    /////////////////////////
    public bool peliPaalla;
    int peliOhiViive=100;//kuinkapitkaan odotetaan kuolemasta alkuvalikoon siirtymista 
    int lisaaVihollisia=500;
    // Start is called before the first frame update

    void Start() {


    //Testiii = lisaaVihollinen;
    //    Testiii(new Vector2(0, 3));
    }
   public void startGame() {
        pelaajanPeliolio = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("vihollinen")) ;
        pelaajanTaistelija = new taistelija();
        pelaajanTaistelija.animaattori = pelaajanPeliolio.GetComponent<Animator>();
        pelaajanTaistelija.VasenJalkaEdessa = true;
        pelaajanTaistelija.peliolio = pelaajanPeliolio;
        pelaajanTaistelija.ruumiinosat = pelaajanPeliolio.GetComponentInChildren<KolariPalikkaLista>();
        KaikkiTaistelijat.Add(pelaajanTaistelija);
        lisaaVihollinen(new Vector2(0, 3));
        peliPaalla = true;
    }

  
    /// <summary>
    /// Lisaa vihollisen
    /// </summary>
    void lisaaVihollinen(Vector2 minne ) {
       
        TaistelijanAi vihollisenAI = new TaistelijanAi();
        GameObject vihollisenPeliolio = Instantiate(Resources.Load<GameObject>("vihollinen"), new Vector3(minne.x,minne.y), new Quaternion());
        taistelija vihollisenTaistelija = new taistelija();
        Canvas c = GameObject.FindObjectOfType<Canvas>();//ei ole kuin yksi canvas tassa
        GameObject g = new GameObject();
        SpriteRenderer vihollisenKunto = g.AddComponent<SpriteRenderer>(); //Instantiate(Resources.Load("vihrea")) as SpriteRenderer;
        vihollisenKunto.sprite = laatikkoVihrea;//Resources.Load("vihrea.png") as Sprite;
        vihollisenKunto.sortingOrder = 30;
        //vihollisenKunto.sprite = Instantiate(Resources.Load<Sprite>("vihrea"));
        vihollisenKunto.transform.SetParent(c.transform);
        vihollisenTaistelija.kuntoPalkki = vihollisenKunto;
        
        vihollisenAI.taistelija = vihollisenTaistelija;
        vihollisenTaistelija.animaattori = vihollisenPeliolio.GetComponentInChildren<Animator>();
        vihollisenTaistelija.peliolio = vihollisenPeliolio;
        vihollisenTaistelija.VasenJalkaEdessa = true;
        vihollisenTaistelija.ruumiinosat = vihollisenPeliolio.GetComponentInChildren<KolariPalikkaLista>();
        KaikkiTaistelijat.Add(vihollisenTaistelija);
        vihollisenAIt.Add(vihollisenAI);
    }

    void paalooppi() {
        if (lisaaVihollisia > 0) { lisaaVihollisia--; } else { lisaaVihollinen(new Vector3(-6,0)); lisaaVihollisia = 600; }
        float kuntoOsuus = (float)pelaajanTaistelija.kunto / (float)pelaajanTaistelija.maksimiKunto;
        if (kuntoOsuus < 0) { kuntoOsuus = 0; }
        pelaajanKunto.localScale = new Vector3(kuntoOsuus, 0.4f, 1);
       // pelaajanKunto.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (pelaajanKunto.anchorMax.x - pelaajanKunto.anchorMin.x) * 0.5f);
        pelaajanTaistelija.Haluttukulma = haluttuKulma;
        pelaajanTaistelija.oikea = oikea;
        pelaajanTaistelija.vasen = vasen;
        pelaajanTaistelija.eteen = eteen;
        pelaajanTaistelija.taakse = taakse;
        pelaajanTaistelija.potkuAsento = potkuValmius;
        pelaajanTaistelija.torju = torju;
        if (pelaajanTaistelija.potkuAsento && hiirenOikea && !pelaajanTaistelija.Oikeapotku && !pelaajanTaistelija.Vasenpotku && !pelaajanTaistelija.torju) {
            pelaajanTaistelija.Oikeapotku = true;
        }
        if (pelaajanTaistelija.potkuAsento && hiirenVasen && !pelaajanTaistelija.Oikeapotku && !pelaajanTaistelija.Vasenpotku && !pelaajanTaistelija.torju) {
            pelaajanTaistelija.Vasenpotku = true;
        }
        if (!pelaajanTaistelija.vasenLyonti && !pelaajanTaistelija.potkuAsento && !pelaajanTaistelija.oikeaLyonti) { //hiiren napautuksen lapimenon varmistamiseksi vasenLyonti nappi retsetataan kasitteleYlavartaloa-metodissa
            pelaajanTaistelija.vasenLyonti = hiirenVasen;
        }
        if (!pelaajanTaistelija.vasenLyonti && !pelaajanTaistelija.potkuAsento && !pelaajanTaistelija.oikeaLyonti)
        { //hiiren napautuksen lapimenon varmistamiseksi vasenLyonti nappi retsetataan kasitteleYlavartaloa-metodissa
            pelaajanTaistelija.oikeaLyonti = hiirenOikea;
        }
        hiirenOikea = false;
        hiirenVasen = false;
        foreach (TaistelijanAi ai in vihollisenAIt) {
            if (pelaajanPeliolio != null){
                ai.pelaajanSijainti = pelaajanPeliolio.transform.position;
                ai.Mieti();
            }
        }

            for (int i = KaikkiTaistelijat.Count-1; i >= 0; i--)
            {
                taistelija T = KaikkiTaistelijat[i];
                KasitteleTaistelia(T);
                if (T.kuolema >= 30)
                {
                if (T.kuntoPalkki != null) {T.kuntoPalkki.enabled = false;}
                ruumiit.Add(KaikkiTaistelijat[i].peliolio);    
                KaikkiTaistelijat.RemoveAt(i);
                }
            }
        }
    

    /// <summary>
    /// Kasitellaan yksittaisen taistelija toiminta
    /// </summary>
    /// <param name="T"></param>
    public void KasitteleTaistelia(taistelija T){
        if (T.kuolema > 0) {
            if (T.kuolema < 30)
            {
                T.kuolema++;
                T.kuolamaAnimaatio((1f / 30f) * T.kuolema);
            }
            else {
                KaikkiTaistelijat.Remove(T);
            }
            return;
        }
        if (T.taintunut > 0) {
            if (T.VasenJalkaEdessa)
            { T.animaattori.PlayInFixedTime("sattuuVasenEdessa", 0, 0.02f); }else { T.animaattori.PlayInFixedTime("sattuuOikeaEdessa", 0, 0.02f); }
            T.taintunut--;
            return;
        }
        if (T.horju > 0) {
            T.horju--;
            if (T.VasenJalkaEdessa) { T.animaattori.PlayInFixedTime("HorjuuVasenEdessa", 0, 0.02f); } else { T.animaattori.PlayInFixedTime("HorjuuOikeaEdessa", 0, 0.02f); }


            return;
        }
        KasitteleAlaruumis(T);
        kasitteleYlavartalo(T);
        kaanna(T.Haluttukulma, T.peliolio.transform.eulerAngles.z, 5f, T);
    }

    /// <summary>
    /// Tavoitekulma on kulma johon pyritaan kaantymaan. Nykyinen kulma on kulma jossa taistelija on tallahetkella. 0 osoittaa ylos, 180 alas.
    /// Maksimikaanto kertoo montako astetta kerralla kaannytaan kohti tavoitekulmaa.
    /// </summary>
    /// <param name="tavoiteKulma"></param>
    /// <param name="nykyinenKulma"></param>
    /// <param name="maksimiKaanto"></param>
    /// <param name="kaantyja"></param>
    void kaanna(float tavoiteKulma, float nykyinenKulma, float maksimiKaanto, taistelija kaantyja) {
        if (kaantyja.eiSaaKaantya > 0) {
            kaantyja.eiSaaKaantya--;
            return;
        }
        if (maksimiKaanto>= Mathf.Abs(tavoiteKulma - nykyinenKulma)){
            kaantyja.peliolio.transform.localEulerAngles = new Vector3(0, 0, tavoiteKulma);
        }
        else {

            if (((tavoiteKulma<nykyinenKulma)&&(nykyinenKulma-tavoiteKulma)<180) || (nykyinenKulma < tavoiteKulma && (tavoiteKulma-nykyinenKulma)>180)) {
                maksimiKaanto = -maksimiKaanto;
                
            }
            kaantyja.peliolio.transform.Rotate(0, 0, maksimiKaanto);
            if (kaantyja.VasenJalkaEdessa && maksimiKaanto < 0 && kaantyja.oikeaaAskeltaJaljella <=0 && kaantyja.askeltaJaljella <=0 && !kaantyja.eteen) {
                kaantyja.animaattori.SetBool("kaannosOikeeaanVasenEdessa", true);
               
            }
            if (kaantyja.VasenJalkaEdessa && maksimiKaanto > 0 && kaantyja.oikeaaAskeltaJaljella <= 0 && kaantyja.askeltaJaljella <= 0 && !kaantyja.eteen)
            {
                kaantyja.animaattori.SetBool("KaannosVasempaanVasenEdessa", true);
            }
            if (!kaantyja.VasenJalkaEdessa && maksimiKaanto > 0 && kaantyja.oikeaaAskeltaJaljella <= 0 && kaantyja.vasentaAskeltaJaljella <= 0 && !kaantyja.eteen)
            {
                kaantyja.animaattori.SetBool("KaannosOikeaanOikeaEdessa", true);
            }
            if (!kaantyja.VasenJalkaEdessa && maksimiKaanto < 0 && kaantyja.oikeaaAskeltaJaljella <= 0 && kaantyja.vasentaAskeltaJaljella <= 0 && !kaantyja.eteen)
            {
                kaantyja.animaattori.SetBool("KaannosVasempaanOikeaEdessa", true);
            }
        }
        if (Mathf.Abs(tavoiteKulma - nykyinenKulma) < 0.5f) {
            kaantyja.animaattori.SetBool("kaannosOikeeaanVasenEdessa", false);
            kaantyja.animaattori.SetBool("KaannosVasempaanVasenEdessa",false);
            kaantyja.animaattori.SetBool("KaannosOikeaanOikeaEdessa",false);
            kaantyja.animaattori.SetBool("KaannosVasempaanOikeaEdessa", false);
        }
    }
    void kasitteleOsuma(int vahinkko,int taintuminen,taistelija johonOsuttiin) {
        if (johonOsuttiin.horju > 0) { johonOsuttiin.kunto -= vahinkko; }//tuplavahinko jos horjuu
        johonOsuttiin.VasemmanKadenLyonti = 0; johonOsuttiin.OikeanKadenLyonti = 0;
        johonOsuttiin.vasenLyonti = false;
        johonOsuttiin.oikeaLyonti = false;
        johonOsuttiin.Vasenpotku = false;
        johonOsuttiin.Oikeapotku = false;
        johonOsuttiin.potkuAsento = false;
        johonOsuttiin.nollaaAnimaatiot();
        johonOsuttiin.taintunut = taintuminen;
        johonOsuttiin.ylavartaloVarattu = 10;
        johonOsuttiin.palautuminen = 10;
        johonOsuttiin.horju = 0;
        //johonOsuttiin.animaattori.PlayInFixedTime("sattuuVasenEdessa", 0, 0.2f);
        johonOsuttiin.kunto -= vahinkko;
    }
    /// <summary>
    /// Kasittelee Alaruumin Animaatiot ja liikkumisen
    /// </summary>
    /// <param name="T"></param>
    void KasitteleAlaruumis (taistelija T){

        if (T.palautuminen > 0) {
            T.palautuminen--;
            return;
        }
        if (T.OikeanJalanPotku > 0) {
            T.OikeanJalanPotku--;
            taistelija johonOsuttiin;
            if (T.hyokkaysEivielaOsunut && tarkistaTormausRuumiinOsiin(new List<taistelija> { T }, T.ruumiinosat.oikeaJalka.transform.position, out johonOsuttiin,T))
            {
                if (!T.hyokkaysTorjuttu) 
                {
                    kasitteleOsuma(jalkavahinko, 10, johonOsuttiin);
                }
                T.hyokkaysEivielaOsunut = false;
                if (johonOsuttiin.kunto <= 0) { johonOsuttiin.kuole(T.ruumiinosat.oikeaJalka.transform.position); }
            }
            if (T.OikeanJalanPotku == 0) {
                T.palautuminen = 30;
            }
            return;
        }
        if (T.OikeanJalanEtupotku > 0) {
            T.OikeanJalanEtupotku--;
            taistelija johonOsuttiin;
            if (T.hyokkaysEivielaOsunut && tarkistaTormausRuumiinOsiin(new List<taistelija> { T }, T.ruumiinosat.oikeaJalka.transform.position, out johonOsuttiin,T))
            {
                if (!T.hyokkaysTorjuttu) 
                {
                    //if (T.horju > 0) { johonOsuttiin.kunto -= jalkavahinko; }//tuplavahinko jos horjuu                    
                    //johonOsuttiin.kunto -= jalkavahinko;
                    kasitteleOsuma(jalkavahinko, 10, johonOsuttiin);
                }
                T.hyokkaysEivielaOsunut = false;
                if (johonOsuttiin.kunto <= 0) { johonOsuttiin.kuole(T.ruumiinosat.oikeaJalka.transform.position); }
            }
            if (T.OikeanJalanEtupotku == 0) {
                T.palautuminen = 30;
            }
            return;
        }
        if (T.VasemmanJalanEtupotku > 0) {
            T.VasemmanJalanEtupotku--;
            if (T.VasemmanJalanEtupotku> 1) {
                taistelija johonOsuttiin;
                if (T.hyokkaysEivielaOsunut && tarkistaTormausRuumiinOsiin(new List<taistelija>{T},T.ruumiinosat.vasenJalka.transform.position,out johonOsuttiin,T)){
                    if (!T.hyokkaysTorjuttu) {
                        //johonOsuttiin.kunto -= jalkavahinko; 
                        kasitteleOsuma(jalkavahinko, 10, johonOsuttiin);
                    }
                    T.hyokkaysEivielaOsunut = false;
                    if (johonOsuttiin.kunto <= 0) { johonOsuttiin.kuole(T.ruumiinosat.vasenJalka.transform.position); }
                }
            }
            if (T.VasemmanJalanEtupotku == 0) {
                T.palautuminen = 30;
            }
            return;
        }
        if (T.VasemmanJalanPotku > 0){
            T.VasemmanJalanPotku--;
            taistelija johonOsuttiin;
            if (T.hyokkaysEivielaOsunut && tarkistaTormausRuumiinOsiin(new List<taistelija> { T }, T.ruumiinosat.vasenJalka.transform.position, out johonOsuttiin,T)){
                if (!T.hyokkaysTorjuttu) {
                    //johonOsuttiin.kunto -= jalkavahinko;
                    kasitteleOsuma(jalkavahinko,10,johonOsuttiin);
                }
                T.hyokkaysEivielaOsunut = false;
                if (johonOsuttiin.kunto <= 0) { johonOsuttiin.kuole(T.ruumiinosat.vasenJalka.transform.position); }
            }
            if (T.VasemmanJalanPotku == 0){
                T.palautuminen = 30;
            }
            return;
        }
        if (T.potkuAsento && T.palautuminen <= 0 && T.ylavartaloVarattu <= 0 && T.Oikeapotku && !T.torju && T.askeltaJaljella<=0 && T.vasentaAskeltaJaljella <=0) {
            if (T.VasenJalkaEdessa){
                T.OikeanJalanPotku = 30;
                T.eiSaaKaantya = 30;
                T.hyokkaysEivielaOsunut = true;
                T.hyokkaysTorjuttu = false;
                T.animaattori.PlayInFixedTime("OikeanTakajalanPotku", 0, 0.8f);
                T.Oikeapotku = false;
            } else {
                T.OikeanJalanEtupotku = 30;
                T.eiSaaKaantya = 30;
                T.hyokkaysEivielaOsunut = true;
                T.hyokkaysTorjuttu = false;
                T.animaattori.PlayInFixedTime("oikeanEtujalanPotku", 0, 1f);
                T.Oikeapotku = false;
            }
          }
        if (T.potkuAsento && T.palautuminen <= 0 && T.ylavartaloVarattu <= 0 && T.Vasenpotku && !T.torju && T.askeltaJaljella<=0 && T.vasentaAskeltaJaljella<=0) {
            if (!T.VasenJalkaEdessa){
                T.VasemmanJalanPotku = 30;
                T.eiSaaKaantya = 30;
                T.hyokkaysEivielaOsunut = true;
                T.hyokkaysTorjuttu = false;
                T.animaattori.PlayInFixedTime("VasemmanTakajalanPotku", 0, 0.8f);
                T.Vasenpotku = false;
            } else{
                T.animaattori.PlayInFixedTime("vasemmanEtujalanPotku", 0, 1f);
                T.Vasenpotku = false;
                T.hyokkaysTorjuttu = false;
                T.hyokkaysEivielaOsunut = true;
                T.eiSaaKaantya = 30;
                T.VasemmanJalanEtupotku = 30; 
            }
        }
        if (T.askeltaJaljella >0){
            T.askeltaJaljella--;
            Vector3 uusiSijainti = T.peliolio.transform.localPosition + T.peliolio.transform.up * 0.08f * nopeuskerroin;
            List < taistelija > eiTarkasteta= new List<taistelija>{T};
            if (!tarkistaTormaysTaistelijoihin(uusiSijainti,0.8f,eiTarkasteta)) {
                T.peliolio.transform.localPosition = uusiSijainti;
            }
            if (T.askeltaJaljella == 0) {
                T.palautuminen = 20;
                if (T.VasenJalkaEdessa)
                {
                    T.VasenJalkaEdessa = false;
                }
                else {
                    T.VasenJalkaEdessa = true;
                }
            }
            return;
        }
        if (T.taakseAskeltaJaljella > 0)
        {
            T.taakseAskeltaJaljella--;
            Vector3 uusiSijainti = T.peliolio.transform.localPosition + T.peliolio.transform.up * -0.04f;
            List<taistelija> eiTarkasteta = new List<taistelija> { T };
            if (!tarkistaTormaysTaistelijoihin(uusiSijainti, 0.8f, eiTarkasteta)){ 
                T.peliolio.transform.localPosition = uusiSijainti;
            }
            if (T.taakseAskeltaJaljella == 0){
                T.palautuminen = 5;
            }
            return;
        }
        if (T.vasentaAskeltaJaljella > 0){
            T.vasentaAskeltaJaljella--;
            T.peliolio.transform.localPosition += T.peliolio.transform.right * -0.08f;
            if (T.vasentaAskeltaJaljella == 0)
            {
                T.palautuminen = 15;
            }
            return;
        }
        if (T.oikeaaAskeltaJaljella > 0){
            T.oikeaaAskeltaJaljella--;
            T.peliolio.transform.localPosition += T.peliolio.transform.right * 0.08f;
            if (T.oikeaaAskeltaJaljella == 0){
                T.palautuminen = 15;
            }
            return;
        }
        if (T.oikea && !T.vasen){
            if (T.VasenJalkaEdessa)
            {
                T.animaattori.SetBool("AskelOikeaanVasenEdessa", true);
            }
            else { 
            T.animaattori.SetBool("SivuaskelOikeaanOikeaEdella", true);
            }
            T.oikeaaAskeltaJaljella = 20;
            T.eiSaaKaantya = 10;
        }
        if (!T.oikea && T.vasen){
            if (T.VasenJalkaEdessa) { T.animaattori.SetBool("AskelVasemmalleVasenEdessa", true); } 
            else {    T.animaattori.SetBool("SivuaskelVasempaanOikeaEdella", true);}
            T.vasentaAskeltaJaljella = 20;
            T.eiSaaKaantya = 10;
        }
        if (T.eteen && !T.taakse && T.OikeanJalanPotku <=0 && T.OikeanJalanEtupotku <=0 && T.VasemmanJalanPotku <=0 && T.VasemmanJalanEtupotku <=0) {
            if (T.VasenJalkaEdessa){ T.animaattori.PlayInFixedTime("oikeaAstuu", 0, 0.4f);}
            else { T.animaattori.PlayInFixedTime("vasenAstuu", 0, 0.4f);}
            T.askeltaJaljella = 20;
            T.eiSaaKaantya = 17;
        }
        if (!T.eteen && T.taakse && T.OikeanJalanPotku <= 0 && T.OikeanJalanEtupotku <= 0 && T.VasemmanJalanPotku <= 0 && T.VasemmanJalanEtupotku <= 0){
            if (T.VasenJalkaEdessa) { T.animaattori.PlayInFixedTime("AstuTaakseVasenEdessa", 0, 0.2f); } 
            else { T.animaattori.PlayInFixedTime("AstuTaakseOikeaEdessa",0,0.2f); }
            T.taakseAskeltaJaljella = 25;
            T.eiSaaKaantya = 5;
        }
        
    }
    /// <summary>
    /// liikuttaaa tastelijaa T uuteen sijaintiin jos mahdollista. Jos siirto ei onnistu palauttaa false, muuten true
    /// </summary>
    /// <param name="T"></param>
    /// <param name="uusiSijainti"></param>
    /// <returns></returns>
    bool liikutaTaisteliaa(taistelija T, Vector3 uusiSijainti) {
        List<taistelija> eiTarkasteta = new List<taistelija> { T };
        if (!tarkistaTormaysTaistelijoihin(uusiSijainti, 0.8f, eiTarkasteta)){
            T.peliolio.transform.localPosition = uusiSijainti;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Kasittelee ylaruumiin Animaatiot;
    /// </summary>
    /// <param name="T"></param>
    void kasitteleYlavartalo(taistelija T) {
        if (T.taintunut>0) { Debug.Log("qeqweqwwqeew"); }
        if (T.sivuunOhjaus > 0) {
            T.sivuunOhjaus--;
            if (T.torju || T.ylavartaloVarattu > 0 || T.palautuminen > 0 ) {
                T.sivuunOhjaus = 0;
            }
        }

        if (T.torju && T.ylavartaloVarattu <= 0) {
            T.sivuunOhjaus = -1;
            T.animaattori.PlayInFixedTime("torjunta", 1, 0.02f);
            return;
        } else if (T.sivuunOhjaus<0){
            T.sivuunOhjaus = 10;
        }


        if (T.potkuAsento && T.ylavartaloVarattu <= 0){
            T.animaattori.SetBool("potkuAsento",true);
            T.animaattori.PlayInFixedTime("potkuAsento", 1);
        }
        else {
            T.animaattori.SetBool("potkuAsento", false);
        }
        if (T.VasemmanKadenLyonti > 0) {
            T.VasemmanKadenLyonti--;
            if ( T.VasemmanKadenLyonti>1) {
                taistelija johonOsutaan = new taistelija();
                if (tarkistaTormausRuumiinOsiin(new List<taistelija>() { T }, T.ruumiinosat.vasenNyrkki.transform.position, out johonOsutaan,T)&& T.hyokkaysEivielaOsunut) {
                    if (!T.hyokkaysTorjuttu) {
                        //if (T.horju > 0) { johonOsutaan.kunto -= nykkivahinko; }//tuplavahinko jos horjuu
                        //johonOsutaan.kunto -= nykkivahinko;
                        kasitteleOsuma(nyrkkivahinko,10,johonOsutaan);
                    }
                    T.hyokkaysEivielaOsunut = false;
                    if (johonOsutaan.kunto < 0) { johonOsutaan.kuole(T.ruumiinosat.vasenNyrkki.transform.position); }
                }
            }
            if (T.VasemmanKadenLyonti == 0) {
                T.ylavartaloVarattu = 10;//palautuminen
            }
        }
        if (T.OikeanKadenLyonti > 0){
            T.OikeanKadenLyonti--;
            if (T.OikeanKadenLyonti > 1)
            {
                taistelija johonOsutaan = new taistelija();
                if (tarkistaTormausRuumiinOsiin(new List<taistelija>() { T }, T.ruumiinosat.oikeaNyrkki.transform.position, out johonOsutaan,T) && T.hyokkaysEivielaOsunut)
                {
                    if (!T.hyokkaysTorjuttu) {
                        //if (T.horju > 0) { johonOsutaan.kunto -= nykkivahinko; }//tuplavahinko jos horjuu
                        //johonOsutaan.kunto -= nykkivahinko;
                        kasitteleOsuma(nyrkkivahinko, 10, johonOsutaan);
                    }
                    T.hyokkaysEivielaOsunut = false;
                    if (johonOsutaan.kunto < 0) { johonOsutaan.kuole(T.ruumiinosat.vasenNyrkki.transform.position); }
                }
            }
            if (T.OikeanKadenLyonti == 0){
                T.ylavartaloVarattu = 10;//palautuminen
            }
        }
        if (T.ylavartaloVarattu >= 0) {
            T.ylavartaloVarattu--;
            return;
        }
        if (T.oikeaLyonti) {
            T.animaattori.PlayInFixedTime("OikeaLyonti",1,0.1f);
            T.oikeaLyonti = false;
            T.hyokkaysEivielaOsunut = true;
            T.hyokkaysTorjuttu = false;
            T.ylavartaloVarattu = 20;
            T.OikeanKadenLyonti = 20;
        }
        
        if (T.vasenLyonti) {
            T.animaattori.PlayInFixedTime("VasenLyonti", 1,0.1f);
            T.hyokkaysEivielaOsunut=true;
            T.hyokkaysTorjuttu = false;
            //T.animaattori.SetBool("VasenLyonti", true);
            T.VasemmanKadenLyonti = 20;
            T.ylavartaloVarattu=20;
            T.vasenLyonti = false;
        }
    }
    /// <summary>
    /// Fixed timestep on 1/50 sekuntia
    /// </summary>
    private void FixedUpdate()
    {
        if (peliPaalla)
        {
            paalooppi();
            if (pelaajanTaistelija.kunto <= 0)
            {
                // Debug.Log("peliohi");
                peliohi();
            }
        }
    }
    public void peliohi() {
        if (peliOhiViive <= 0)
        {
            peliOhiViive = 100;

            for(int i= ruumiit.Count-1;i >= 0;i--)
            {
                GameObject.Destroy(ruumiit[i]);

            }
            for (int i = KaikkiTaistelijat.Count-1;i>=0;i--) {
                taistelija t = KaikkiTaistelijat[i];
                if (t.kuntoPalkki != null) {
                    GameObject.Destroy(t.kuntoPalkki.gameObject);
                }
                GameObject.Destroy(t.peliolio);
            }
            KaikkiTaistelijat = new List<taistelija>();
            vihollisenAIt = new List<TaistelijanAi>();
            pelaajanTaistelija = null;
            peliPaalla = false;
            aloitusmenu.SetActive(true);
        }
        else {
            Debug.Log("peliohi" + peliOhiViive);
            peliOhiViive--;
        }
    }
    /// <summary>
    /// Pelajan inputit otetaan vastaan  updattessa, joka on riippuvainen frameratesta
    /// </summary>
    void Update()
    {

        if (peliPaalla)
        {

            potkuValmius = Input.GetButton("PotkuValmius");
            torju = Input.GetButton("Torju");
            if (Input.GetMouseButtonDown(1)) { hiirenOikea = true; }
            if (Input.GetMouseButtonDown(0)) { hiirenVasen = true; }
            if (Input.GetAxis("Horizontal") > 0.1f) { oikea = true; }
            else { oikea = false; }
            if (Input.GetAxis("Horizontal") < -0.1f) { vasen = true; }
            else { vasen = false; }
            if (Input.GetAxis("Vertical") > 0.1f) { eteen = true; }
            else { eteen = false; }
            if (Input.GetAxis("Vertical") < -0.1f) { taakse = true; }
            else { taakse = false; }

            hiirenSijaintiRuudulla = Input.mousePosition;
            pelaajanSijaintiRuudulla = kamera.WorldToScreenPoint(pelaajanPeliolio.transform.position);
            Vector2 suunta = new Vector2(pelaajanSijaintiRuudulla.x - hiirenSijaintiRuudulla.x, pelaajanSijaintiRuudulla.y - hiirenSijaintiRuudulla.y);
            haluttuKulma = Vector2.SignedAngle(new Vector2(0, -1), suunta);
            if (haluttuKulma < 0)
            {
                haluttuKulma = 180 + (180 + haluttuKulma);
            }
            peuler = pelaajanPeliolio.transform.rotation.eulerAngles.z;
            foreach (taistelija t in KaikkiTaistelijat)
            {
                if (t.kuntoPalkki != null) //pelaajalla on erillainen systeemi kuntopalkin nayttamiseen
                {
                    float suhde = ((float)t.kunto) / t.maksimiKunto;
                    t.kuntoPalkki.transform.position = t.peliolio.transform.position;
                    t.kuntoPalkki.transform.position = new Vector3(t.kuntoPalkki.transform.position.x, t.kuntoPalkki.transform.position.y + 1);
                    t.kuntoPalkki.transform.localScale = new Vector2(suhde, 0.3f);
                }
            }
        }
    }

    public bool tarkistaTormaysTaistelijoihin(Vector3 piste, float sade, List<taistelija> eiHuomioda) {
        foreach (taistelija t in KaikkiTaistelijat) {
            if (!eiHuomioda.Contains(t)) {
                if (tarkistaTormaysTaisteliaan(t,piste,sade)) {
                    return true;
                }
             }
        }
        return false;
    }
    public bool tarkistaTormaysTaisteliaan(taistelija t, Vector3 piste, float sade) {
        Vector3 ero = t.peliolio.transform.position - piste;
        if (ero.magnitude <=sade){
            return true;
        }
        return false;
    }
    /// <summary>
    /// Tarkistaa osuttiinko johonkuhun ja samalla onnnistuttiinko hyokkays torjumaan tai sivuun ohjaamaan.
    /// </summary>
    /// <param name="taistelijatJoihinEiOsuta"></param>
    /// <param name="tarkasteltavaPiste"></param>
    public bool tarkistaTormausRuumiinOsiin(List<taistelija> taistelijatJoihinEiOsuta, Vector3 tarkasteltavaPiste, out taistelija johonOsuttiin,taistelija jokaHyokkaa) {
        foreach (taistelija t in KaikkiTaistelijat) {
            if (t.torju) {
                if ( Vector3.Magnitude(t.ruumiinosat.vasenNyrkki.transform.position - tarkasteltavaPiste) <= 0.5f){
                    jokaHyokkaa.hyokkaysTorjuttu = true;   
                    //Debug.Log("Torjunta!!!");
                }
            }
            if (t.sivuunOhjaus>0) {
                jokaHyokkaa.horju=20+jokaHyokkaa.VasemmanKadenLyonti+ jokaHyokkaa.OikeanKadenLyonti+jokaHyokkaa.VasemmanJalanEtupotku+jokaHyokkaa.VasemmanJalanPotku+jokaHyokkaa.OikeanJalanEtupotku+jokaHyokkaa.OikeanJalanPotku;
                jokaHyokkaa.hyokkaysTorjuttu = true;
                //Debug.Log("sivuunohjaus!!!");
            }
            if (Vector3.Magnitude( t.ruumiinosat.paa.transform.position-tarkasteltavaPiste) <= 0.6f && !taistelijatJoihinEiOsuta.Contains(t)) {
                johonOsuttiin = t;
                Debug.Log("osuma paahan");
                return true;
            }
            if (Vector3.Magnitude(t.ruumiinosat.vartalo.transform.position - tarkasteltavaPiste) <= 0.6f && !taistelijatJoihinEiOsuta.Contains(t)) {
                johonOsuttiin = t;
                Debug.Log("osuma vartaloon");
                return true;
            }
            if (Vector3.Magnitude(t.ruumiinosat.hartiat.transform.position - tarkasteltavaPiste) <= 0.6f && !taistelijatJoihinEiOsuta.Contains(t))
            {
                johonOsuttiin = t;
                Debug.Log("osuma hartioihin");
                return true;
            }
        }
        johonOsuttiin = null;
        return false;
    }

}


public class taistelija
{
    public SpriteRenderer kuntoPalkki;
    //kuolema//////////////////////
    public float Haluttukulma;
    public int kuolema;//0=elossa, 1-30 kuolema animaatio
    public int taintunut;
    Vector2 kuolemaSuunta;
    Vector3 alkuperainen;
    Vector3 alkuperainenvasenjalka;
    Vector3 alkuperainenoikeajalka;
    Vector3 alkuperainenoikeapolvi;
    Vector3 alkuperainenvasenpolvi;
    Vector3 alkuperainenhartiat;
    Vector3 alkuperainenPaa;
    Vector3 alkuperainenVasenKasi;
    Vector3 alkuperainenOikeaKasi;
    Transform[] paanosat;
    Vector3[] alkuperaisenPaansijainnit;
    ///////////////////////////
    public bool hyokkaysEivielaOsunut;
    public bool hyokkaysTorjuttu;
    public int kunto = 15;
    public int maksimiKunto = 15;
    public bool torju;
    public bool Oikeapotku;
    public bool Vasenpotku;
    public bool potkuAsento;
    public bool vasenLyonti;
    public bool oikeaLyonti;
    public int OikeanJalanEtupotku;
    public int OikeanJalanPotku;
    public int VasemmanJalanEtupotku;
    public int VasemmanJalanPotku;
    public int VasemmanKadenLyonti;
    public int OikeanKadenLyonti;

    public bool VasenJalkaEdessa;
    public bool vasen;
    public bool oikea;
    public bool eteen;
    public bool taakse;
    public int eiSaaKaantya;
    public int askeltaJaljella;
    public int ylavartaloVarattu;
    public int vasentaAskeltaJaljella;
    public int oikeaaAskeltaJaljella;
    public int taakseAskeltaJaljella;
    public int palautuminen;
    //komponentit
    public GameObject peliolio;
    public Animator animaattori;
    public KolariPalikkaLista ruumiinosat;
    public int sivuunOhjaus;
    public int horju;
    /// <summary>
    /// Kuolettaa hahmon.
    /// </summary>
    /// <param name="IskunSuunta"></param>
    public void kuole(Vector3 IskunSuunta)
    {
        kuolemaSuunta = (new Vector3(peliolio.transform.position.x - IskunSuunta.x, peliolio.transform.position.y - IskunSuunta.y)).normalized * 0.7f;
        kuolema = 1;
        animaattori.enabled = false;
        alkuperainen = peliolio.transform.position;
        alkuperainenoikeajalka = ruumiinosat.oikeaJalka.transform.position;
        alkuperainenvasenjalka = ruumiinosat.vasenJalka.transform.position;
        alkuperainenvasenpolvi = ruumiinosat.vasenPolvi.transform.position;
        alkuperainenoikeapolvi = ruumiinosat.oikeaPolvi.transform.position;
        alkuperainenhartiat = ruumiinosat.hartiat.transform.position;
        alkuperainenPaa = ruumiinosat.paa.transform.position;
        alkuperainenOikeaKasi = ruumiinosat.oikeaNyrkki.transform.position;
        alkuperainenVasenKasi = ruumiinosat.vasenNyrkki.transform.position;
        paanosat = ruumiinosat.paa.GetComponentsInChildren<Transform>();
        alkuperaisenPaansijainnit = new Vector3[paanosat.Length];
        for (int i = 0; i < paanosat.Length; i++) {
          
            alkuperaisenPaansijainnit[i]=paanosat[i].transform.position;
        }
    }

    public void nollaaAnimaatiot()
    {
        VasemmanKadenLyonti = 0;
        OikeanKadenLyonti = 0;
        VasemmanJalanEtupotku = 0;
        OikeanJalanEtupotku = 0;
        VasemmanJalanPotku = 0;
        OikeanJalanPotku = 0;
    }
    /// <summary>
    /// Kuolema animaatio. Sen vaihe valilta 0-1 ja se edellyttaa
    /// etta kuolema-metodi on ajettu.
    /// </summary>
    /// <param name="vaihe"></param>
    public void kuolamaAnimaatio(float vaihe){
        //kallistetaan paa kaatumis suuntaan
       for(int i=0;i< paanosat.Length;i++)
        {
            Transform go = paanosat[i];
            float korkeus = 0.3f-(go.localPosition.z);
            Vector3 paanosasenSuunta = new Vector3(kuolemaSuunta.x * vaihe * korkeus, kuolemaSuunta.y * vaihe * korkeus, 0)*0.5f;
            Vector3 eropaahan = alkuperaisenPaansijainnit[i]-alkuperainenPaa;
            go.position = new Vector3(paanosasenSuunta.x+ ruumiinosat.paa.transform.position.x , paanosasenSuunta.y + ruumiinosat.paa.transform.position.y);
            go.position += eropaahan;
        }
        ruumiinosat.vartalo.transform.position = alkuperainen + new Vector3(kuolemaSuunta.x * vaihe, kuolemaSuunta.y * vaihe, 1f * vaihe);
        ruumiinosat.vasenJalka.transform.position = alkuperainenvasenjalka;// - new Vector3(kuolemaSuunta.x * vaihe, kuolemaSuunta.y * vaihe, +1);
        ruumiinosat.oikeaJalka.transform.position = alkuperainenoikeajalka;// - new Vector3(kuolemaSuunta.x * vaihe, kuolemaSuunta.y * vaihe, +1);
        ruumiinosat.vasenPolvi.transform.position = alkuperainenvasenpolvi + new Vector3(kuolemaSuunta.x * vaihe * 0.5f, kuolemaSuunta.y * vaihe * 0.5f, +0.5f * vaihe);
        ruumiinosat.oikeaPolvi.transform.position = alkuperainenoikeapolvi + new Vector3(kuolemaSuunta.x * vaihe * 0.5f, kuolemaSuunta.y * vaihe * 0.5f, +0.5f * vaihe);
        ruumiinosat.hartiat.transform.position = alkuperainenhartiat + new Vector3(kuolemaSuunta.x * vaihe * 1.5f, kuolemaSuunta.y * vaihe * 1.5f, +1.5f * vaihe);
        ruumiinosat.paa.transform.position = alkuperainenPaa + new Vector3(kuolemaSuunta.x * vaihe * 1.8f, kuolemaSuunta.y * vaihe * 1.8f, +1.8f * vaihe);
        ruumiinosat.vasenNyrkki.transform.position = alkuperainenOikeaKasi + new Vector3(kuolemaSuunta.x * vaihe * 1.5f, kuolemaSuunta.y * vaihe * 1.5f, +1.5f * vaihe);


        if (vaihe > 0.99f) {
           SpriteRenderer[] spritet= peliolio.GetComponentsInChildren<SpriteRenderer>();
            
            foreach (SpriteRenderer sp in spritet) {
                sp.sortingOrder = -2;
            }
        }
    }
}


