using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    [SerializeField] private Text text;
    public PlayerAnimSO player;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.gameObject.GetComponentInChildren<Text>();     
        
    }
    public void updateText(int partIndex)
    {
        text.text = player.getAllParts()[partIndex].name;
        
    }


}
