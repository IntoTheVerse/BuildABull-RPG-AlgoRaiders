using Algorand.Unity;
using Algorand.Unity.Algod;
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
    private AssetIndex index;
    private bool owns;
    private string nftName_;
    private string nftDesc;
    private Sprite nftSprite;
    private NFTDetails details;

    private void Start()
    {
        buyButton.onClick.AddListener(() => BuyNFT());
        equipButton.onClick.AddListener(() => EquipNFT());
        detailsButton.onClick.AddListener(() => ShowDetails());
        details = FindObjectOfType<NFTDetails>(true);
    }

    public void SetupNFT(string name, string desc, int price, Sprite nftSprite, bool owns, int type, int id, AssetIndex index)
    {
        this.type = type;
        this.price = price;
        this.id = id;
        this.owns = owns;
        this.nftSprite = nftSprite;
        this.index = index;
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

    private async void BuyNFT()
    {
        if (WalletManager.instance.DunTokenBalance < price)
        {
            InfoDisplay.Instance.ShowInfo("Error", $"Dun Balacne low! You need {price}, but have {WalletManager.instance.DunTokenBalance}.");
            Debug.LogError("Dun Balance too low!");
            return;
        }
        
        TransactionParametersResponse suggestedParams = (await WalletManager.instance.client.TransactionParams()).Payload;
        Address receiver = WalletManager.instance.accountStore[0].Address;

        InfoDisplay.Instance.ShowInfo("Hold tight!", "Minting");
        var TransferTxn = Transaction.AssetTransfer(
            WalletManager.instance.gameAccount.Address,
            suggestedParams,
            index,
            1,
            receiver);
        SignedTxn<AssetTransferTxn> signedTransferTxn = WalletManager.instance.gameAccount.SignTxn(TransferTxn);
        var signedTransferTxnResponse = await WalletManager.instance.client.SendTransaction(signedTransferTxn);
        LogTransactionDetails(signedTransferTxnResponse);

        if (signedTransferTxnResponse.Status == UnityEngine.Networking.UnityWebRequest.Result.Success)
        {
            var paymentTx = Transaction.Payment(receiver, suggestedParams, WalletManager.instance.gameAccount.Address, (ulong)price * 10);
            var signedPaymentTxn = WalletManager.instance.accountStore[0].SignTxn(paymentTx);
            var paymentTxnResponse = await WalletManager.instance.client.SendTransaction(signedPaymentTxn);
            LogTransactionDetails(paymentTxnResponse);
            await WalletManager.instance.client.WaitForConfirmation(signedTransferTxnResponse.Payload.TxId);
            await WalletManager.instance.client.WaitForConfirmation(paymentTxnResponse.Payload.TxId);

            if(paymentTxnResponse.Status == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                owns = true;
                CheckOwnsAndSetButtons();
                FindObjectOfType<MainMenuUI>().UpdateUserDunBalance((int)(WalletManager.instance.DunTokenBalance - price));
            }
        }

        InfoDisplay.Instance.HideInfo();
        return;
    }

    private void LogTransactionDetails(AlgoApiResponse<PostTransactionsResponse> response)
    {
        Debug.Log(response.Error);
        Debug.Log(response.Status);
        Debug.Log(response.ResponseCode);
        Debug.Log(response.Payload.TxId);
    }

    private void EquipNFT()
    {
        if(type == 0) FindObjectOfType<CharacterSelectorUI>(true).SwitchToCharacter(id);
    }

    private void ShowDetails()
    {
        details.SetupDetails(nftName_, nftDesc, owns, type, nftSprite, (int)price, () => BuyNFT(), EquipNFT);
    }
}