using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItem : InventoryItem
{
    public void OnItemClicked()
    {
        Helpers.CheckNftOnOpenSea(GameManager.ContractAddress, GameManager.ContractChain.ToString(), GetId());
    }
}
