using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float time_scale = 10;
    public float clock = 0;
    
    // Update is called once per frame
    protected void Update()
    {
        clock += Time.deltaTime;
    }
    
    public float GetTimeStep(){
        return time_scale * Time.deltaTime;
    }
}
