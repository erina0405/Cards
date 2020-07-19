﻿
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private CardDealer m_cardDealer = null;
    [SerializeField] private List<RectTransform> m_playerCards = new List<RectTransform>();
    private List<Card> m_playerHand = new List<Card>();

    private bool[] m_changeChoice = new bool[5] { false, false, false, false, false };

    public PokerHand.Hand PlayerJudgeHand = PokerHand.Hand.None;

    private void Start()
    {
        m_cardDealer.CardDeal(m_playerHand);
        CardUpDate();

        for (int i = 0; i < m_playerCards.Count; i++)
        {
            var count = i;
            //クリックしたときに何をするか
            m_playerCards[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                CardChangeChoise(count);
            });
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < m_changeChoice.Length; i++)
            {
                if (m_changeChoice[i] == true)
                {
                    m_cardDealer.CardChange(m_playerHand, i);
                }
            }

            
            CardUpDate();

            var selectAllFalse = m_changeChoice.Select(s => s = false).ToArray();
            m_changeChoice = selectAllFalse;

            PlayerJudgeHand = PokerHand.CardHand(m_playerHand);

            PokerFacilitator.ChangeCount--;
        }

    }

    private void CardUpDate()
    {
        for (int i =0; i < m_playerHand.Count; i++)
        {
            var card = m_playerHand[i];
            m_playerCards[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{card.CardSuit}:{card.Number}";
        }
    }

    private void CardChangeChoise(int _choice)
    {
        m_changeChoice[_choice] = true;
    }

    public void PlayerCardDeal()
    {
        m_cardDealer.CardDeal(m_playerHand);
        CardUpDate();
    }
}
