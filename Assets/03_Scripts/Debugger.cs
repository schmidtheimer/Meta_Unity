using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debugger : MonoBehaviour{
    
    public void PrintLog(string message) => Debug.Log(message);
    public void PrintErrorLog(string message) => Debug.Log(message);


}
