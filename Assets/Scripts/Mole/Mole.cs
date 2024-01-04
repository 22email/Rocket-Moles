// Sounds from mixkit & pixaby
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Mole : Interactable
{
    public GameObject whackParticle;
    public ParticleSystem byeSpeechBubble;
    public UnityEvent onWhack;
    private MoleController moleController;

    [Header("Audio")]
    [SerializeField]
    private AudioSource hitSound;

    [SerializeField]
    private GameObject screamSourcesParent;

    private AudioSource[] screamSources;

    private bool screamingEnabled = true; // TODO --Put this in user settings

    // Start is called before the first frame update
    void Start()
    {
        onWhack.AddListener(
            GameObject
                .FindGameObjectWithTag("GameController")
                .GetComponent<ScoreController>()
                .IncreaseScore
        );
        moleController = GameObject
            .FindGameObjectWithTag("GameController")
            .GetComponent<MoleController>();
        screamSources = screamSourcesParent.GetComponents<AudioSource>();
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

        hitSound.pitch = Random.Range(0.8f, 1f);
        hitSound.Play();

        if (screamingEnabled)
            screamSources[Random.Range(0, screamSources.Length)].Play();

        Destroy(whackObj, 2f);

        moleController.GenRandMole();
        onWhack.Invoke();
    }

    IEnumerator change()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        moleController.GenRandMole();
        byeSpeechBubble.Play();
    }
}
