using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateProps : MonoBehaviour
{
    // References to various game objects and UI elements.
    [SerializeField] private GameObject propObject; // The prefab object for props.
    [SerializeField] private List<Vector3> locations; // Possible spawn locations for props.
    [SerializeField] private GameObject electricalPannels; // Parent object for spawned props.
    [SerializeField] private Canvas wireTask; // UI element for wire tasks.
    [SerializeField] private WireTask wires; // Reference to the wire task logic.
    [SerializeField] private Canvas info; // Information canvas.
    [SerializeField] private Canvas playerHud; // Player HUD canvas.
    [SerializeField] private PlayerBluePrint playerStats; // Player stats for rewards management.
    public List<GameObject> wiresList; // List of all wire objects.
    private bool missionActive = false; // Flag to check if a mission is currently active.

    private int succes = 0; // Count of successful completions.
    private int propsLeft = 0; // Count of remaining props.

    // Initialize mission state.
    private void Start()
    {
        missionActive = false;
    }

    // Generate props at designated locations and set up their behavior.
    public void generateProps()
    {
        if (!missionActive)
        {
            missionActive = true;
            succes = 0;
            propsLeft = 0;
            wiresList = new List<GameObject>();
            foreach (var p in locations)
            {
                Vector3 realPosition = p;
                realPosition.x -= 3; // Adjust position slightly.

                GameObject newObject = Instantiate(propObject, realPosition, Quaternion.identity);
                ElectricianOpener openCanvas = newObject.AddComponent<ElectricianOpener>();

                // Set up the necessary references for the prop's behavior.
                openCanvas.hud = playerHud;
                openCanvas.buttoninfo = info;
                openCanvas.electrician = wireTask;
                openCanvas.wires = wires;
                openCanvas.props = this;
                newObject.transform.SetParent(electricalPannels.transform, false);
                newObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = false;
                wiresList.Add(newObject);
            }
        }
        else
        {
            Debug.Log("Mission already active");
        }
    }

    // Record a successful task completion and check if the mission is complete.
    public void addSucces()
    {
        succes++;
        propsLeft++;
        Debug.Log($"{propsLeft}, Success: {succes}");
        if (succes == wiresList.Count || propsLeft == wiresList.Count)
        {
            getPrize(succes);
            destroyProps();
        }
    }

    // Record a failed task and check if all props have been interacted with.
    public void addFail()
    {
        propsLeft++;
        Debug.Log($"{propsLeft}, Success: {succes}");
        if (propsLeft == wiresList.Count)
        {
            getPrize(succes);
            destroyProps();
        }
    }

    // Reward the player based on the number of successes and clean up.
    public void getPrize(int succes)
    {
        playerStats.giveMoney(succes * 200); // Give money based on success count.
        playerHud.GetComponent<HudManager>().updateMoney(); // Update the HUD.
        destroyProps();
    }

    // Clear all props from the scene and reset mission state.
    public void destroyProps()
    {
        foreach (var obj in wiresList)
        {
            Destroy(obj);
        }
        missionActive = false;
        wiresList.Clear();
    }

    // Check if a mission is currently active.
    public bool isMissionActive()
    {
        return missionActive;
    }
}
