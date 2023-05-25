using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Player heart meter on UI
/// </summary>
public class HealthBar : MonoBehaviour {
    /// <summary>
    /// Length of hearts array
    /// </summary>
    /// <remarks>
    /// Note that this is distinct from the actual maximum hearts of the player
    /// </remarks>
    private const int _MAXIMUM_HEARTS = 50;

    /// <summary>
    /// Heart prefab
    /// </summary>
    /// <remarks>
    /// Assigned in Inspector
    /// </remarks>
    public GameObject heartPrefab;

    // Variables instantiated during runtime
    private Heart[] _hearts = new Heart[_MAXIMUM_HEARTS];

    /// <summary>
    /// Maximum hearts a player can have
    /// </summary>
    [SerializeField]
    [Range(1, _MAXIMUM_HEARTS)]
    private int _max_hearts = 5;

    /// <summary>
    /// Current number of hearts the player has
    /// </summary>
    private int _current_health;

    /// <summary>
    /// Initializes HealthBar members
    /// </summary>
    void Start() {
        if(this._max_hearts > HealthBar._MAXIMUM_HEARTS) {
            this._max_hearts = HealthBar._MAXIMUM_HEARTS;
        }

        // Populates heart array up to index _max_hearts
        for(int idx = 0; idx < this._max_hearts; idx++) {
            this._hearts[idx] = (
                Instantiate(this.heartPrefab, Vector3.zero, Quaternion.identity, this.transform)
                .GetComponent<Heart>()
                .Init(idx)
            );
        }
        this._current_health = this._max_hearts;
    }

    /// <summary>
    /// Updates HealthBar after damage occurs
    /// </summary>
    /// <param name="new_health"> New player health after damage taken </param>
    /// <returns> new_health clamped between 0 and _max_hearts </returns>
    public int UpdateHealth(int new_health) {
        // Ensures player's health variable never exceeds the max number of hearts in use
        new_health = Mathf.Clamp(new_health, 0, this._max_hearts);

        // Sets heart sprites to match player health
        // TODO: Update this when we add way to regain health
        for(int idx = new_health; idx < this._current_health; idx++) {
            this._hearts[idx].SetMode(HEART_MODE.inactive);
        }
        this._current_health = new_health;

        return new_health;
    }

    // TODO: Instantiate new Hearts if _max_hearts increases
    /// <summary>
    /// Changes maximum hearts player can have at runtime
    /// </summary>
    /// <param name="new_max_hearts"> New maximum hearts a player can have </param>
    /// <returns> new_max_hearts clamped between 1 and _MAXIMUM_HEARTS </returns>
    public int SetMaxHearts(int new_max_hearts) {
        new_max_hearts = Mathf.Clamp(new_max_hearts, 1, HealthBar._MAXIMUM_HEARTS);
        this._max_hearts = new_max_hearts;
        return this._max_hearts;
    }

    // TODO: Factor our Heart instantiation into helper function
}
