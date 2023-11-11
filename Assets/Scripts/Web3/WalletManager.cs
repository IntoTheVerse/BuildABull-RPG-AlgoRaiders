using UnityEngine;
using DapperLabs.Flow.Sdk;
using DapperLabs.Flow.Sdk.Unity;
using DapperLabs.Flow.Sdk.Crypto;
using DapperLabs.Flow.Sdk.WalletConnect;
using System.Threading.Tasks;
using DapperLabs.Flow.Sdk.DataObjects;

public class WalletManager : MonoBehaviour
{
    public static WalletManager instance { get; private set; }
    public FlowAccount flowAccount;
    public FlowControl.Account scriptsExecutionAccount;
    public FlowControlData flowControl;
    public GameObject qrCodeCustomPrefab;
    public GameObject walletSelectCustomPrefab;
    private string walletAddress = "";
    [HideInInspector] public float DunTokenBalance;
    [HideInInspector] public float playerLevel;
    [HideInInspector] public NFTMetadatas metadatas;
    [HideInInspector] public OwnedNFTIds ownedNFTIds;

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

    void Start()
    {
        FlowConfig flowConfig = new()
        {
            NetworkUrl = FlowConfig.TESTNETURL,
            Protocol = FlowConfig.NetworkProtocol.HTTP
        };
        FlowSDK.Init(flowConfig);
        IWallet walletProvider = new WalletConnectProvider();
        walletProvider.Init(new WalletConnectConfig
        {
            ProjectId = "c5a0e570828c856d8d6908a95e64d40c",
            ProjectDescription = "Dungeon Flow is a game developed for Flow Hackathon",
            ProjectIconUrl = "https://walletconnect.com/meta/favicon.ico",
            ProjectName = "Dungeon Flow",
            ProjectUrl = "https://linktr.ee/intotheverse",
            QrCodeDialogPrefab = qrCodeCustomPrefab,
            WalletSelectDialogPrefab = walletSelectCustomPrefab
        });
        FlowSDK.RegisterWalletProvider(walletProvider);
        scriptsExecutionAccount = new()
        {
            GatewayName = "Flow Testnet"
        };
    }

    public async Task AuthenticateWithWallet()
    {
        await FlowSDK.GetWalletProvider().Authenticate("", (string flowAddress) =>
        {
            walletAddress = flowAddress;
        }, () =>
        {
            Debug.LogError("Authentication failed.");
        });
    }

    public async Task SetFlowAccout()
    {
        if (string.IsNullOrEmpty(walletAddress))
        {
            Debug.LogError("Unable to load wallet!");
        }
        else
        {
            var acc = await Accounts.GetByAddress(walletAddress);
            if (acc.Error != null)
            {
                Cadence.instance.DebugFlowErrors(acc.Error);
                FlowSDK.GetWalletProvider().Unauthenticate();
                return;
            }
            else
            {
                flowAccount = acc;
            }
        }
    }
}
