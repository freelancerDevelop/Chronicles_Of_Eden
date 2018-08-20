﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// An item that can be pickedup and used from the inventory.
/// </summary>
public abstract class EquipableItem : ScriptableObject {

    [SerializeField] string InGameName;
    [SerializeField] Sprite _Image;
    /// <summary>
    /// Activate the item.
    /// </summary>
    /// <param name="userTranform"> The transform of the character using the item. </param>
    public abstract void Use(Transform userTranform);

    public Sprite Image {
        get {
            return _Image;
        }
    }

}
