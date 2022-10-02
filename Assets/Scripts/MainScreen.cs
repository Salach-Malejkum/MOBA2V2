using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MonoBehaviour
{
    public Button playButt;
    public Button stopQueueButt;
    public GameObject timer;
    private int seconds = 0;

    private IEnumerator coroutine;

    void Start()
    {
        playButt.onClick.AddListener(StartQueue);
        stopQueueButt.onClick.AddListener(StopQueue);
    }

    void StartQueue()
    {
        timer.GetComponent<TextMeshProUGUI>().text = "00:00";
        playButt.gameObject.SetActive(false);
        timer.transform.parent.gameObject.SetActive(true);
        seconds = 0;
        coroutine = WaitForMachUp();
        //dodaj po³¹czenie z serverem

        StartCoroutine(coroutine);
    }

    void StopQueue()
    {
        playButt.gameObject.SetActive(true);
        timer.transform.parent.gameObject.SetActive(false);
        seconds = 0;
        //dodaj po³¹czenie z serverem

        StopCoroutine(coroutine);
    }

    private IEnumerator WaitForMachUp()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            seconds++;
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string ans = string.Format("{0:D2}:{1:D2}",
                t.Minutes,
                t.Seconds);
            timer.GetComponent<TextMeshProUGUI>().text = ans;
        }
    }
}
