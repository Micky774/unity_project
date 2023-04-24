using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Heart : MonoBehaviour
{
    public Sprite filled_heart;
    public Sprite empty_heart;
    public Heart Init(int idx) {
        name = "Heart" + idx;
        GetComponent<RectTransform>().anchoredPosition = new Vector3(50f + 100f*idx, 0f, 0f);
        return this;
    }
    public void SetMode(HEART_MODE mode) {
        switch(mode){
            case HEART_MODE.active:
                GetComponent<Image>().sprite = filled_heart;
                break;
            case HEART_MODE.inactive:
                GetComponent<Image>().sprite = empty_heart;
                break;
            case HEART_MODE.invisible:
                GetComponent<CanvasRenderer>().SetAlpha(0f);
                return;
        } 
        GetComponent<CanvasRenderer>().SetAlpha(1f);
    }
}

public enum HEART_MODE{
    active,
    inactive,
    invisible,
}
