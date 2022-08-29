using MoralisUnity.Platform.Objects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;




public class ItemData : MoralisObject
{
    public string name { get; set; }
    public string description { get; set; }
    public string imageUrl { get; set; }

    public ItemData() : base("ItemData") { }
}
public class InventoryItem : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField] private Image myIcon;
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

    public void Init(ItemData newData)
    {
        _itemData = newData;
        StartCoroutine((string)GetTexture(_itemData.imageUrl));
    }

    public void Init(string tokenId, NftMetadata nftMetadata)
    {
        _itemData = new ItemData
        {
            objectId = tokenId,
            name = nftMetadata.name,
            description = nftMetadata.description,
            imageUrl = nftMetadata.image
        };

        StartCoroutine((string)GetTexture(_itemData.imageUrl));
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

    private IEnumerable GetTexture(string imageUrl)
    {
        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl);
        _currentWebRequest = uwr;

        yield return uwr.SendWebRequest();

        if(uwr.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(uwr.error);
            uwr.Dispose();
        }
        else
        {
            var tex = DownloadHandlerTexture.GetContent(uwr);
            myIcon.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100, 0);

            myIcon.gameObject.SetActive(true);
            myButton.interactable = true;

            uwr.Dispose();
        }



    }


}