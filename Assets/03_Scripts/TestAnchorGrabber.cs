using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class TestAnchorGrabber : MonoBehaviour {

    public OVRSceneManager sceneManager;

    public List<OVRSemanticClassification> semanticClassificationObjects = new List<OVRSemanticClassification>();
    [SerializeField] MeshRenderer[] allObjects;

    bool inMenu;
    private Text sliderText;

    public bool placeObjects;
    private bool placed = false;
    public List<GameObject> objectsToPlace = new List<GameObject>();
    public List<GameObject> tables = new List<GameObject>();
    public List<Transform> walls = new List<Transform>();
    public GameObject interactable;
    public event System.Action SortingCompleted;

    private void Awake() {
        sceneManager.SceneModelLoadedSuccessfully += StartSemanticCoroutine;
    }

    void Start() {
        if (!DebugUIBuilder.instance) return;

        InitializeDebugUI();
    }

    private void InitializeDebugUI() {
        DebugUIBuilder.instance.AddButton("Button Pressed", LogButtonPressed);
        DebugUIBuilder.instance.AddLabel("Label");
        var sliderPrefab = DebugUIBuilder.instance.AddSlider("Slider", 1.0f, 10.0f, SliderPressed, true);
        var textElementsInSlider = sliderPrefab.GetComponentsInChildren<Text>();
        Assert.AreEqual(textElementsInSlider.Length, 2,
            "Slider prefab format requires 2 text components (label + value)");
        sliderText = textElementsInSlider[1];
        Assert.IsNotNull(sliderText, "No text component on slider prefab");
        sliderText.text = sliderPrefab.GetComponentInChildren<Slider>().value.ToString();
        DebugUIBuilder.instance.AddDivider();
        DebugUIBuilder.instance.AddToggle("Toggle", TogglePressed);
        DebugUIBuilder.instance.AddRadio("Radio1", "group", delegate (Toggle t) { RadioPressed("Radio1", "group", t); });
        DebugUIBuilder.instance.AddRadio("Radio2", "group", delegate (Toggle t) { RadioPressed("Radio2", "group", t); });
        DebugUIBuilder.instance.AddLabel("Secondary Tab", 1);
        DebugUIBuilder.instance.AddDivider(1);
        DebugUIBuilder.instance.AddRadio("Side Radio 1", "group2",
            delegate (Toggle t) { RadioPressed("Side Radio 1", "group2", t); }, DebugUIBuilder.DEBUG_PANE_RIGHT);
        DebugUIBuilder.instance.AddRadio("Side Radio 2", "group2",
            delegate (Toggle t) { RadioPressed("Side Radio 2", "group2", t); }, DebugUIBuilder.DEBUG_PANE_RIGHT);

        DebugUIBuilder.instance.Show();
        inMenu = true;
    }

    public void TogglePressed(Toggle t)
    {
        Debug.Log("Toggle pressed. Is on? " + t.isOn);
    }

    public void RadioPressed(string radioLabel, string group, Toggle t)
    {
        Debug.Log("Radio value changed: " + radioLabel + ", from group " + group + ". New value: " + t.isOn);
    }

    public void SliderPressed(float f)
    {
        Debug.Log("Slider: " + f);
        sliderText.text = f.ToString();
    }

    void Update(){
        if (OVRInput.GetDown(OVRInput.Button.Two) || OVRInput.GetDown(OVRInput.Button.Start)){
            if (inMenu) DebugUIBuilder.instance.Hide();
            else DebugUIBuilder.instance.Show();
            inMenu = !inMenu;
        }
        if (!placed) return;
        
    }

    void LogButtonPressed()
    {
        Debug.Log("Button pressed");
    }

   
    [ContextMenu("SetSemantics")]
    public void StartSemanticCoroutine() { 
        StartCoroutine(GetSemanticClassification()); 
    }

    private IEnumerator GetSemanticClassification() {
        yield return new WaitForEndOfFrame();

        allObjects = FindObjectsOfType<MeshRenderer>();
        foreach (var obj in allObjects){
            if (obj.GetComponent<Collider>() == null){
                Debug.Log(obj);
                obj.gameObject.AddComponent<BoxCollider>();
            }
        }
        
        var ovrArray = FindObjectsOfType<OVRSemanticClassification>();

        for (int i = 0; i < ovrArray.Length; i++) {
            semanticClassificationObjects.Add(ovrArray[i]);
        }
        
        SortSemantics();
    }

    private void SortSemantics(){
        foreach(OVRSemanticClassification ovr in semanticClassificationObjects){
            if (ovr.Contains("TABLE")) tables.Add(ovr.gameObject);
            if (ovr.Contains("OTHER")) interactable = ovr.gameObject;
            if (ovr.Contains("WALL_FACE")) walls.Add(ovr.transform);
        }
        if (tables.Count > 0) PlaceItems();
        SortingCompleted?.Invoke();
    }

    private void PlaceItems(){
        if (!placeObjects)  return;
        if (interactable) interactable.transform.SetParent(null);

        for (int i = 0; i < objectsToPlace.Count; i++){
            objectsToPlace[i].transform.localPosition = tables[0].transform.position + new Vector3(-1 + (i * .5f), 0.1f, 0); //Places obj
        }
        placed = true;
        //if (tables.Count > 0) SetTablePuzzles();
    }

    [SerializeField] TableHandler tablePuzzle;
    private void SetTablePuzzles(){
        for(int tableIndex = 0; tableIndex < tables.Count; tableIndex++){
            tablePuzzle = tables[tableIndex].GetComponentInChildren<TableHandler>();
            tablePuzzle.Initialize();
        }
    }
}
