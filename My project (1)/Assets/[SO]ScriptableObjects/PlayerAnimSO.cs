using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerAnimSO", menuName = "Custom/Create Player Animation SO")]
[System.Serializable]
public class PlayerAnimSO : ScriptableObject
{
    //SerializeField pot sa il vad in unity, dar nu pot sa il modific(View-Only)

    [SerializeField] private List<BodyPart_SO> BodyParts;

   
    public void createNewBodyPart()
    {
        BodyParts.Add(new BodyPart_SO());
    }
    public int bodyCount()
    {
        return BodyParts.Count;
    }
    public void UpdatePart(int partIndex,BodyPart_SO part)
    {
        BodyParts[partIndex] = part;
    }
    public List<BodyPart_SO> getAllParts()
    {
       

        return BodyParts;
    }




}
