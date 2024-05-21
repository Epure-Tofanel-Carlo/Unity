using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.WSA;
using UnityEditor;
using UnityEngine.UI;

public class BodyPart_SO : ScriptableObject
{
    
    [SerializeField]private string type;
    [SerializeField] private List<AnimationClip> animations;
    [SerializeField]private int baseCost;
    [SerializeField]private Sprite image;
    [SerializeField]private bool purchased;

    public BodyPart_SO()
    {
        
        purchased = false;
        animations = new List<AnimationClip>();
    }

   
    public void addAnimation(AnimationClip anim)
    {
        animations.Add(anim);
    }
    public int animNumber()
    {
        return animations.Count;
    }
    public bool haveAnim(AnimationClip anim)
    {
        foreach (AnimationClip anim2 in animations)
        {
            if (anim2.name == this.name)
            {
                return true;
            }
        }
        return false;
    }
    

    public string findFolderType(string folderToSearch)
    {
        string[] folders = Directory.GetDirectories(folderToSearch, "*", SearchOption.TopDirectoryOnly);
        foreach (string folderPath in folders)
        {
            string folderName = Path.GetFileName(folderPath); // numele directorului din cale
           // Debug.Log("Type: " + type + " folder: " + folderName);
            if (folderName.ToLower().Equals(type))
            {

                return folderPath;
            }
        }
        return null;
    }


    public string getName()
    {
        return name;
    }
    public int getPrice()
    {
        return baseCost;
    }
    public void setPrice(int cost)
    {
        this.baseCost = cost;
    }

    public bool isPurchased()
    {
        return purchased;
    }
    public void buyed()
    {
        purchased = true;
    }
    public Sprite getImage()
    {
        return image;
    }
    public void setImage(Sprite sprite)
    {
        this.image = sprite;
    }
    public List<AnimationClip> getAnimations()
    {
        return animations;
    }
    public void setType(string newType)
    {
     type = newType;
    }
    public string gettype() {
        return type;
    }
    public void setName(string newName)
    {
        name = newName;
    }

}
