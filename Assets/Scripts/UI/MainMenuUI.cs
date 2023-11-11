using System.Collections;
using System.Collections.Generic;
using DapperLabs.Flow.Sdk;
using DapperLabs.Flow.Sdk.Cadence;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

public struct NFTMetadatas
{
    public Dictionary<System.UInt64, string> charactersMetadata;
    public Dictionary<System.UInt64, string> weaponsMetadata;
}

public struct OwnedNFTIds
{
    public List<System.UInt64> ownedCharactersId;
    public List<System.UInt64> ownedWeaponsId;
}

public class MainMenuUI : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    [Tooltip("Populate with the enter the dungeon play button gameobject")]
    [SerializeField] private GameObject playButton;

    [Tooltip("Populate with the quit button gameobject")]
    [SerializeField] private GameObject quitButton;

    [Tooltip("Populate with the high scores button gameobject")]
    [SerializeField] private GameObject highScoresButton;

    [Tooltip("Populate with the instructions button gameobject")]
    [SerializeField] private GameObject instructionsButton;

    [Tooltip("Populate with the return to main menu button gameobject")]
    [SerializeField] private GameObject returnToMainMenuButton;

    [Tooltip("Populate with sign in button gameobject")]
    [SerializeField] private GameObject signInButton;
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject InstructionMenuPanel;
    [SerializeField] private GameObject HighScorePanel;
    [SerializeField] private GameObject marketplaceButton;

    [SerializeField] private TextMeshProUGUI userPublicKey;
    [SerializeField] private TextMeshProUGUI userDunBalance;
    [SerializeField] private NFT nftPrefab;
    [SerializeField] private Transform characterNftSpawn;
    [SerializeField] private Transform weaponNftSpawn;
    [SerializeField] private Sprite[] characterSprites;
    [SerializeField] private Sprite[] weaponSprites;

    private void Start()
    {
        MusicManager.Instance.PlayMusic(GameResources.Instance.mainMenuMusic, 0f, 2f);
        returnToMainMenuButton.SetActive(false);
    }

    /// <summary>
    /// Called from the Play Game / Enter The Dungeon Button
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("MainGameScene");
    }

    /// <summary>
    /// Sign in using WalletConnect
    /// </summary>
    public async void SignIn()
    {
        await WalletManager.instance.AuthenticateWithWallet();
        await WalletManager.instance.SetFlowAccout();

        if(FlowSDK.GetWalletProvider().IsAuthenticated())
            StartCoroutine(SetupPlayer());
    }

    /// <summary>
    /// Checks if the User is a new User
    /// </summary>
    private IEnumerator SetupPlayer()
    {
        HighScoreManager.Instance.Refresh();
        var scpRespone = WalletManager.instance.scriptsExecutionAccount.ExecuteScript(
            Cadence.instance.getUserName.text, 
            Convert.ToCadence(WalletManager.instance.flowAccount.Address, "Address")
        );
        yield return new WaitUntil(() => scpRespone.IsCompleted);
        var scpResult = scpRespone.Result;
        if (scpResult.Error != null)
        {
            var txResponse = Transactions.SubmitAndWaitUntilSealed
            (
                Cadence.instance.createNewUser.text
            );
            InfoDisplay.Instance.ShowInfo("Sign Transaction", "Please sign the account creation trasaction from your wallet!");
            yield return new WaitUntil(() => txResponse.IsCompleted);
            InfoDisplay.Instance.HideInfo();
            var txResult = txResponse.Result;

            if (txResult.Error != null)
            {
                Cadence.instance.DebugFlowErrors(txResult.Error);
                FlowSDK.GetWalletProvider().Unauthenticate();
                yield break;
            }
            else
            {
                Debug.Log($"Transaction Completion Code: {txResult.StatusCode}");
                StartCoroutine(SetupPlayer());
                yield break;
            }
        }
        else
        {
            StartCoroutine(GetTokenBalance());
            userDunBalance.gameObject.SetActive(true);
            FindObjectOfType<CharacterSelectorUI>().UpdateNameFromWeb3(Convert.FromCadence<string>(scpResult.Value));
            userPublicKey.text = $"Public Key: 0x{WalletManager.instance.flowAccount.Address}";
            signInButton.SetActive(false);
            highScoresButton.SetActive(true);
            playButton.SetActive(true);
        }
    }

    private IEnumerator GetTokenBalance()
    {
        var scpTokenRespone = WalletManager.instance.scriptsExecutionAccount.ExecuteScript(
        Cadence.instance.getTokenBalance.text,
        Convert.ToCadence(WalletManager.instance.flowAccount.Address, "Address")
        );
        yield return new WaitUntil(() => scpTokenRespone.IsCompleted);
        var scpTokenResult = scpTokenRespone.Result;
        if (scpTokenResult.Error != null)
        {
            var txResponse = Transactions.SubmitAndWaitUntilSealed
            (
                Cadence.instance.createTokenVault.text
            );
            InfoDisplay.Instance.ShowInfo("Sign Transaction", "Please sign the token account creation trasaction from your wallet!");
            yield return new WaitUntil(() => txResponse.IsCompleted);
            InfoDisplay.Instance.HideInfo();
            var txResult = txResponse.Result;
            if (txResult.Error != null)
            {
                userDunBalance.gameObject.SetActive(false);
                Cadence.instance.DebugFlowErrors(txResult.Error);
                FlowSDK.GetWalletProvider().Unauthenticate();
                yield break;
            }
            else
            {
                Debug.Log($"Transaction Completion Code: {txResult.StatusCode}");
                StartCoroutine(GetTokenBalance());
                yield break;
            }
        }
        else
        {
            WalletManager.instance.DunTokenBalance = (int)Convert.FromCadence<decimal>(scpTokenResult.Value);
            userDunBalance.text = $"$DUN: {(int)Convert.FromCadence<decimal>(scpTokenResult.Value)}";
            StartCoroutine(GetNFTData());
        }
    }

    private IEnumerator GetNFTData()
    {
        InfoDisplay.Instance.ShowInfo("Info", "Fetching NFTs");
        var characterIdRespone = WalletManager.instance.scriptsExecutionAccount.ExecuteScript(
        Cadence.instance.getOwnedNftIds.text,
        Convert.ToCadence(WalletManager.instance.flowAccount.Address, "Address")
        );
        yield return new WaitUntil(() => characterIdRespone.IsCompleted);
        var characterIdResult = characterIdRespone.Result;
        if (characterIdResult.Error != null)
        {
            var txResponse = Transactions.SubmitAndWaitUntilSealed
            (
                Cadence.instance.setupNftCollections.text,
                Convert.ToCadence((System.UInt64)1, "UInt64")
            );
            InfoDisplay.Instance.ShowInfo("Sign Transaction", "Please sign the NFT collection creation trasaction from your wallet!");
            yield return new WaitUntil(() => txResponse.IsCompleted);
            InfoDisplay.Instance.HideInfo();
            var txResult = txResponse.Result;
            if (txResult.Error != null)
            {
                userDunBalance.gameObject.SetActive(false);
                Cadence.instance.DebugFlowErrors(txResult.Error);
                FlowSDK.GetWalletProvider().Unauthenticate();
                yield break;
            }
            else
            {
                Debug.Log($"Transaction Completion Code: {txResult.StatusCode}");
                StartCoroutine(GetNFTData());
                yield break;
            }
        }
        else
        {
            InfoDisplay.Instance.ShowInfo("Info", "Fetching NFTs");
            OwnedNFTIds ownedNftIds = Convert.FromCadence<OwnedNFTIds>(characterIdResult.Value);
            ownedNftIds.ownedCharactersId.Add(1);
            ownedNftIds.ownedWeaponsId.Add(1);
            WalletManager.instance.ownedNFTIds = ownedNftIds;

            var scpTokenRespone = WalletManager.instance.scriptsExecutionAccount.ExecuteScript(
                Cadence.instance.getAllMetadatas.text
            );
            yield return new WaitUntil(() => scpTokenRespone.IsCompleted);
            var scpTokenResult = scpTokenRespone.Result;
            if (scpTokenResult.Error != null)
            {
                Cadence.instance.DebugFlowErrors(scpTokenResult.Error);
                FlowSDK.GetWalletProvider().Unauthenticate();
                yield break;
            }
            else
            {
                NFTMetadatas metadatas = Convert.FromCadence<NFTMetadatas>(scpTokenResult.Value);
                WalletManager.instance.metadatas = metadatas;

                foreach (Transform item in characterNftSpawn)
                {
                    Destroy(item.gameObject);
                }

                foreach (Transform item in weaponNftSpawn)
                {
                    Destroy(item.gameObject);
                }

                for (int i = 0; i < metadatas.charactersMetadata.Count; i++)
                {
                    JSONNode node = JSON.Parse(metadatas.charactersMetadata[(ulong)i + 1]);
                    Instantiate(nftPrefab, characterNftSpawn).SetupNFT(
                        node["Name"], 
                        node["Description"], 
                        node["Price"], 
                        characterSprites[i], 
                        ownedNftIds.ownedCharactersId.Contains((ulong)i + 1), 
                        0, 
                        (int)(i + 1));
                }

                for (int i = 0; i < metadatas.weaponsMetadata.Count; i++)
                {
                    JSONNode node = JSON.Parse(metadatas.weaponsMetadata[(ulong)i + 1]);
                    Instantiate(nftPrefab, weaponNftSpawn).SetupNFT(
                        node["Name"],
                        node["Description"],
                        node["Price"],
                        weaponSprites[i],
                        ownedNftIds.ownedWeaponsId.Contains((ulong)i + 1),
                        1,
                        (int)(i + 1));
                }
                marketplaceButton.gameObject.SetActive(true);
            }
        }
        InfoDisplay.Instance.HideInfo();
    }

    public void UpdateUserDunBalance(int val)
    {
        WalletManager.instance.DunTokenBalance = val;
        userDunBalance.text = $"$DUN: {val}";
    }

    /// <summary>
    /// Called from the High Scores Button
    /// </summary>
    public void LoadHighScores()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        marketplaceButton.SetActive(false);
        instructionsButton.SetActive(false);
        returnToMainMenuButton.SetActive(true);

        MainMenuPanel.SetActive(false);
        InstructionMenuPanel.SetActive(false);
        HighScorePanel.SetActive(true);
    }

    /// <summary>
    /// Called from the Return To Main Menu Button
    /// </summary>
    public void LoadCharacterSelector()
    {
        returnToMainMenuButton.SetActive(false);
        MainMenuPanel.SetActive(true);
        InstructionMenuPanel.SetActive(false);
        HighScorePanel.SetActive(false);
        instructionsButton.SetActive(true);

        if (FlowSDK.GetWalletProvider().IsAuthenticated())
        {
            playButton.SetActive(true);
            highScoresButton.SetActive(true);
            marketplaceButton.SetActive(true);
        }
        else
        { 
            signInButton.SetActive(true);
        }

    }

    /// <summary>
    /// Called from the Instructions Button
    /// </summary>
    public void LoadInstructions()
    {
        playButton.SetActive(false);
        quitButton.SetActive(false);
        highScoresButton.SetActive(false);
        instructionsButton.SetActive(false);
        signInButton.SetActive(false);
        marketplaceButton.SetActive(false);
        returnToMainMenuButton.SetActive(true);

        MainMenuPanel.SetActive(false);
        InstructionMenuPanel.SetActive(true);
        HighScorePanel.SetActive(false);
    }

    /// <summary>
    /// Quit the game - this method is called from the onClick event set in the inspector
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }


    #region Validation
#if UNITY_EDITOR
    // Validate the scriptable object details entered
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(playButton), playButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(quitButton), quitButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(highScoresButton), highScoresButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(instructionsButton), instructionsButton);
        HelperUtilities.ValidateCheckNullValue(this, nameof(returnToMainMenuButton), returnToMainMenuButton);
    }
#endif
    #endregion
}
