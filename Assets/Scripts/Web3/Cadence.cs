using DapperLabs.Flow.Sdk.Exceptions;
using DapperLabs.Flow.Sdk.Unity;
using UnityEngine;

public class Cadence : MonoBehaviour
{
    public static Cadence instance { get; private set; }

    [Header("Contracts")]
    public CadenceContractAsset userInfo;
    public CadenceContractAsset tokenAccount;
    public CadenceContractAsset nftCharacter;

    [Header("Transactions")]
    public CadenceTransactionAsset createNewUser;
    public CadenceTransactionAsset updateUserName;
    public CadenceTransactionAsset updateUserHighScore;
    public CadenceTransactionAsset createTokenVault;
    public CadenceTransactionAsset transferToken;
    public CadenceTransactionAsset setupNftCollections;
    public CadenceTransactionAsset mintCharacterNft;
    public CadenceTransactionAsset mintWeaponNft;

    [Header("Scripts")]
    public CadenceScriptAsset getUserName;
    public CadenceScriptAsset getUserHighScore;
    public CadenceScriptAsset getUserNameAndHighScore;
    public CadenceScriptAsset getTokenBalance;
    public CadenceScriptAsset getAllMetadatas;
    public CadenceScriptAsset getOwnedNftIds;

    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;
    }

    /// <summary>
    /// Debug Flow Errors
    /// </summary>
    public void DebugFlowErrors(FlowError err)
    {
        InfoDisplay.Instance.ShowInfo("Error", err.Message);
        Debug.LogError(err.Exception);
        Debug.LogError(err.Message);
        Debug.LogError(err.StackTrace);
    }
}
