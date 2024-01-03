using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Mole : Interactable
{
    public GameObject whackParticle;
    public ParticleSystem byeSpeechBubble;
    public UnityEvent onWhack;
    private MoleController moleController;
    [SerializeField]
    private AudioSource hitSound;

    // Start is called before the first frame update
    void Start()
    {
        onWhack.AddListener(GameObject.FindGameObjectWithTag("GameController").GetComponent<ScoreController>().IncreaseScore);
        moleController = GameObject.FindGameObjectWithTag("GameController").GetComponent<MoleController>();
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
