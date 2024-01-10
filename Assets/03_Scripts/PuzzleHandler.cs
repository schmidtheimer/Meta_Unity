using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleHandler : MonoBehaviour{

    public List<Puzzle> puzzlesInScene = new List<Puzzle>();

    private void Start(){
        Initialize();
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

    private void TryEndPuzzles(){
        Debug.LogWarning("Trying to end");
        if (ValidPuzzleStatus()){
            if(puzzlesInScene[0].completed || puzzlesInScene[1].completed){
                EndGame();
                return;
            }
            //Do shit here. It ends here
        }
    }

 

    private void EndGame(){
        Application.Quit();
    }

    private bool ValidPuzzleStatus(){
        foreach(Puzzle p in puzzlesInScene){
            if (!p.completed) return false;
        }
        return true;
    }
}
