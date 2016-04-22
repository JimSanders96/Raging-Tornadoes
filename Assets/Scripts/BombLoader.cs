using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum BombType{Dojo, Training }

public class BombLoader : MonoBehaviour {
    public BombType Bomb;

    [SerializeField] 
    private ParticleSystem smoke;

    [SerializeField]
    [Range(0, 2)]
    private float loadTime = 2f;

    [SerializeField]
    [Range(0, 100)]
    private float bombCollisionForce = 10f;

    [SerializeField]
    private int dojoLevel = 1;

    [SerializeField]
    private int trainingLevel = 2;


    IEnumerator LoadSceneWithBomb()
    {
        smoke.gameObject.transform.position = transform.position;
        smoke.Play();
        yield return new WaitForSeconds(loadTime);
        switch (Bomb)
        {
            case BombType.Dojo:
                SceneManager.LoadScene(dojoLevel);
                break;
            case BombType.Training:
                SceneManager.LoadScene(trainingLevel);
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > bombCollisionForce)
        {
            Debug.Log("POEF");
            StartCoroutine(LoadSceneWithBomb());
        }
    }
}
