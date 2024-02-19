using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private Transform timerPanel; //pannello del timer
    private Image timerImage; //immagine che rappresenta la barra verde/arancione/rossa che cambia sulla base del timer
    private TextMeshProUGUI timerText; //timer text
    private int min, sec; //minuti e secondi del timer
    private float originalFillAmountTimerImage; //valore di riempimento originale dell'immagine (1)
    private float orangeTrigger, redTrigger; //ci dicono il momento in cui deve scattare il nuovo colore dell'immagine
    private bool orangeColorSetted, redColorSetted; //booleani per capire se è cambiato colore
    private int secondsToSubstract; //i secondi che andranno sotratti al timer

    public float timeSettingsInMinutes; //variabile public per far decidere la durata del timer

    /* 1) Risolvere bug del timer in time.deltatime --> FATTO
     * 2) Se finisce il tempo -> gameover e giocatore muore
     * 3) se prende energia, si aumenta un po' il tempo
     */

    private void Awake()
    {
        
        
    }
    void Start()
    {
       

        #region Canvas' childs
        //Ottengo tutti i figli della canvas
        Transform[] childs = transform.GetComponentsInChildren<Transform>();
        //Ottengo il transform del figlio che ha come tag Timer
        timerPanel = childs.Where(c => c.transform.tag == "Timer")?.FirstOrDefault();
        timerImage = timerPanel.GetComponent<Image>();
        //Ottengo il componente Text di TextMeshPro del figlio della Canvas che ha come tag TimerText
        timerText = childs.Where(c => c.transform.tag == "TimerText")?.FirstOrDefault().transform.GetComponent<TextMeshProUGUI>();
        #endregion

      
        #region Settings of timer bar color
       
        originalFillAmountTimerImage = timerImage.fillAmount;

        //La barra diventa arancione quando sono a 2/3 della sua lunghezza
        orangeTrigger = (originalFillAmountTimerImage / 3) * 2;
        //La barra diventa rossa quando sono all'ultima porzione della sua lunghezza
        redTrigger = (originalFillAmountTimerImage / 3);
        #endregion

        timeSettingsInMinutes = timeSettingsInMinutes * 60f; //trasformiamo il tempo in secondi
        secondsToSubstract = 0;

        InvokeRepeating(nameof(UpdateTimer), 0, 1);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void UpdateTimer()
    {
        /*Otteniamo i minuti: 
            * timeSettingsInMinutes ci dà il tempo in secondi (ci serve in secondi perché la variabile secondsToSubstract è in secondi)
            * timeSettingsInMinutes - secondsToSubstract ci dà il tempo rimanente in secondi 
            * ((timeSettingsInMinutes - secondsToSubstract) / 60f): dividendo per 60 riotteniamo i minuti
            */
        min = Mathf.FloorToInt((timeSettingsInMinutes - secondsToSubstract) / 60f);

        //Stessa cosa del'istruzione precedente ma utilizziamo %60 per capire i secondi rimanenti
        sec = Mathf.FloorToInt(((timeSettingsInMinutes) - secondsToSubstract) % 60);

        // Aggiorna il testo del countdown con il formato {0:00}:{1:00}
        timerText.text = string.Format("{0:00}:{1:00}", min, sec);


        UpdateTimerImage(); //Aggiorno anche la barra del timer

        //Aggiungiamo 1 secondo ogni volta così possiamo utilizzarlo per sottrarre il totale dei secondi dal tempo indicato dall'utente
        secondsToSubstract += 1;

        //Se il timer va a 0, stoppa il timer e chiama TimeEnd in PlayerController
        if (CheckDeathTimer())
        {
            CancelInvoke(nameof(UpdateTimer));
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().TimeEnd();
            Invoke("GameOver", 0.1f);
        }

    }


    void UpdateTimerImage()
    {

        /*
         * (time / (timeSettingsInMinutes): se time è il tempo passato dal momento dell'avvio del timer ad ora (in secondi), e timeSettingsInMinutes * 60f è il tempo indicato dall'inspector 
         * in secondi
         * con l'operazione (secondsToSubstract / (timeSettingsInMinutes * 60f) otteniamo il tempo rimanente
         * Aggiungendo 1 - andiamo a calcolare la percentuale rimanente
         */
        timerImage.fillAmount = 1 - (secondsToSubstract / (timeSettingsInMinutes)); // Calcola la percentuale rimanente;

        //In base al suo riempimento assegno il colore corretto
        AssignColor();


    }

    public void IncreaseTimer()
    {
        CancelInvoke(nameof(UpdateTimer));
        secondsToSubstract -=  (secondsToSubstract * 20) / 100;
        InvokeRepeating(nameof(UpdateTimer), 0, 1);
    }

    

    void AssignColor()
    {
        //Se ancora non è diventata arancione e il suo riempimento è compreso nei suoi 2/3
        if (orangeColorSetted == false && timerImage.fillAmount <= orangeTrigger && timerImage.fillAmount > redTrigger)
        {
            timerImage.color = new Color(1.0f, 0.64f, 0.0f); //arancione
            orangeColorSetted = true;
        }
        else if (timerImage.fillAmount <= redTrigger) //se il suo riempimento è nell'ultima porzione
        {
            timerImage.color = Color.red;
            redColorSetted = true;
        }
    }

    public bool CheckDeathTimer()
    {
        if (min == 0 && sec == 0)
            return true;
        else
            return false;
    }


    public void GameOver()
    {
        Debug.Log("GameOver");
    }


}
