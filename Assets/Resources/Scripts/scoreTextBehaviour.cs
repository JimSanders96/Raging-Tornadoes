using UnityEngine;
using System.Collections;

public class scoreTextBehaviour : MonoBehaviour {

    [Range(0, 50)]
    public float Speed;

    private TextMesh myTextMesh;

    void Awake()
    {
        myTextMesh = GetComponent<TextMesh>();
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position, Vector3.up);
    }

    void Update()
    {
        transform.Translate(0, Speed * Time.deltaTime, 0);
    }

    /// <summary>
    /// Method for setting he text of the mesh.
    /// </summary>
    /// <param name="text">The text that will be displayed.</param>
    public void SetText(string text)
    {
        myTextMesh.text = text;
    }

    /// <summary>
    /// Method for fading the text and destroying it.
    /// </summary>
    /// <param name="time">Time that it will be active</param>
    /// <returns>waits till the gameobject is removed.</returns>
    public IEnumerator SetDeath(float time)
    {
        float timer = 0;

        while( timer < time)
        {
            if(timer * 2 > time)
                myTextMesh.color = new Color(myTextMesh.color.r, myTextMesh.color.g, myTextMesh.color.b, myTextMesh.color.a - (1 / (time / (Time.deltaTime * 2))));

            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
