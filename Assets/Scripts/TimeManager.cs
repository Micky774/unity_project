using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently unused script for timekeeping
/// </summary>
public class TimeManager : MonoBehaviour {
    /// <summary>
    /// Scaling factor to convert deltaTime to a time step
    /// </summary>
    public float time_scale = 10;
    /// <summary>
    /// Time since the frame before TimeManager became active
    /// </summary>
    public float clock = 0;

    // Update is called once per frame
    /// <summary>
    /// Updates clock on current frame
    /// </summary>
    protected void Update() {
        clock += Time.deltaTime;
    }

    /// <summary>
    /// Produces a time step based on time_scale and time between previous and current frame
    /// </summary>
    /// <returns> Produced time step </returns>
    public float GetTimeStep() {
        return time_scale * Time.deltaTime;
    }
}
