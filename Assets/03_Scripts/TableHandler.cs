using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TableHandler : MonoBehaviour{

    private float tableDepth = .035f; //In world coords
    
    public TextMeshProUGUI textCodeObject;
    public string text;

    public void Initialize(){
        textCodeObject.text = text;
        Vector3 originalTextPos = textCodeObject.transform.position;
        SetPos(textCodeObject.gameObject, new Vector3(originalTextPos.x, originalTextPos.y, originalTextPos.z + (-.05f)));
        Debug.Log(textCodeObject.transform.position);
    }


    public void SetPos(GameObject _obj, Vector3 pos){
        _obj.transform.localPosition = pos;
    }
}
