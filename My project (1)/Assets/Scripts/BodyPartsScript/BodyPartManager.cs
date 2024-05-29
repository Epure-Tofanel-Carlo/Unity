using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;

public class BodyPartManager : MonoBehaviour
{
    [SerializeField] private PlayerAnimSO PlayerAnimations;
    [SerializeField] private List<Animator> animators = new List<Animator>();
    private List<KeyValuePair<AnimationClipOverrides, AnimatorOverrideController>> defaultAndOverrideControllers;
    
    private AnimationClip animationClip;

    
    void Start()
    {
        defaultAndOverrideControllers = new List<KeyValuePair < AnimationClipOverrides, AnimatorOverrideController >> ();

        for (int i = 0; i < transform.childCount; i++) // trec prin fiecare Child Object al Player-ului si iau (daca au ) Animatoarele
        {

                Animator anim = transform.GetChild(i).GetComponent<Animator>();    
            if (anim != null)
            {
                animators.Add(anim);
            }
            
        }

        foreach (Animator animator in animators)
        {
            AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);
            animator.runtimeAnimatorController = overrideController;
            AnimationClipOverrides defaultAnimationClip = new AnimationClipOverrides(overrideController.overridesCount);
            overrideController.GetOverrides(defaultAnimationClip);
            var values = new KeyValuePair<AnimationClipOverrides, AnimatorOverrideController>(defaultAnimationClip, overrideController);
            defaultAndOverrideControllers.Add(values);
        }






        setAnimations();

    }
    public void setAnimations() // o sa setez animatiile conform SO-ului PlayerAnimSO
    {
        List<BodyPart_SO> bodyParts = PlayerAnimations.getAllParts();
        foreach(KeyValuePair<AnimationClipOverrides, AnimatorOverrideController> keyValue in defaultAndOverrideControllers)
        {
            foreach (BodyPart_SO bodyPart in bodyParts)
            {

                foreach (AnimationClip animation in bodyPart.getAnimations()) // fiecare body part are o lista de animatii(pana acum sunt 8)
                {
                //    Debug.Log(animation.name);
                    //Body_x_directie_tip, o sa dau split pentur a obtine defaultAnimation care x = "00"
                    string[] animationText = animation.name.Split('_'); // ["Body","x","directie","tip"]
                    animationText[1] = "00";
                    string defaultName = string.Join("_", animationText);
                    //Debug.Log("Default name:" + defaultName);
                    //Debug.Log("------------");
                    keyValue.Key[defaultName] = animation; //defaultAnimationClips[Key] = value(dictionar)
                }

            }
            keyValue.Value.ApplyOverrides(keyValue.Key);

        }
            
    }

    // ce era anterior ca sa testez daca functioneaza
    /* private void setAnimations()
     {
         BodyPart_SO bodyPart = PlayerAnimations.getBody();

         string animationClipPath = "Assets/AnimationFolder/idle/Body_06_down_idle.anim";
          animationClip = AssetDatabase.LoadAssetAtPath<AnimationClip>(animationClipPath);


         Debug.Log(animationClip.name);
         Debug.Log(animationClip.frameRate);

         defaultAnimationClips["Body_02_down_idle"] = animationClip;
         overrideController.ApplyOverrides(defaultAnimationClips);
     }



      */
  
    public class AnimationClipOverrides:List<KeyValuePair<AnimationClip, AnimationClip>>
    {
        public AnimationClipOverrides(int capacity):base(capacity) { }
        public AnimationClip this[string name]
        {
            get { return this.Find(x => x.Key.name.Equals(name)).Value; }
            set { int index = this.FindIndex(x => x.Key.name.Equals(name));

                if (index != -1)
                {
                    this[index] = new KeyValuePair<AnimationClip, AnimationClip>(this[index].Key, value);
                }
            }
        }
    }
        
        }
