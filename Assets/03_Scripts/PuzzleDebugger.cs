using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.PoseDetection;

public class PuzzleDebugger : MonoBehaviour{
    
    public void DebugGesture(ShapeRecognizer shape){
        Debug.Log(shape.ShapeName);
    }

    //Unused fr now
    public void DebugTable(){

    }

}
