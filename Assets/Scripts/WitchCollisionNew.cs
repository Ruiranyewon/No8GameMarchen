// WitchCollision.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class WitchCollision : MonoBehaviour
{
    public GameObject keyPrefab;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOver");
        }
        else if (other.CompareTag("FirePot"))
        {
            
            var ai = GetComponent<WitchAINew>();
            if (ai != null)
                ai.StopMovement();

            StartCoroutine(HandleWitchDeath());
        }
    }

    IEnumerator HandleWitchDeath()
    {
        yield return new WaitForSeconds(0f);
        if (keyPrefab != null)
            Instantiate(keyPrefab, new Vector3(2f, -7.6f, 0f), Quaternion.identity);

        yield return new WaitForSeconds(0f);
        Destroy(gameObject);
    }
}
