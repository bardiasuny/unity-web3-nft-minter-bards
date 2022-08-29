using MoralisUnity.Platform.Objects;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour

{

    public static Action Opened;
    public static Action Closed;

    [Header("Item Prefab")]
    [SerializeField] protected InventoryItem item;

    [Header("UI Elements")]
    [SerializeField] protected GameObject uiPanel;
    [SerializeField] protected TextMeshProUGUI titleText;
    [SerializeField] protected GridLayoutGroup itemsGrid;
    [SerializeField] protected GameObject logo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected virtual void Awake()
    {

    }



    protected void UpdateItem(string idToUpdate, ItemData newData)
    {     
        foreach(Transform childItem in itemsGrid.transform)
        {
            InventoryItem item = childItem.GetComponent<InventoryItem>();

           
        }
    }

    protected void ActivatePanel(bool activate)
    {
        //if(activate)
        //{
        //    //TODO play sound
        //}
        if (uiPanel == null) return;
        uiPanel.SetActive(activate);

        if (logo = null) return;
        logo.SetActive(activate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
