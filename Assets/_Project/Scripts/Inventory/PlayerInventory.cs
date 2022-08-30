using System;
using System.Collections.Generic;
using System.Linq;
using MoralisUnity;
using MoralisUnity.Platform.Objects;
using MoralisUnity.Web3Api.Models;
using UnityEngine;
using UnityEngine.InputSystem;

[SerializeField]
public class NftMetadata
{
    public string name;
    public string description;
    public string image;
    public string attributes;

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
       // ActivatePanel(true);
        Opened?.Invoke();

        LoadPurchasedNfts();

    }

    private async void LoadPurchasedNfts()
    {
        MoralisUser user = await Moralis.GetUserAsync();
        var playerAddress = user.authData["moralisEth"]["id"].ToString();

        try
        {
            NftOwnerCollection noc = await Moralis.Web3Api.Account.GetNFTsForContract(playerAddress.ToLower(), GameManager.ContractAddress.ToLower(), GameManager.ContractChain);

            List<NftOwner> nftOwners = noc.Result;

            if (!nftOwners.Any())
            {
                Debug.Log("you dont have any NFT");
            }

            foreach (var nftOwner in nftOwners)
            {
                if (nftOwner.Metadata == null)
                {
                    Moralis.Web3Api.Token.ReSyncMetadata(address: nftOwner.TokenAddress, tokenId: nftOwner.TokenId, chain: GameManager.ContractChain);
                    Debug.Log("we couldnt get Metadata on first try: Re-Syncing ....");
                    continue;
                }

                var nftMetaData = nftOwner.Metadata;
                NftMetadata formattedMetaData = JsonUtility.FromJson<NftMetadata>(nftMetaData);

                print(formattedMetaData.attributes.ToString());

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
        newItem.gameObject.SetActive(true);
        newItem.Init(tokenId, data);
    }


}
