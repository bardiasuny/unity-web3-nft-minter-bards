using System;
using System.Collections;
using System.Collections.Generic;
using MoralisUnity.Platform.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



public class ItemData : MoralisObject
{
    public string hash { get; set; }
    public int edition { get; set; }
    public long date { get; set; }
    public string name { get; set; }
    public string imageUrl { get; set; }
    public string character { get; set; }
    public List<Attr> attributes { get; set; }

    public ItemData() : base("ItemData") { }
}
public class InventoryItem : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private Image myIcon;
    [SerializeField] private TextMeshProUGUI nftNameText;
    [SerializeField] private TextMeshProUGUI attribute1Text;


    [SerializeField] private Button myButton;

    private ItemData _itemData;
    private UnityWebRequest _currentWebRequest;


    private void Awake()
    {
        myIcon.gameObject.SetActive(false);
        myButton.interactable = false;
    }

    private void onDisable()
    {
        StopAllCoroutines();
        _currentWebRequest.Dispose();
    }

    #region PUBLIC_METHODS

    public void Init(ItemData newData)
    {
        _itemData = newData;
        StartCoroutine(GetTexture(_itemData.imageUrl));
    }

    public void Init(string tokenId, NftMetadata nftMetadata)
    {
        _itemData = new ItemData
        {
            objectId = tokenId,
            name = nftMetadata.name,
            imageUrl = nftMetadata.image,
            attributes = nftMetadata.attributes
        };
        nftNameText.text = _itemData.name;
        attribute1Text.text = _itemData.attributes[7].trait_type.ToString() + _itemData.attributes[7].value.ToString();
        Attr staminaObj = _itemData.attributes.Find(x => x.trait_type == "Stamina");
        attribute1Text.text = staminaObj.trait_type + staminaObj.value;

        StartCoroutine(GetTexture(_itemData.imageUrl));
    }

    public string GetId()
    {
        return _itemData.objectId;
    }

    public ItemData GetData()
    {
        return _itemData;
    }

    public Sprite GetSprite()
    {
        return myIcon.sprite;
    }

    #endregion

    #region PRIVATE_METHODS
    private IEnumerator GetTexture(string imageUrl)
    {
        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl);
        _currentWebRequest = uwr;
        
        yield return uwr.SendWebRequest();

        if (uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
            uwr.Dispose();
        }
        else
        {
            var tex = DownloadHandlerTexture.GetContent(uwr);
            myIcon.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
                
            //Now we are able to click the button and we will pass the loaded sprite :)
            myIcon.gameObject.SetActive(true);
            myButton.interactable = true;
            
            uwr.Dispose();
        }
    }
    #endregion


}
