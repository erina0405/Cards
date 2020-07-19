using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerFacilitator : MonoBehaviour
{
    [SerializeField] private PlayerHand m_playerHand;
    [SerializeField] private CPUHand m_cpuHand;

    public static int ChangeCount = 1;

    private enum GameState
    {
        Invalid = -1,
        Init,
        Deal,
        Change,
        judge,
        Result
    }

    private GameState m_gameState = GameState.Invalid;

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.Invalid: // ゲーム前
                m_gameState = GameState.Init;
                break;
            case GameState.Init:　//　初期化
                m_gameState = GameState.Deal;
                break;
            case GameState.Deal: //　カードをディールする
                m_playerHand.PlayerCardDeal();
                m_cpuHand.CPUCardDeal();

                m_gameState = GameState.Change;
                break;
            case GameState.Change: //　プレイヤーは1回だけ(今は)カードをチェンジする

                if (ChangeCount < 1)
                {
                    m_gameState = GameState.judge;
                }
                break;

            case GameState.judge: //チェンジし終えたらジャッジ
               
                if (m_playerHand.PlayerJudgeHand > m_cpuHand.CPUJudgeHand)
                {
                    Debug.Log("Playerの勝ち");
                }
                else if (m_cpuHand.CPUJudgeHand > m_playerHand.PlayerJudgeHand)
                {
                    Debug.Log("CPUの勝ち");
                }
                ChangeCount++;
                m_gameState = GameState.Result;
                break;
            case GameState.Result: //結果表示などに使う

                break;
        }
    }
}
