using UnityEngine;
using UnityEngine.UI;

public class DisplayHighScoresUI : MonoBehaviour
{
    #region Header OBJECT REFERENCES
    [Space(10)]
    [Header("OBJECT REFERENCES")]
    #endregion Header OBJECT REFERENCES
    #region Tooltip
    [Tooltip("Populate with the child Content gameobject Transform component")]
    #endregion Tooltip
    [SerializeField] private Transform contentAnchorTransform;
    [SerializeField] private Button RefreshButton;

    void Start()
    {
        RefreshButton.onClick.AddListener(() => FindAnyObjectByType<HighScoreManager>().Refresh());
    }

    public void DisplayScores(HighScores highScores)
    {
        for (int i = 1; i < contentAnchorTransform.childCount; i++)
        {
            Destroy(contentAnchorTransform.GetChild(i).gameObject);
        }

        int rank = 0;
        foreach (Score score in highScores.scoreList)
        {
            rank++;

            GameObject scoreGameobject = Instantiate(GameResources.Instance.scorePrefab, contentAnchorTransform);

            ScorePrefab scorePrefab = scoreGameobject.GetComponent<ScorePrefab>();

            if (rank == 1) scorePrefab.rankTMP.color = Color.yellow;
            else if (rank == 2) scorePrefab.rankTMP.color = new(0.85f, 0.85f, 0.85f);
            else if (rank == 3) scorePrefab.rankTMP.color = new(1, 0.5f, 0);
            else scorePrefab.rankTMP.color = Color.grey;

            scorePrefab.rankTMP.text = rank.ToString();
            scorePrefab.nameTMP.text = score.playerName;
            scorePrefab.levelTMP.text = score.levelDescription;
            scorePrefab.scoreTMP.text = score.playerScore.ToString();
        }
    }
}
