using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NFT : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nftName;
    [SerializeField] private TextMeshProUGUI nftPrice;
    [SerializeField] private Image nftImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button detailsButton;

    private float price;
    private int type;
    private int id;
    private bool owns;
    private string nftName_;
    private string nftDesc;
    private Sprite nftSprite;
    private NFTDetails details;

    private void Start()
    {
        //buyButton.onClick.AddListener(() => StartCoroutine(BuyNFT()));
        equipButton.onClick.AddListener(() => EquipNFT());
        detailsButton.onClick.AddListener(() => ShowDetails());
        details = FindObjectOfType<NFTDetails>(true);
    }

    public void SetupNFT(string name, string desc, int price, Sprite nftSprite, bool owns, int type, int id)
    {
        this.type = type;
        this.price = price;
        this.id = id;
        this.owns = owns;
        this.nftSprite = nftSprite;
        nftDesc = desc;
        nftName_ = name;

        nftName.text = name;
        nftPrice.text = price.ToString();
        nftImage.sprite = nftSprite;

        CheckOwnsAndSetButtons();
    }

    private void CheckOwnsAndSetButtons()
    {
        equipButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        if (owns)
        {
            if (type == 0) equipButton.gameObject.SetActive(true);
        }
        else buyButton.gameObject.SetActive(true);
    }

    // private IEnumerator BuyNFT()
    // {
    //     if (WalletManager.instance.DunTokenBalance < price)
    //     {
    //         InfoDisplay.Instance.ShowInfo("Error", $"Dun Balacne low! You need {price}, but have {WalletManager.instance.DunTokenBalance}.");
    //         Debug.LogError("Dun Balance too low!");
    //         yield break;
    //     }

    //     string transactionString = type == 0 ? Cadence.instance.mintCharacterNft.text : Cadence.instance.mintWeaponNft.text;
    //     var txResponse = Transactions.SubmitAndWaitUntilSealed
    //     (
    //         transactionString,
    //         Convert.ToCadence(WalletManager.instance.flowAccount.Address, "Address"),
    //         Convert.ToCadence((System.UInt64)id, "UInt64"),
    //         Convert.ToCadence((decimal)price, "UFix64")
    //     );
    //     InfoDisplay.Instance.ShowInfo("Sign Transaction", $"Please sign the {(type == 0 ? "character" : "weapon")} minting trasaction from your wallet!");
    //     yield return new WaitUntil(() => txResponse.IsCompleted);
    //     InfoDisplay.Instance.HideInfo();
    //     var txResult = txResponse.Result;
    //     if (txResult.Error != null)
    //     {
    //         Cadence.instance.DebugFlowErrors(txResult.Error);
    //         yield break;
    //     }
    //     else
    //     {
    //         if (txResult.StatusCode == 1)
    //         {
    //             InfoDisplay.Instance.ShowInfo("Error", "Internal Error");
    //             yield break;
    //         }
    //         Debug.Log($"Transaction Completion Code: {txResult.StatusCode}");
    //         owns = true;
    //         CheckOwnsAndSetButtons();
    //         FindObjectOfType<MainMenuUI>().UpdateUserDunBalance((int)(WalletManager.instance.DunTokenBalance - price));
    //         yield break;
    //     }
    // }

    private void EquipNFT()
    {
        if(type == 0) FindObjectOfType<CharacterSelectorUI>(true).SwitchToCharacter(id - 1);
    }

    private void ShowDetails()
    {
        //details.SetupDetails(nftName_, nftDesc, owns, type, nftSprite, (int)price, () => StartCoroutine(BuyNFT()), EquipNFT);
    }
}
