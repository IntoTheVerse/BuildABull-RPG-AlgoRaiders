using UnityEngine;
using Algorand.Unity;
using System;
using Algorand.Unity.Crypto;
using System.Threading.Tasks;
using Algorand.Unity.Samples.CreatingAsas;
using Algorand.Unity.Algod;

public class WalletManager : MonoBehaviour
{
    private string walletPassword = "n-':%6s^@x~hG9a*;3<HEA";
    public static WalletManager instance { get; private set; }
    public string playerPrefsPath = Guid.NewGuid().ToString();
    public LocalAccountStore accountStore;
    public AccountObject gameAccount;
    public AssetIndex dunTokenId = 479674406;
    public AssetObject[] characterObjects;
    public AssetObject[] weaponObjects;
    [HideInInspector] public float playerLevel;
    [HideInInspector] public NFTMetadatas metadatas;
    [HideInInspector] public OwnedNFTIds ownedNFTIds;
    [HideInInspector] public AlgodClient client;
    [HideInInspector] public Algorand.Unity.Algod.Account account;
    [HideInInspector] public int DunTokenBalance;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(playerPrefsPath))
            playerPrefsPath = Guid.NewGuid().ToString();
    }

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public async Task<bool> AuthenticateWithWallet()
    {
        client = new("https://testnet-api.algonode.cloud");
        var encryptedAccountStore = PlayerPrefs.GetString(playerPrefsPath, null);
        if (string.IsNullOrEmpty(encryptedAccountStore))
        {
            InfoDisplay.Instance.ShowInfo("Hold tight!", "Creating new wallet!");
            CreateNewWallet();
            await OptIn();
            InfoDisplay.Instance.HideInfo();
        }
        else
        {
            InfoDisplay.Instance.ShowInfo("Hold tight!", "Logging In!");
            LoginToWallet();
            InfoDisplay.Instance.HideInfo();
        }
        account = (await client.AccountInformation(accountStore[0].Address)).Payload.Account;
        return true;
    }

    private void LoginToWallet()
    {
        Debug.Log("Open Wallet");
        var encryptedAccountStore = PlayerPrefs.GetString(playerPrefsPath, null);
        using var securePassword = new SodiumString(walletPassword);
        var decryptError = LocalAccountStore.Decrypt(encryptedAccountStore, securePassword, out accountStore);
        if (decryptError == LocalAccountStore.DecryptError.InvalidFormat)
        {
            PlayerPrefs.DeleteKey(playerPrefsPath);
            PlayerPrefs.Save();
            return;
        }
    }

    private void CreateNewWallet()
    {
        Debug.Log("Create Wallet");
        using var securePassword = new SodiumString(walletPassword);
        accountStore = new LocalAccountStore(securePassword);
        using var seedRef = SodiumReference<Ed25519.Seed>.Alloc();
        using var secretKeyRef = SodiumReference<Ed25519.SecretKey>.Alloc();
        var pk = default(Ed25519.PublicKey);
        Ed25519.GenKeyPair(ref seedRef.RefValue, ref secretKeyRef.RefValue, ref pk);
        accountStore = accountStore.Add(ref secretKeyRef.RefValue);
        Save();
    }

    private async Task OptIn()
    {
        TransactionParametersResponse suggestedParams = (await client.TransactionParams()).Payload;
        Address receiver = accountStore[0].Address;

        InfoDisplay.Instance.ShowInfo("Hold tight!", "Airdropping Algod");
        var paymentTx = Transaction.Payment(gameAccount.Address, suggestedParams, receiver, 1_000_000L);
        var signedPaymentTxn = gameAccount.SignTxn(paymentTx);
        var paymentTxnResponse = await client.SendTransaction(signedPaymentTxn);
        LogTransactionDetails(paymentTxnResponse);
        await client.WaitForConfirmation(paymentTxnResponse.Payload.TxId);

        InfoDisplay.Instance.ShowInfo("Hold tight!", "Opting in to Dun tokens");
        var tokenOptinTxn = Transaction.AssetAccept(receiver,suggestedParams,dunTokenId);
        SignedTxn<AssetTransferTxn> signedTokenOptinTxn = accountStore[0].SignTxn(txn: tokenOptinTxn);
        var tokenOptinTxnResponse = await client.SendTransaction(signedTokenOptinTxn);
        LogTransactionDetails(tokenOptinTxnResponse);
        await client.WaitForConfirmation(tokenOptinTxnResponse.Payload.TxId);

        InfoDisplay.Instance.ShowInfo("Hold tight!", "Opting in to characters and weapons");
        for (int i = 0; i < characterObjects.Length; i++)
        {
            var characterOptinTxn = Transaction.AssetAccept(receiver, suggestedParams, characterObjects[i].Index);
            SignedTxn<AssetTransferTxn> signedCharacterOptinTxn = accountStore[0].SignTxn(txn: characterOptinTxn);
            var characterOptinTxnResponse = await client.SendTransaction(signedCharacterOptinTxn);
            LogTransactionDetails(characterOptinTxnResponse);
            await client.WaitForConfirmation(characterOptinTxnResponse.Payload.TxId);
        }

        for (int i = 0; i < weaponObjects.Length; i++)
        {
            var weaponOptinTxn = Transaction.AssetAccept(receiver, suggestedParams, weaponObjects[i].Index);
            SignedTxn<AssetTransferTxn> signedWeaponOptinTxn = accountStore[0].SignTxn(txn: weaponOptinTxn);
            var weaponOptinTxnResponse = await client.SendTransaction(signedWeaponOptinTxn);
            LogTransactionDetails(weaponOptinTxnResponse);
            await client.WaitForConfirmation(weaponOptinTxnResponse.Payload.TxId);
        }

        InfoDisplay.Instance.ShowInfo("Hold tight!", "Initializing default players and weapons");
        var defaultCharacterTransferTxn = Transaction.AssetTransfer(
            gameAccount.Address,
            suggestedParams,
            characterObjects[0].Index,
            1,
            receiver);
        SignedTxn<AssetTransferTxn> signedDefaultCharacterTransferTxn = gameAccount.SignTxn(defaultCharacterTransferTxn);
        var signedDefaultCharacterTransferTxnResponse = await client.SendTransaction(signedDefaultCharacterTransferTxn);
        LogTransactionDetails(signedDefaultCharacterTransferTxnResponse);
        await client.WaitForConfirmation(signedDefaultCharacterTransferTxnResponse.Payload.TxId);

        var defaultWeaponTransferTxn = Transaction.AssetTransfer(
            gameAccount.Address,
            suggestedParams,
            weaponObjects[0].Index,
            1,
            receiver);
        SignedTxn<AssetTransferTxn> signedDefaultWeaponTransferTxn = gameAccount.SignTxn(defaultWeaponTransferTxn);
        var signedDefaultWeaponTransferTxnResponse = await client.SendTransaction(signedDefaultWeaponTransferTxn);
        LogTransactionDetails(signedDefaultWeaponTransferTxnResponse);
        await client.WaitForConfirmation(signedDefaultWeaponTransferTxnResponse.Payload.TxId);
    }

    private void LogTransactionDetails(AlgoApiResponse<PostTransactionsResponse> response)
    {
        Debug.Log(response.Error);
        Debug.Log(response.Status);
        Debug.Log(response.ResponseCode);
        Debug.Log(response.Payload.TxId);
    } 

    public void Save()
    {
        if (accountStore.IsCreated)
        {
            var encryptError = accountStore.Encrypt(out var encryptedString);
            if (encryptError != LocalAccountStore.EncryptError.None)
            {
                Debug.LogError($"Failed to encrypt account store: {encryptError}");
                return;
            }
            PlayerPrefs.SetString(playerPrefsPath, encryptedString);
            PlayerPrefs.Save();
        }
    }

    private void OnDisable()
    {
        if (accountStore.IsCreated)
        {
            Save();
            accountStore.Dispose();
        }
    }
}