using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFTDetails : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI nftName;
    [SerializeField] public TextMeshProUGUI nftDesc;
    [SerializeField] public TextMeshProUGUI nftPrice;
    [SerializeField] public GameObject buyButton;
    [SerializeField] public GameObject equipButton;
    [SerializeField] public Image nftImage;
    [SerializeField] public GameObject panel;

    private Action buyAction;
    private Action equipAction;

    public void SetupDetails(string name, string desc, bool owns, int type, Sprite sprite, int price, Action buy, Action equip)
    {
        panel.SetActive(true);
        nftName.text = name;
        nftDesc.text = desc;
        nftImage.sprite = sprite;
        nftPrice.text = price.ToString();

        buyAction = buy;
        equipAction = equip;

        if (owns) 
        {
            if (type == 0)
            { 
                equipButton.SetActive(true);
                buyButton.SetActive(false);
            }
            else
            {
                equipButton.SetActive(false);
                buyButton.SetActive(false);
            }
        }
        else
        {
            equipButton.SetActive(false);
            buyButton.SetActive(true);
        }
    }

    public void Buy()
    {
        buyAction?.Invoke();
        panel.SetActive(false);
    }

    public void Equip()
    {
        equipAction?.Invoke();
        panel.SetActive(false);
    }
}
