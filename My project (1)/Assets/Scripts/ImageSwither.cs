using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwither : MonoBehaviour
{
    [SerializeField] private Image[] bodyPartsImage;
    public PlayerAnimSO player;
    void Start()
    {
        bodyPartsImage = GetComponentsInChildren<Image>();
        updateFrame();


    }

    public void updateFrame()
    {
      
        foreach (var part in bodyPartsImage)
        {
            BodyPart_SO part_SO = player.getAllParts().Find(u => u.gettype().ToLower().Equals( part.gameObject.name.ToLower()));

            if (part_SO != null)
            {
                part.sprite = part_SO.getImage();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
}
