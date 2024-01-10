using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TablePuzzle : Puzzle{

    public TestAnchorGrabber anchorGrabber;
    public Transform player;
    public Canvas tablePuzzleCanvas;
    public List<ButtonPuzzle> buttons;
    public GameObject successText, failText;
    public AudioSource au;

    [SerializeField] private List<bool> buttonStatus = new List<bool>();
    [SerializeField] private char[] combination;
    [SerializeField] private char[] inputCombination;
    private int passIndex;

    private void Awake(){
        anchorGrabber.SortingCompleted += Initialize;
    }

    private void Initialize(){
        buttons = new List<ButtonPuzzle>();
        var _but = FindObjectsOfType<ButtonPuzzle>();
        foreach(var v in _but){
            buttons.Add(v);
        }
        buttonStatus = new List<bool>();
        for (int buttonIndex = 0; buttonIndex < buttons.Count; buttonIndex++){
            buttonStatus.Add(false);
        }
        inputCombination = new char[buttons.Count];
        successText.SetActive(false);
        failText.SetActive(false);
        PlaceButtons();
    }

    public void ToggleButtonStatus(ButtonPuzzle buttonIndex){
        buttonStatus[buttonIndex.buttonIdentity] = !buttonStatus[buttonIndex.buttonIdentity];
        inputCombination[passIndex] = buttonIndex.buttonPassInput;
        passIndex++;
        if (passIndex < combination.Length) return;
        if (QueryPassCodeValidity()) {
            OnSuccessfulPassCode();
            return;
        }
        OnFailurePassCode();
    }

    private void PlaceButtons(){
        for(int buttonIndex = 0; buttonIndex < buttons.Count; buttonIndex++){
            GameObject buttonObj = buttons[buttonIndex].gameObject;
            Vector3 pos = anchorGrabber.walls[buttonIndex].position;
            Vector3 dir = (player.position - pos).normalized;
            buttonObj.transform.position = pos;
            Physics.Raycast(buttonObj.transform.position, -buttonObj.transform.up, out RaycastHit hit);
            buttonObj.transform.up = hit.normal;
        }
    }

    private void OnSuccessfulPassCode(){
        base.OnPuzzleCompleted();
        successText.SetActive(true);
    }

    private void OnFailurePassCode(){
        failText.SetActive(true);
        Invoke("Initialize", 1f);
    }

    private bool QueryPassCodeValidity(){
        for(int charIndex = 0; charIndex < combination.Length; charIndex++){
            if (inputCombination[charIndex] != combination[charIndex])
            return false;
        }
        return true;
    }

}
