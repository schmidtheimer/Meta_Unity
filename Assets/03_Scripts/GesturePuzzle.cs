using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction.PoseDetection;
using System;
using UnityEngine.UI;

public class GesturePuzzle : Puzzle {

    public static GesturePuzzle _gesturePuzzle;
    public TestAnchorGrabber anchorGrabber;
    public List<ShapeRecognizerActiveState> shapeSequence = new List<ShapeRecognizerActiveState>();
    public static event Action onPoseSuccess, onPoseFail;
    public GameObject gestureUIParent, successText;
    public TMPro.TextMeshProUGUI instructions;
    [SerializeField]private int currentIndex;
    [SerializeField] private ShapeRecognizer currentShape;
    [SerializeField] private bool recieveingInput = false;

    private void Awake(){
        anchorGrabber.SortingCompleted += Initialize;
        _gesturePuzzle = this;
        currentIndex = 0;
    }

    private void OnEnable(){
        onPoseSuccess += DelayInvoke;
    }

    private void Initialize(){
        ToggleSuccessText();
        SequencePuzzle();
        SetCanvasPosition();
    }

    private void DelayInvoke(){
        Invoke("SequencePuzzle", 1f);
    }

    public void SequencePuzzle(){
        if (currentIndex >= shapeSequence.Count){
            completed = true;
            base.OnPuzzleCompleted();
            return;
        }

        successText.SetActive(false);
        
        currentShape = shapeSequence[currentIndex].Shapes[0];
        foreach(ShapeRecognizerActiveState shapeState in shapeSequence){
            if (shapeState.Shapes[0] != currentShape)
                shapeState.gameObject.SetActive(false);
            else shapeState.gameObject.SetActive(true);
        }

        SetInstructionText(currentShape.ShapeName);

        
        currentIndex++;
        recieveingInput = true;
    }

    public void PosePuzzle(ShapeRecognizer poseShape){
        if (completed) return;
        if (!recieveingInput) return;
        if (QueryShape(poseShape)){
            onPoseSuccess?.Invoke();
            ToggleSuccessText();
            recieveingInput = false;
            return;
        }
        onPoseFail?.Invoke();
    }

    private bool QueryShape(ShapeRecognizer posee){
        return posee = currentShape;
    }

    private void OnDisable(){
        onPoseSuccess-= DelayInvoke;
    }

    //UI
    private void SetCanvasPosition(){
        if(anchorGrabber.tables.Count > 0){
            GameObject table = anchorGrabber.tables[0];
            gestureUIParent.transform.position = new Vector3(
                table.transform.position.x,
                table.transform.position.y + .1f, 
                transform.position.z-.5f);
        }
    }

    private void ToggleSuccessText(){
        successText.SetActive(!successText.activeInHierarchy);
    }

    private void SetInstructionText(string shapeName){
        instructions.text = "Show a " + shapeName + " pose";
    }

}
