using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameProgressViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_progressText;

    private const string GameStartText = "ゲームが開始しました";
    private const string GameChangeText = "カードを選択し、ボタンをクリックしてチェンジしてください";
    private const string GameBetText = "ベットしてください、勝負しない場合は0を入力してEnterキーを押してください";

    public void SetGameStartText()
    {
        m_progressText.text = GameStartText;
    }

    public void SetGameChangeText()
    {
        m_progressText.text = GameChangeText;
    }

    public void SetGameBetText()
    {
        m_progressText.text = GameBetText;
    }

    public void SetGameJudgeText(bool playerWin,PokerHand.Hand playerHand, PokerHand.Hand cpuHand)
    {
        if (playerWin)
        {
            m_progressText.text = $"{playerHand}、{cpuHand}よってPlayerの勝ちです";
        }
        else
        {
            m_progressText.text = $"{cpuHand}、{playerHand}よってCPUの勝ちです";
        }
    }
}
