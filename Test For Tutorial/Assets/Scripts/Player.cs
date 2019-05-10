﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Weapon> Weapons;  
    private Weapon selectedWeapon,foundWeapon;
    private int indexWeapon = -1;
    private GameObject objectWeapon = null;
    private float curentY,time;
    private WaitForSeconds scrollTime = new WaitForSeconds(13.5f);
    [SerializeField] Inventory inventory;


    // Update is called once per frame
    private void OnTriggerEnter(Collider otherCollider)
    {
        if(otherCollider.gameObject.transform.parent != null && otherCollider.gameObject.transform.parent.GetComponent<ItemBoxes>() != null )
        {
            foundWeapon = null;
            ItemBoxes itembox = otherCollider.gameObject.transform.parent.GetComponent<ItemBoxes>();
            GiveItem(itembox.Type, itembox.Amount);
                
            if (foundWeapon == null)
            {
                //Debug.Log(otherCollider.gameObject);
                //Debug.Log(objectWeapon);
                Destroy(otherCollider.gameObject);
            }
            
        }
    }
    private void GiveItem(ItemBoxes.ItemType type,int amount) {
        Weapon currentWeapon = null;
        if (type == ItemBoxes.ItemType.Pistol)
            currentWeapon = new Pistol();
        else if (type == ItemBoxes.ItemType.AK47)
            currentWeapon = new AK47();
        else if (type == ItemBoxes.ItemType.UMP45)
            currentWeapon = new UMP45();
        else if (type == ItemBoxes.ItemType.M4A1)
            currentWeapon = new M4A1();
        for (int i = 0; i < Weapons.Count; i++)
        {
            if (Weapons[i].itemName == currentWeapon.itemName)
            {
                foundWeapon = Weapons[i];
                objectWeapon = transform.GetChild(0).GetChild(i).gameObject;
            }
        }
       
        if (foundWeapon == null)
        {   
            //Debug.Log("New Weapon");
            selectedWeapon = currentWeapon;
            Weapons.Add(selectedWeapon);
            inventory.AddItem(selectedWeapon);
        }
        if (indexWeapon == -1)
        {
            indexWeapon = Weapons.Count - 1;
            //Debug.Log("indexWeapon : " + indexWeapon);
        }
        selectedWeapon.AddAmmuntion(amount);
        selectedWeapon.LoadClip();
    }
    private void ToggleInventory()
    {
        
        if (Input.GetKeyDown(KeyCode.I))
            inventory.gameObject.SetActive(true);
        else if (Input.GetKeyUp(KeyCode.I))
            inventory.gameObject.SetActive(false);
    }
    void WeaponActive()
    {
        if (indexWeapon == -1)
            return;
        objectWeapon = Camera.main.transform.GetChild(indexWeapon).gameObject;
        objectWeapon.SetActive(true);
        for (int i = 0; i < Weapons.Count; i++)
            if (i != indexWeapon)
            {
                ///Debug.Log("finally");
                Camera.main.transform.GetChild(i).gameObject.SetActive(false);
            }
    }
    private void SwitchWeapon()
    {

        if (indexWeapon == -1)
        {
            ///Debug.Log("No weapons");
            return;
        }
        //Debug.Log("WE have a weapon at index : " + indexWeapon);
        Camera.main.transform.GetChild(indexWeapon).gameObject.SetActive(true);
        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            indexWeapon++;
            indexWeapon %= Camera.main.transform.childCount;

        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            indexWeapon--;
            if (indexWeapon < 0)
                indexWeapon = Camera.main.transform.childCount - 1;

        }
    }
    private void InspectWeapon()
    {
        if (selectedWeapon == null)
            return;
        Quaternion rotation = selectedWeapon.Prefab.transform.rotation;
        Quaternion modifiedRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + curentY, transform.rotation.eulerAngles.x);
        if (Input.GetKey(KeyCode.F) && selectedWeapon != null)
        {
            Debug.Log(time);
            
            objectWeapon.transform.localRotation = Quaternion.Slerp(rotation,modifiedRotation, time);
            if (curentY >= -100)
            {
                curentY -= 5.0f;
                time += Time.deltaTime;
            }
        }
    }

    void Start()
    {
        Weapons = new List<Weapon>();
        inventory.gameObject.SetActive(false);
        
    }
    void Update()
    {
        ToggleInventory();
        SwitchWeapon();
        WeaponActive();
        InspectWeapon();
    }
}
