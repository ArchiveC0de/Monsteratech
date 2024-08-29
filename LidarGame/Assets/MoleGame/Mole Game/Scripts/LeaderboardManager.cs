using MoleGame;
using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public RankSystem rankSystem; // RankSystem 스크립트 참조
    public TextMeshProUGUI[] playerNameTexts; // 플레이어 이름 텍스트 배열
    public TextMeshProUGUI[] scoreTexts; // 점수 텍스트 배열
    public TextMeshProUGUI[] comboTexts; // 콤보 텍스트 배열

    void Start()
    {
        UpdateLeaderboard();
    }

    void UpdateLeaderboard()
    {
        var players = rankSystem.GetPlayers(); // RankSystem에서 정렬된 플레이어 리스트 가져오기

        for (int i = 0; i < players.Length && i < playerNameTexts.Length; i++)
        {
            // 이름 대신 순위를 보여주는 경우
            playerNameTexts[i].text = (i + 1).ToString(); // 1등, 2등, 3등 순위 표시

            // 점수와 콤보 표시
            scoreTexts[i].text = players[i].score.ToString();
            comboTexts[i].text = players[i].maxCombo.ToString();
        }
    }
}
