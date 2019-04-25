﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Weapon> Weapons;  
    private Weapon selectedWeapon;
    [SerializeField] Inventory inventory;
    void Start()
    {
        Weapons = new List<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider otherCollider)
    {
        Debug.Log("DA");
        if(otherCollider.gameObject.transform.parent.GetComponent<ItemBox>() != null)
        {
            ItemBox itembox = otherCollider.gameObject.transform.parent.GetComponent<ItemBox>();         
            GiveItem(itembox.Type, itembox.Amount);
            Destroy(otherCollider.gameObject);
        }
    }
    private void GiveItem(ItemBox.ItemType type,int amount) {  
        if(type == ItemBox.ItemType.Pistol)
        {
            Weapon curweapon = null;
            for(int i = 0; i < Weapons.Count; i++)
            {
                if (Weapons[i] is Pistol)
                    curweapon = Weapons[i];
            }

            if (curweapon == null)
            {
                curweapon = new Pistol();
                selectedWeapon = curweapon;
                Weapons.Add(selectedWeapon);
                inventory.AddItem(selectedWeapon);
            }
            selectedWeapon.AddAmmuntion(amount);
            selectedWeapon.LoadClip();
        }
    }
}