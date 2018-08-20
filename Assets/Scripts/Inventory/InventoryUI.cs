﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {

    public Inventory inventory;

	// Use this for initialization
	void Start () {
        inventory.ItemAdded += InventoryUIItemAdded;
	}

    private void InventoryUIItemAdded(object sender, InventoryEventArgs e) {
        Transform inventoryPanel = transform.Find("Inventory");

        foreach (Transform slot in inventoryPanel) {
            Image image = slot.GetComponent<Image>();

            // We found the empty slot.
            if (!image.enabled) {
                image.enabled = true;
                image.sprite = e.Item.Image;
                break;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
