using System;
using System.Numerics;
using Cysharp.Threading.Tasks;
using MoralisUnity;
using Nethereum.Hex.HexTypes;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace NFT_Minter
{
    public class MintingPanel : MonoBehaviour
    {
        [Header("Smart Contract Data")]
        public string contractAddress;
        public string contractAbi;
        [SerializeField] private string contractFunction;

      
        [Header("NFT Metadata")]
        [SerializeField] private string metadataUrl;

        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI metadataUrlText;
        [SerializeField] private Button mintButton;
        [SerializeField] private Button openSeaButton;
        [SerializeField] private TextMeshProUGUI statusText;

    
        
        private void OnEnable()
        {
            statusText.text = string.Empty;
           // metadataUrlText.text = metadataUrl;
           contractAddress = GameManager.ContractAddress;
           contractAbi = GameManager.ContractAbi;
    }

        private void OnDisable()
        {
            statusText.text = string.Empty;
            metadataUrlText.text = string.Empty;
        }


        

        private void OnValidate()
        {
            metadataUrlText.text = metadataUrl;
        }

        #region MINTING_METHODS

        public async void MintNft()
        {
            if (contractAddress == string.Empty || contractAbi == string.Empty || contractFunction == string.Empty)
            {
                Debug.LogError("Contract data is not fully set");
                return;
            }

            
            statusText.text = "Please confirm transaction in your wallet";
            mintButton.interactable = false;
        
            var result = await ExecuteMinting(3);

            if (result is null)
            {
                statusText.text = "Transaction failed";
                mintButton.interactable = true;
                return;
            }
    
            // We tell the GameManager what we minted the item successfully
            statusText.text = "Transaction completed!";
            Debug.Log($"Token Contract Address: {contractAddress}");
            
            // Activate OpenSea button
            mintButton.gameObject.SetActive(false);
            openSeaButton.gameObject.SetActive(true);
        }
    
        private async UniTask<string> ExecuteMinting(uint quantity)
        {
           
            // These are the parameters that the contract function expects
            object[] parameters = {
                quantity
            };

            // Set gas configuration. If you set it at 0, your wallet will use its default gas configuration
            HexBigInteger value = new(0);
            HexBigInteger gas = new(0);
            HexBigInteger gasPrice = new(0);


            string resp = await Moralis.ExecuteContractFunction(contractAddress, contractAbi, contractFunction, parameters, value, gas, gasPrice);


            
            return resp;
        }

        #endregion


        #region SECONDARY_METHODS

        public void ViewContract()
        {
            if (contractAddress == string.Empty)
            {
                Debug.Log("Contract address is not set");
                return;
            }
            
            MoralisTools.Web3Tools.ViewContractOnPolygonScan(contractAddress);
        }

        public void ViewOnOpenSea()
        {
           // MoralisTools.Web3Tools.ViewNftOnTestnetOpenSea(contractAddress, Moralis.CurrentChain.Name, _currentTokenId.ToString());
        }

        #endregion
    }   
}
