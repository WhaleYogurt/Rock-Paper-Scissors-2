using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private ObjectStarter[] objectStarters;
    private bool hasWinner = false;
    public float slowDownDuration = 1f;

    void Start()
    {
        objectStarters = FindObjectsOfType<ObjectStarter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasWinner)
        {
            string team = null;
            bool winner = true;
            foreach (ObjectStarter objectStarter in objectStarters)
            {
                if (team == null)
                {
                    team = objectStarter.team;
                }
                else if (team != objectStarter.team)
                {
                    winner = false;
                    break;
                }
            }
            if (winner)
            {
                // Debug.Log("Winner is " + team);
                hasWinner = true;
                StartCoroutine(SlowDownTime());
            }
        }
    }

    private IEnumerator SlowDownTime()
    {
        float originalTimeScale = Time.timeScale;
        float elapsedTime = 0f;

        while (Time.timeScale > 0)
        {
            elapsedTime += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Lerp(originalTimeScale, 0, elapsedTime / slowDownDuration);
            yield return null;
        }

        Time.timeScale = 0.01f;
        yield return new WaitForSeconds(0.02f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }
}
