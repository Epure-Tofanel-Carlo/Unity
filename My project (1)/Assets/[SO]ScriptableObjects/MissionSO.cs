using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewMission", menuName = "Custom/Create Mission SO")]
[System.Serializable]
public class MissionSO : ScriptableObject
{
    [SerializeField] private string npcName;
    [SerializeField] private string missionDialog;
    [SerializeField] private Sprite npcFace;
    

    public string getNpcName() {  return npcName; }
    public string getMissionDialog() {  return missionDialog; }
    public Sprite getNpcFace() { return npcFace;}
}
