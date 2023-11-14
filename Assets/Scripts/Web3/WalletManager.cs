using UnityEngine;
using Algorand.Unity;
using System;
using Algorand.Unity.Crypto;

public class WalletManager : MonoBehaviour
{
    private string walletPassword = "n-':%6s^@x~hG9a*;3<HEA";
    public static WalletManager instance { get; private set; }
    public string playerPrefsPath = Guid.NewGuid().ToString();
    public LocalAccountStore accountStore;
    [HideInInspector] public float DunTokenBalance;
    [HideInInspector] public float playerLevel;
    [HideInInspector] public NFTMetadatas metadatas;
    [HideInInspector] public OwnedNFTIds ownedNFTIds;
    [HideInInspector] public AlgodClient client;

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

    public void AuthenticateWithWallet()
    {
        client = new("https://testnet-algorand.api.purestake.io/ps2");
        var encryptedAccountStore = PlayerPrefs.GetString(playerPrefsPath, null);
        if (string.IsNullOrEmpty(encryptedAccountStore))
        {
            CreateNewWallet();
        }
        else
        {
            LoginToWallet();
        }
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