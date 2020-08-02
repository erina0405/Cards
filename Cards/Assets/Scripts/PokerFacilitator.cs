using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerFacilitator : MonoBehaviour
{
    [SerializeField] private PlayerHand m_playerHand;
    [SerializeField] private CPUHand m_cpuHand;

    [SerializeField] private InputField m_betField;

    [SerializeField] private GameProgressViewer m_gameProgressViewer;

    public static int ChangeCount = 1;

    private enum GameState
    {
        Invalid = -1,
        Init,
        Deal,
        Change,

        Bet,

        judge,
        Result
    }

    public int PlayerCoin = 100;
    public int CPUCoin = 100;
    public int BetCoin = 0;
    public bool PlayerWin = false;
    private float m_resultViewTime = 5.0f;

    private GameState m_gameState = GameState.Invalid;

    void Update()
    {
        switch (m_gameState)
        {
            case GameState.Invalid: // ゲーム前
                m_gameState = GameState.Init;
                break;
            case GameState.Init:　//　初期化

                m_resultViewTime = 5.0f;

                m_betField.enabled = false;

                m_gameState = GameState.Deal;
                break;
            case GameState.Deal: //　カードをディールする

                PlayerCoin -= 5;
                CPUCoin -= 5;

                m_playerHand.PlayerCardDeal();
                m_cpuHand.CPUCardDeal();

                m_gameState = GameState.Change;
                break;
            case GameState.Change: //　プレイヤーは1回だけ(今は)カードをチェンジする

                m_gameProgressViewer.SetGameChangeText();

                if (ChangeCount < 1)
                {
                    m_gameState = GameState.Bet;
                }
                break;

            case GameState.Bet:

                m_gameProgressViewer.SetGameBetText();

                m_betField.enabled = true;

                break;

            case GameState.judge: //チェンジし終えたらジャッジ]

                m_cpuHand.CPUCardShowDown();
               
                if (m_playerHand.PlayerJudgeHand > m_cpuHand.CPUJudgeHand)
                {
                    PlayerWin = true;
                    PlayerCoin += BetCoin;
                    CPUCoin -= BetCoin;
                }
                else if (m_cpuHand.CPUJudgeHand > m_playerHand.PlayerJudgeHand)
                {
                    PlayerWin = false;
                    PlayerCoin -= BetCoin;
                    CPUCoin += BetCoin;
                }
                else
                {
                    //役が同じだった場合
                    if(m_playerHand.PlayerHighCardNumber > m_cpuHand.CPUHightCardNumber)
                    {
                        PlayerWin = true;
                        PlayerCoin += BetCoin;
                        CPUCoin -= BetCoin;
                    }
                    else
                    {
                        PlayerWin = false;
                        PlayerCoin -= BetCoin;
                        CPUCoin += BetCoin;
                    }
                }


                m_gameProgressViewer.SetGameJudgeText(PlayerWin, m_playerHand.PlayerJudgeHand, m_cpuHand.CPUJudgeHand);

                ChangeCount++;

                BetCoin = 0;

                m_betField.text = string.Empty;

                m_gameState = GameState.Result;
                break;
            case GameState.Result: //結果表示などに使う

                m_resultViewTime -= Time.deltaTime;

                if (m_resultViewTime < 0)
                {
                    m_gameState = GameState.Init;
                }
                break;
        }
    }

    public void BetInput()
    {
        BetCoin = int.Parse(m_betField.text);
        m_gameState = GameState.judge;
    }
}
