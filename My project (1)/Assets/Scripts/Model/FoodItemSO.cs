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
        //Functie care verifica ce lucruri modifica cand este folosit acel item, exemplu daca jucatorul mananca un mar, viata jucatorului va creste cu un anumite valori
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
    //Interfata pentru actiunea pe care poate sa o aiba un item
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