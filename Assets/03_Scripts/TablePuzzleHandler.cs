using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TablePuzzleHandler : MonoBehaviour{

    private float tableDepth = .035f; //In world coords
    
    public TextMeshProUGUI textCodeObject;
    public string text;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void Initialize(){
        textCodeObject.text = text;
        Vector3 originalTextPos = textCodeObject.transform.position;
        SetPos(textCodeObject.gameObject, new Vector3(originalTextPos.x, originalTextPos.y+1f, originalTextPos.z + (-.05f)));
    }

    public void SetPos(GameObject _obj, Vector3 pos){
        _obj.transform.localPosition = pos;
    }
}
