using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MineableItemSO : ItemSO, IDestroyableItem
{
    [field: SerializeField]
    public int MineTime { get; set; } = 1; // Timpul necesar pentru a mina acest item
    public interface IDestroyableItem
    {
       
    }
}
