/// \file enum and class for individual hearts on HealthBar

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Possible display states a Heart can have
/// </summary>
public enum HEART_MODE {
    /// <summary>
    /// Heart represents an active health point
    /// </summary>
    active,
    /// <summary>
    /// Heart represents a lost health point
    /// </summary>
    inactive,
    /// <summary>
    /// Heart should not be displayed
    /// </summary>
    /// <remarks>
    /// This is the case for Hearts beyond player max health
    /// </remarks>
    invisible,
}

/// <summary>
/// Heart representing a health point on HealthBar
/// </summary>
public class Heart : MonoBehaviour {
    /// <summary>
    /// Sprite of a full heart to represent an existing health point
    /// </summary>
    /// <remarks>
    /// Assigned in Inspector
    /// </remarks>
    [SerializeField]
    private Sprite _filledHeart;

    /// <summary>
    /// Sprite of an empty heart to represent a lost health point
    /// </summary>
    /// <remarks>
    /// Assigned in Inspector
    /// </remarks>
    [SerializeField]
    private Sprite _emptyHeart;

    /// <summary>
    /// Initializes a Heart
    /// </summary>
    /// <param name="idx"> Index of Heart in HealthBar's underlying array </param>
    /// <returns> Initialized Heart </returns>
    public Heart Init(int idx) {
        this.name = "Heart" + idx;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector3(50f + 100f * idx, 0f, 0f);
        return this;
    }

    /// <summary>
    /// Sets the display state of the Heart
    /// </summary>
    /// <param name="mode"> New display state </param>
    public void SetMode(HEART_MODE mode) {
        switch(mode) {
            case HEART_MODE.active:
                this.GetComponent<Image>().sprite = _filledHeart;
                break;
            case HEART_MODE.inactive:
                this.GetComponent<Image>().sprite = _emptyHeart;
                break;
            case HEART_MODE.invisible:
                this.GetComponent<CanvasRenderer>().SetAlpha(0f);
                return;
        }
        this.GetComponent<CanvasRenderer>().SetAlpha(1f);
    }
}