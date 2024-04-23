using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterStatModifier : ScriptableObject
{
    // is abstract because an item can modify health, mana, stats, etc
    public abstract void AffectCharacter(GameObject character, float val);
}
