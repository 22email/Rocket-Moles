using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Mole : Interactable
{
    [SerializeField] private GameObject whackParticle;
    [SerializeField] private ParticleSystem byeSpeechBubble;
    public UnityEvent onWhack;
    private MoleManager moleManager;

    // Start is called before the first frame update
    void Start()
    {
        onWhack.AddListener(GameObject.FindGameObjectWithTag("ScoreManager").GetComponent<ScoreManager>().updateScore);
        moleManager = GameObject.FindGameObjectWithTag("MoleManager").GetComponent<MoleManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
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

        moleManager.genRandMole();
        onWhack.Invoke();
    }

    IEnumerator change()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        moleManager.genRandMole();
        byeSpeechBubble.Play();
    }
}
