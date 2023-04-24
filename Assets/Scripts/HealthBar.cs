using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * UI player health bar
 * Displays player health remaining using a heart tally
 */

public class HealthBar : MonoBehaviour
{
    public const int MAXIMUM_HEARTS = 50;
    // Variable values assigned in Inspector
    public GameObject heart_prefab;

    // Variables instantiated during runtime
    public Heart[] hearts = new Heart[MAXIMUM_HEARTS];
    
    [Range(1, 50)]
    public int max_hearts = 5;
    private int _current_health;
    
    void Start() {
        if(max_hearts > hearts.Length) {
            max_hearts = hearts.Length;
        }

        // Populates heart array up to index max_hearts
        for(int idx = 0; idx < max_hearts; idx++) {
            hearts[idx] = (
                Instantiate(heart_prefab, Vector3.zero, Quaternion.identity, transform)
                .GetComponent<Heart>()
                .Init(idx)
            );
        }
        _current_health = max_hearts;
    }
    
    public int UpdateHealth(int new_health){
        // Ensures player's health variable never exceeds the max number of hearts in use
        new_health = Mathf.Clamp(new_health, 0, max_hearts);

        // Sets heart sprites to match player health
        for (int i = new_health; i < _current_health; i++) {
            hearts[i].SetMode(HEART_MODE.inactive);
        }
        _current_health = new_health;

        return new_health;
    }
    public int SetMaxHearts(int new_max_hearts){
        new_max_hearts = Mathf.Clamp(new_max_hearts, 1, MAXIMUM_HEARTS);
        max_hearts = new_max_hearts;
        return max_hearts;
    }
}
