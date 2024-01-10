using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour{
    public bool completed;
    public event System.Action puzzleCompleted;

    protected virtual void OnPuzzleCompleted(){
        puzzleCompleted?.Invoke();
    }
}
