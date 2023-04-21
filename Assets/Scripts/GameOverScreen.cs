using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Game over screen scripting.
 * Provides methods for displaying a game over screen and restarting the game from said screen.
 */
public class GameOverScreen : MonoBehaviour
{
    public static float seconds_til_game_over = 3f;

    // Displays game over screen after a short time
    public IEnumerator Display() {
        yield return new WaitForSeconds(seconds_til_game_over);
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("SampleScene");
    }
}
