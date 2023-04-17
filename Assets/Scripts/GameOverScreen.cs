using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Display() {
        gameObject.SetActive(true);
    }

    public void Restart(){
        SceneManager.LoadScene("SampleScene");
    }
}
