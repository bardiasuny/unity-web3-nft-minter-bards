using MoralisUnity;
using MoralisUnity.Web3Api.Models;
using MoralisUnity.Platform.Objects;

using Nethereum.ABI.Model;
using Nethereum.Hex.HexTypes;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;
using WalletConnectSharp.Core.Models;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class GetToken : MonoBehaviour
{
    [SerializeField]
    private string contractAddress;
    [SerializeField]
    private string contractAbi;

    // Start is called before the first frame update

    readonly string Contract = "0xDE6BcE10b1F803e2A40e21D6fd8db9a50B71330B";

    public Image myicon;

    [SerializeField]
    private TextMeshProUGUI accountText;

    async void Start()
    {
        // GetTokenURI(contractAddress, contractAbi);
        FetchNFTs();
    }

    public async void FetchNFTs()
    {
        //NftOwnerCollection polygonNFTs = await Moralis.Web3Api.Account.GetNFTs("0xD6fD801B2ca1bfbC6543b76C190F6563549f1213".ToLower(), ChainList.rinkeby);
        //Debug.Log(polygonNFTs.ToJson());

        MoralisUser user = await Moralis.GetUserAsync();

        Debug.Log(user.ethAddress);

        accountText.text = user.ethAddress;

        NftOwnerCollection NFTs = await Moralis.Web3Api.Account.GetNFTsForContract(user.ethAddress.ToLower(), Contract, ChainList.rinkeby);
        Debug.Log(NFTs.Result[0].ToJson());

        string TextureURL = NFTs.Result[0].TokenUri.ToString();

        print(TextureURL);
       StartCoroutine(DownloadImage(NFTs.Result[0].TokenUri));
    
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            // image.mainTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            Texture2D texture2D = DownloadHandlerTexture.GetContent(request);
            myicon.sprite =  Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f), texture2D.height);
        }

       
    }

    //private async void GetTokenURI(string contractAddress, string contractAbi, uint tokenId = 2) {
    //    HexBigInteger value = new(0);
    //    HexBigInteger gas = new(21000000);
    //    HexBigInteger gasPrice = new(0);
    //    object[] parameters2 = {
    //        tokenId
    //        };
    //    string resp2 = await Moralis.ExecuteContractFunction(contractAddress, contractAbi, "tokenURI", parameters2, value, gas, gasPrice);
    //    print("TOKENURI" + resp2);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
