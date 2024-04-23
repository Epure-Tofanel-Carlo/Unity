using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class FoodItemSO : ItemSO, IDestroyableItem, IItemAction
    {
        [SerializeField]
        private List<ModifierData> modifiersData = new List<ModifierData>();
        public string actionName => "Consume";
        [field: SerializeField]
        public AudioClip actionSFX { get; private set; }

        public bool PerformAction(GameObject character, List<ItemParameter> itemState = null)
        {
            foreach (ModifierData modifierData in modifiersData)
            {
                modifierData.statModifier.AffectCharacter(character, modifierData.value);
            }
            return true;
        }
    }
    public interface IDestroyableItem
    {

    }
    public interface IItemAction
    {
        public string actionName { get;}
        public AudioClip actionSFX { get; }
        bool PerformAction(GameObject character, List<ItemParameter> itemState);
    }
    [Serializable]
    public class ModifierData
    {
        public CharacterStatModifier statModifier;
        public float value;
    }
}