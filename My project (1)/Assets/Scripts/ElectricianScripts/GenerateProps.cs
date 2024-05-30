using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GenerateProps : MonoBehaviour
{
    [SerializeField] private GameObject propObject;
    [SerializeField] private List<Vector3> locations;
    [SerializeField] private GameObject electricalPannels;
    [SerializeField] private Canvas wireTask;
    [SerializeField] private WireTask wires;
    [SerializeField] private Canvas info;
    [SerializeField] private Canvas playerHud;
    [SerializeField] private PlayerBluePrint playerStats;
    public List<GameObject> wiresList;
    private bool missionActive = false;

    private int succes = 0;
    private int propsLeft = 0;

    private void Start()
    {
        missionActive = false;
    }

    public void generateProps()
    {
        if (!missionActive)
        {
            missionActive = true;
            succes = 0; propsLeft = 0;
            wiresList = new List<GameObject>();
            foreach (var p in locations)
            {
                Vector3 realPosition = p;
                realPosition.x -= 3;
             
                GameObject newObject = Instantiate(propObject);
                newObject.transform.position = realPosition;
                ElectricianOpener openCanvas = newObject.AddComponent<ElectricianOpener>();

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
            Debug.Log("Misiune activa");
        }
        
    }


    public void addSucces()
    {
        succes++;
        propsLeft++;
        Debug.Log(propsLeft + ", Succes:" + succes);
        if (succes == wiresList.Count)
        {
            getPrize(succes);
        }else if(propsLeft == wiresList.Count)
        {
            getPrize(succes);
            destroyProps();
            
        }
    }
    public void addFail()
    {
        propsLeft++;
        Debug.Log(propsLeft + ", Succes:" + succes);
        if (propsLeft == wiresList.Count)
        {
            getPrize(succes);
            destroyProps();

        }

    }
    public void getPrize(int succes)
    {
        
        playerStats.giveMoney(succes * 200);
        playerHud.GetComponent<HudManager>().updateMoney();
        destroyProps();
    }

    public void destroyProps()
    {
        foreach (var obj in wiresList)
        {
            obj.SetActive(true);
            Destroy(obj);
        }
        missionActive = false;
        wiresList.Clear();
       
    }
    public bool isMissionActive()
    {
        return missionActive;
    }

   
}
