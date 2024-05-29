using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;

public class AnimationGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public string spriteSheetFolderPath = "Assets/SpriteFolder";
    public string animationFolder = "Assets/AnimationFolder";
    public string SOFolder = "Assets/AnimationFolder/BodyPartsSO";
    public GameObject Player;



    void Start()
    {
        string[] spriteSheetPaths = Directory.GetFiles(spriteSheetFolderPath, "*.png");
       // string[] SOFolders = Directory.GetDirectories(SOFolder, "*", SearchOption.TopDirectoryOnly);
      //  string shopFolder = null;
        if (Player != null)
        {
            // iau tranform-ul de la player
            Transform playerTransform = Player.transform;

            for (int i = 0; i < playerTransform.childCount; i++) // iterez prin toti copiii
            {
                Transform child = playerTransform.GetChild(i); // folosesc un geter ca sa imi dea tranformul de la copil
                  
                    string soNewFolder = Path.Combine(SOFolder, child.name);
               //d Debug.Log(soNewFolder);
                    if (!File.Exists(soNewFolder))
                    {
                        Directory.CreateDirectory(Path.Combine(soNewFolder, child.name)); // creez folderul
                    }
                }
        }
        string[] SOFolders = Directory.GetDirectories(SOFolder, "*", SearchOption.TopDirectoryOnly);


        foreach (string spriteSheetPath in spriteSheetPaths)
        {
            BodyPart_SO bodyPart = ScriptableObject.CreateInstance<BodyPart_SO>();
            int randomPrice = UnityEngine.Random.Range(100, 200);
           bodyPart.setPrice(randomPrice);

            string name = Path.GetFileNameWithoutExtension(spriteSheetPath).ToLower();
            bodyPart.setName(name);
            //Numele Sheet : Body_x , eu vreau doar body ca sa ma ajute mai incolo 
            int index = name.IndexOf('_');
            bool specialRemove = false;
            if (index != -1)
            {
                string type = name.Substring(0, index);
                bodyPart.setType(type);
               // Debug.Log(type);
                if (type == "glasses" || type == "beard") { specialRemove = true; }
            }
            else
            {
                Debug.Log("A aparut o eroare neasteptata! Numele sprite Sheet-ului nu este corespunzator");
            }
            bodyPart.setName(Path.GetFileNameWithoutExtension(spriteSheetPath));
            // Ia decupaturile si le stocheaza intr-un Array
            Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
            
            //creez un sprite list pentru a ma ajuta sa elimin anumite elemente
            List<Sprite> spriteList = new List<Sprite>(sprites);
            spriteList.Sort((a, b) => int.Parse(a.name.Split('_')[2]).CompareTo(int.Parse(b.name.Split('_')[2])));
            //elimin elementele de sus(e caracterul static nu ma ajuta la animatia propriu-zisa)
            List<string> directions;
           // Debug.Log(specialRemove);
            if (specialRemove)
            {
                bodyPart.setImage(spriteList[2]);
                spriteList.RemoveRange(0, 3);
                directions  = new List<string> { "right", "left", "down" };
               
            }
            else
            {
                directions = new List<string> { "right", "up", "left", "down" };
                bodyPart.setImage(spriteList[3]);
                spriteList.RemoveRange(0, 4);
            }
            //Debug.Log(directions.Count);


            // Ordinea animatiilor: dreapta, sus, stânga, jos

            List<string> animations = new List<string> { "idle", "walk"};

            foreach (string anim in animations)
            {
                string animationFolderPath = Path.Combine(animationFolder, anim); // fac path-ul pentru fiecare folder de animatie pentru a le avea organizate
                if (!Directory.Exists(animationFolderPath)) //  in cauzul in care directorul nu exista nu il fac
                {
                    Directory.CreateDirectory(animationFolderPath);
                }
                foreach (string direction in directions)
                {
                    string animationName = Path.Combine(animationFolderPath, Path.GetFileNameWithoutExtension(spriteSheetPath) + "_" + direction + "_" + anim + ".anim");
                    if (File.Exists(animationName))
                    {
                        continue;
                    }

                    AnimationClip animationClip = new AnimationClip();
                    animationClip.frameRate = 12; //Frame Rate-ul

                    EditorCurveBinding spriteBinding = new EditorCurveBinding();
                    spriteBinding.type = typeof(SpriteRenderer);
                    spriteBinding.path = "";
                    int animationLegth = 6;
                    spriteBinding.propertyName = "m_Sprite";

                    ObjectReferenceKeyframe[] spriteKeyframes = new ObjectReferenceKeyframe[animationLegth];

                    float frameTime = 1f / animationClip.frameRate; // Timpul pentru fiecare cadru
                  //  Debug.Log("Animatia " + animationName + "are sprite-urile: ");
                    for (int i = 0; i < animationLegth; i++)
                    {
                        spriteKeyframes[i] = new ObjectReferenceKeyframe();
                        spriteKeyframes[i].time = i * frameTime; // Setăm timpul în funcție de indexul cadrelor
                        spriteKeyframes[i].value = spriteList[i];

                     //   Debug.Log(spriteList[i].name);
                    }
                  //  Debug.Log("===================================== ");
                    AnimationUtility.SetObjectReferenceCurve(animationClip, spriteBinding, spriteKeyframes);

                    // Setează loopTime la true
                    AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(animationClip);
                    clipSettings.loopTime = true;
                    AnimationUtility.SetAnimationClipSettings(animationClip, clipSettings);

                    // Salvează animația în locul specificat
                    AssetDatabase.CreateAsset(animationClip, animationName);

                    bodyPart.addAnimation(animationClip);
                    // Șterge ce am folosit
                    spriteList.RemoveRange(0, animationLegth);
                }
            }
            string finalPath = Path.Combine(bodyPart.findFolderType(SOFolder), bodyPart.getName() + ".asset");
           
            if (!File.Exists(finalPath))
            {
               AssetDatabase.CreateAsset(bodyPart, finalPath);
            }


        }
        Debug.Log("Animations generated successfully!");
    }


   
}
