using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PuzzleHandler : MonoBehaviour{

    public List<Puzzle> puzzlesInScene = new List<Puzzle>();
    public Canvas instructionCanvas;
    public GameObject instructions;

    public OVRSceneManager _scceneManager;
    public OVRSceneAnchor replacementVolume, replacementPlane;

    private bool showUI;

    private void Start(){
        Initialize();
        Invoke("SetInstructionPosition", 1f);
    }

    private void Initialize(){
        foreach(Puzzle _p in puzzlesInScene){
            _p.puzzleCompleted += TryEndPuzzles;
        }
        //ActivatePuzzle(puzzlesInScene[0]);
    }

    public void ActivatePuzzle(Puzzle puzzleToActivate){
        foreach(Puzzle _p in puzzlesInScene){
            if (_p.gameObject.activeInHierarchy) _p.gameObject.SetActive(false);
        }

        if (puzzlesInScene.Contains(puzzleToActivate)) { 
            puzzleToActivate.gameObject.SetActive(true); 
        }
    }

    private void DeactivatePuzzle(Puzzle puzzleToDeactivate){
        puzzleToDeactivate.gameObject.SetActive(false);
    }

    private void TryEndPuzzles(){
        Debug.LogWarning("Trying to end");
        if (ValidPuzzleStatus()){
            _scceneManager.PlanePrefab = replacementPlane;
            _scceneManager.VolumePrefab = replacementVolume;
            EndGame();
            //Do shit here. It ends here
        }
        if (puzzlesInScene[0].completed) DeactivatePuzzle(puzzlesInScene[0]);

    }

    private void SetInstructionPosition(){
        var _wall = FindObjectOfType<TestAnchorGrabber>().walls;
        instructions.transform.position = _wall[1].position+ new Vector3(-1.75f,0,-.5f);
    }

    public void ShowInstructionText(){
        instructionCanvas.gameObject.SetActive(true);
        showUI = true;  
    }
    
    public void HideInstructionText(){
        instructionCanvas.gameObject.SetActive(false);
        showUI = false;
    }

    private void EndGame(){
        Application.Quit();
    }

    //This only returns true if all puzzles are completed
    private bool ValidPuzzleStatus(){
        foreach(Puzzle p in puzzlesInScene){
            if (!p.completed) return false;
        }
        return true;
    }
}
