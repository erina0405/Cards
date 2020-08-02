using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PokerHand
{
    public enum Hand
    {
        None = -1,       // 無し
        OnePair,         // ワンペア
        TwoPair,         // ツーペア
        ThreeOfKind,     // スリーカード
        Straight,        // ストレート
        Flush,           // フラッシュ
        FullHouse,       // フルハウス
        FourofKind,      // フォーカード
        StraightFlush,   // ストレートフラッシュ
        RoyalFlush       // ロイヤルストレートフラッシュ
    }

    public static int HighCard = 0;

    public static Hand CardHand(List<Card> cards)
    {
        // まず数字をソートします(1,2,3,4,5等)
        cards.Sort((a, b) => a.Number - b.Number);

        var kinds = 0;
        var cardsElement = 0;

        foreach (var card in cards.ToLookup(s => s.Number))
        {
            if (card.Count() > 1)
            {
                cardsElement++;
                kinds += card.Count();
                HighCard = card.FirstOrDefault().Number;
            }
        }
        //フラッシュの確認 Suitが全て同じか
        var clubRoyal = cards.TrueForAll(s => s.CardSuit == Card.Suit.Club);
        var diaRoyal = cards.TrueForAll(s => s.CardSuit == Card.Suit.Dia);
        var heartRoyal = cards.TrueForAll(s => s.CardSuit == Card.Suit.Heart);
        var spadeRoyal = cards.TrueForAll(s => s.CardSuit == Card.Suit.Spade);

        var firstCardNo = cards[0].Number;

        #region ロイヤルストレートフラッシュ(1,10,11,12,13) Suitが全て一緒
        if (firstCardNo.Equals(1))
        {
            if (cards[1].Number.Equals(10))
            {
                var count = 0;
                for (int i = 2; i < cards.Count; i++)
                {
                    if (cards[i].Number.Equals(9 + i))
                    {
                        count++;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                //　Straightは確定
                if (count.Equals(3))
                {
                    if (clubRoyal || diaRoyal || heartRoyal || spadeRoyal)
                    {
                        return Hand.RoyalFlush;
                    }
                    else
                    {
                        return Hand.Straight;
                    }
                }
            }
        }
        #endregion

        #region ストレートフラッシュ
        var straightCount = 0;
        for (int i = 1; i < cards.Count; i++)
        {
            var straightCarsNo = firstCardNo;
            if (cards[i].Number.Equals(straightCarsNo + i))
            {
                straightCount++;
                continue;
            }
            else
            {
                break;
            }
        }
        if (straightCount.Equals(4))
        {
            // 上のboolのいずれかがtrueだった場合はStraightFlush
            if (clubRoyal || diaRoyal || heartRoyal || spadeRoyal)
            {
                return Hand.StraightFlush;
            }
            else
            {
                return Hand.Straight;
            }
        }
        #endregion

        #region フォーカード

        if (cardsElement.Equals(1) && kinds.Equals(4))
        {
            return Hand.FourofKind;
        }
        #endregion

        #region フルハウス

        if (cardsElement.Equals(2) && kinds.Equals(5))
        {
            return Hand.FullHouse;
        }

        #endregion

        #region フラッシュ
        if (clubRoyal || diaRoyal || heartRoyal || spadeRoyal)
        {
            return Hand.Flush;
        }
        #endregion

        #region ワンペアかツーペアかスリーカード

        if (kinds.Equals(3))
        {
            return Hand.ThreeOfKind;
        }

        if (cardsElement.Equals(2) && kinds.Equals(4))
        {
            return Hand.TwoPair;
        }
        if (kinds.Equals(2))
        {
            return Hand.OnePair;
        }
        #endregion

        HighCard = cards[4].Number;
        return Hand.None;
    }
}

