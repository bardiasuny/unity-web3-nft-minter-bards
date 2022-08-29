using Cysharp.Threading.Tasks.Triggers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using System;
using System.Linq;
using UnityEditor.Tilemaps;

[SerializeField]
public class NftMetadata
{
    public string name;
    public string description;
    public string image;
}

public class PlayerInventory : Inventory
{

  

    protected override void Awake()
    {
        base.Awake();

        titleText.text = "Player Invetory";
        OpenInvetory();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OpenInvetory()
    {
    

        print("truning uion");
        //ActivatePanel(true);
        //Opened?.Invoke();

        LoadPurchasedNfts();

    }

    private async void LoadPurchasedNfts()
    {
        MoralisUser user = await Moralis.GetUserAsync();
        var playerAddress = user.authData["moralisEth"]["id"].ToString();

        print(playerAddress);
        print(GameManager.ContractAddress);

        print(GameManager.ContractChain);


        try
        {
            NftOwnerCollection noc = await Moralis.Web3Api.Account.GetNFTsForContract(playerAddress.ToLower(), GameManager.ContractAddress, GameManager.ContractChain);

            List<NftOwner> nftOwners = noc.Result;

            Debug.Log(noc.ToJson());

            if(!nftOwners.Any())
            {
                Debug.Log("you dont have any NFT");
            }

            foreach (var nftOwner in nftOwners)
            {
                //if (nftOwner.Metadata == null)
                //{
                //    Moralis.Web3Api.Token.ReSyncMetadata(nftOwner.TokenAddress, nftOwner.TokenId, GameManager.ContractChain);
                //    Debug.Log("we couldnt get Metadata on first try: Re-Syncing ....");
                //    continue;
                //}

                var nftMetaData = nftOwner.Metadata;
                NftMetadata formattedMetaData = JsonUtility.FromJson<NftMetadata>(nftMetaData);

                PopulatePlayerItems(nftOwner.TokenId, formattedMetaData);

            }
        }
        catch (Exception exp)
        {
             Debug.LogError(exp.Message);
        }

    }

    private void PopulatePlayerItems(string tokenId, NftMetadata data)
    {
        InventoryItem newItem = Instantiate(item, itemsGrid.transform);
        newItem.Init(tokenId, data);
    }


}
