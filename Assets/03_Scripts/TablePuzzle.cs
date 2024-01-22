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
    public TMPro.TextMeshProUGUI codeDisplay_;
    public GameObject displayObject;
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
        inputCombination = new char[4];
        for(int i = 0; i < inputCombination.Length; i++){
            inputCombination[i] = (char)61;
        }
    }

    public void ToggleButtonStatus(ButtonPuzzle buttonIndex){
        buttonStatus[buttonIndex.buttonIdentity] = !buttonStatus[buttonIndex.buttonIdentity];
        inputCombination[buttonIndex.buttonIdentity] = buttonIndex.buttonPassInput;
        UpdatePasscodeDisplay();
        for(int charIndex = 0; charIndex < combination.Length; charIndex++){
            if(inputCombination[charIndex] != combination[charIndex]){
                return;
            }
        }
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
            Physics.Raycast(buttonObj.transform.position, -buttonObj.transform.up, out RaycastHit _hit);
            buttonObj.transform.up = _hit.normal;
        }
        displayObject.transform.forward = -anchorGrabber.walls[1].forward;
        displayObject.transform.position = anchorGrabber.walls[1].position+(-codeDisplay_.transform.forward*1.1f)+(-codeDisplay_.transform.right*1.4f);
    }

    
    private string GetString(char[] c){
        char[] newArray= new char[c.Length]; int index =0;
        for(int i = 0; i < c.Length; i++){
            if(c[i] != (char)0) { newArray[index] = c[i]; }
        }
        string s = string.Concat(inputCombination);
        Debug.LogError(s);
        return s;
    }
    [ContextMenu("Update Display")]
    private void UpdatePasscodeDisplay(){

        codeDisplay_.text = GetString(inputCombination);
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
