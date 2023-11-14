using System.Data;
using UnityEngine;
using System;
using System.Linq;
using GoogleSheetsToUnity;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class HighScoreManager : SingletonMonobehaviour<HighScoreManager>
{
    private HighScores Scores = new();

    private const string SPREADSHEET_PUBLIC_ID = "1e9_OIlqk9LSTHSWGa-uLL0oGUSJHAyMqkWKm8QnzOcY";
    private const string SPREADSHEET_NAME = "WalletAddress";
    private GstuSpreadSheet spreadSheet;
    
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void Refresh()
    {
        GetScores((success) => 
        {
            // if (success && Scores.scoreList.Count > 0 && SceneManager.GetActiveScene().name == "MainMenuScene")
            // {
            //     FindObjectOfType<DisplayHighScoresUI>(true).DisplayScores(Scores);
            //     Score playerScore = Scores.scoreList.Find(x => x.walletAddress == WalletManager.instance.flowAccount.Address);
            //     if (playerScore != null)
            //         WalletManager.instance.playerLevel = playerScore.level;
            //     else
            //         WalletManager.instance.playerLevel = 0;
            // }
        });
    }

    /// <summary>
    /// Add score to high scores list
    /// </summary>
    public void AddScore(Score newScore)
    {
        if (Scores.scoreList.Exists(x => x.walletAddress == newScore.walletAddress))
        {
            Score oldScore = Scores.scoreList.Find(x => x.walletAddress == newScore.walletAddress);
            if (newScore.playerScore > oldScore.playerScore)
            {
                GetScores((success) =>
                {
                    if (success)
                    {
                        spreadSheet[oldScore.walletAddress, "Score"].UpdateCellValue(SPREADSHEET_PUBLIC_ID, SPREADSHEET_NAME, newScore.playerScore.ToString());
                    }
                });
            }

            if (oldScore.level < GameManager.Instance.currentDungeonLevelListIndex)
                spreadSheet[oldScore.walletAddress, "LevelIndex"].UpdateCellValue(SPREADSHEET_PUBLIC_ID, SPREADSHEET_NAME, GameManager.Instance.currentDungeonLevelListIndex.ToString());

            if (newScore.playerName != oldScore.playerName)
                spreadSheet[oldScore.walletAddress, "Username"].UpdateCellValue(SPREADSHEET_PUBLIC_ID, SPREADSHEET_NAME, newScore.playerName);
        }
        else
        {
            List<string> test = new() {};
            List<string> newValues = new()
            {
                newScore.walletAddress,
                newScore.playerName,
                newScore.levelDescription,
                newScore.playerScore.ToString(),
                newScore.level.ToString()
            };

            List<List<string>> combined = new()
            {
                test,
                newValues,
            };

            GetScores((success) =>
            {
                if (success)
                {
                    SpreadsheetManager.Write(
                        new GSTU_Search(SPREADSHEET_PUBLIC_ID, SPREADSHEET_NAME, $"A{spreadSheet.rows.primaryDictionary.Count}"), 
                        new ValueRange(combined), 
                        null
                    );
                }
            });
        }
    }

    private void GetScores(Action<bool> result)
    {
        SpreadsheetManager.ReadPublicSpreadsheet(new GSTU_Search(SPREADSHEET_PUBLIC_ID, SPREADSHEET_NAME), (sheet) =>
        {
            if (sheet == null)
            {
                result(false);
                Debug.LogError("Couldn't Load Sheets!");
            }
            else 
            {
                spreadSheet = sheet;
                Scores.scoreList.Clear();

                for (int i = 1; i < sheet.rows.primaryDictionary.Count; i++)
                {
                    Scores.scoreList.Add(new()
                    {
                        walletAddress = sheet.columns["Wallet Address"][i].value,
                        playerName = sheet.columns["Username"][i].value,
                        levelDescription = sheet.columns["Level"][i].value,
                        playerScore = long.Parse(sheet.columns["Score"][i].value),
                        level = long.Parse(sheet.columns["LevelIndex"][i].value)
                    });
                }

                Scores.scoreList = Scores.scoreList.OrderBy(x => x.playerScore).ToList();
                Scores.scoreList.Reverse();
                result(true);
            }
        });
    }
}