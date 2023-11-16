using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;
using Algorand.Unity;
using System.Linq;

public struct NFTMetadatas
{
    public Dictionary<AssetIndex, string> charactersMetadata;
    public Dictionary<AssetIndex, string> weaponsMetadata;
}

public struct OwnedNFTIds
{
    public List<AssetIndex> ownedCharactersId;
    public List<AssetIndex> ownedWeaponsId;
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
        SetupPlayer();
    }

    /// <summary>
    /// Checks if the User is a new User
    /// </summary>
    private void SetupPlayer()
    {
        userDunBalance.gameObject.SetActive(true);
        signInButton.SetActive(false);
        highScoresButton.SetActive(true);
        playButton.SetActive(true);
        HighScoreManager.Instance.Refresh();
        userPublicKey.text = WalletManager.instance.accountStore[0].Address;
        GetTokenBalance();
    }

    private void GetTokenBalance()
    {
        ulong dunBalance = 0;
        if (WalletManager.instance.account.Assets != null)
        {
            foreach (var item in WalletManager.instance.account.Assets)
            {
                if (item.AssetId == WalletManager.instance.dunTokenId)
                {
                    dunBalance = item.Amount;
                }
            }
        }
        UpdateUserDunBalance(dunBalance == 0 ? (int)dunBalance : (int)dunBalance / 10);
        FindObjectOfType<CharacterSelectorUI>().
            UpdateNameFromWeb3(PlayerPrefs.GetString("PlayerName", $"Player{Random.Range(0, 10000000)}"));
        GetNFTData();
    }

    private void GetNFTData()
    {
        OwnedNFTIds ownedNftIds = new()
        { 
            ownedCharactersId = new(),
            ownedWeaponsId = new()
        };
        if (WalletManager.instance.account.Assets != null)
        {
            foreach (var ownedAssets in WalletManager.instance.account.Assets)
            {
                foreach (var obj in WalletManager.instance.characterObjects)
                {
                    if (ownedAssets.AssetId == obj.Index && ownedAssets.Amount > 0)
                    { 
                        ownedNftIds.ownedCharactersId.Add(obj.Index);
                    }
                }

                foreach (var obj in WalletManager.instance.weaponObjects)
                {
                    if (ownedAssets.AssetId == obj.Index && ownedAssets.Amount > 0)
                    {
                        ownedNftIds.ownedWeaponsId.Add(obj.Index);
                    }
                }
            }
        }
        WalletManager.instance.ownedNFTIds = ownedNftIds;

        NFTMetadatas metadatas = new()
        {
            charactersMetadata = new(),
            weaponsMetadata = new()
        };
        foreach (var obj in WalletManager.instance.characterObjects)
        {
            metadatas.charactersMetadata.Add(obj.Index, obj.Params.Url);
        }

        foreach (var obj in WalletManager.instance.weaponObjects)
        {
            metadatas.weaponsMetadata.Add(obj.Index, obj.Params.Url);
        }

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
            JSONNode node = JSON.Parse(metadatas.charactersMetadata.ElementAt(i).Value);
            Instantiate(nftPrefab, characterNftSpawn).SetupNFT(
                node["Name"], 
                node["Description"], 
                node["Price"], 
                characterSprites[i], 
                ownedNftIds.ownedCharactersId.Contains(metadatas.charactersMetadata.ElementAt(i).Key), 
                0, 
                i,
                metadatas.charactersMetadata.ElementAt(i).Key);
        }

        for (int i = 0; i < metadatas.weaponsMetadata.Count; i++)
        {
            JSONNode node = JSON.Parse(metadatas.weaponsMetadata.ElementAt(i).Value);
            Instantiate(nftPrefab, weaponNftSpawn).SetupNFT(
                node["Name"],
                node["Description"],
                node["Price"],
                weaponSprites[i],
                ownedNftIds.ownedWeaponsId.Contains(metadatas.weaponsMetadata.ElementAt(i).Key),
                1,
                i,
                metadatas.weaponsMetadata.ElementAt(i).Key);
        }
        marketplaceButton.SetActive(true);
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

        if (WalletManager.instance.accountStore.IsCreated)
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
