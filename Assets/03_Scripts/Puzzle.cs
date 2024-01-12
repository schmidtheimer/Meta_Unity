using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour{
    public bool completed;
    public event System.Action puzzleCompleted;
    public UnityEngine.Events.UnityEvent puzzleCompletedUnityEvent;

    [TextArea(3,7)]
    public string _instructions;
    protected virtual void OnPuzzleCompleted(){
        puzzleCompleted?.Invoke();
    }
}
