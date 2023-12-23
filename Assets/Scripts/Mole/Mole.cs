using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mole : Interactable
{
    public GameObject whackParticle;
    public ParticleSystem byeSpeechBubble;
    public UnityEvent onWhack;
    private MoleManager moleManager;

    // Start is called before the first frame update
    void Start()
    {
        onWhack.AddListener(GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().UpdateScore);
        moleManager = GameObject.FindGameObjectWithTag("MoleManager").GetComponent<MoleManager>();
    }

    void OnEnable()
    {
        StartCoroutine(change());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void interact()
    {
        GameObject whackObj = Instantiate(whackParticle, transform.position, Quaternion.identity);
        gameObject.SetActive(false);

        Destroy(whackObj, 2f);

        moleManager.GenRandMole();
        onWhack.Invoke();
    }

    IEnumerator change()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        moleManager.GenRandMole();
        byeSpeechBubble.Play();
    }
}
