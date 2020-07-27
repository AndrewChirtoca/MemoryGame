//===----------------------------------------------------------------------===//
//
//  vim: ft=cs tw=80
//
//  Creator: Chirtoca Andrei <andrewchirtoca@gmail.com>
//
//===----------------------------------------------------------------------===//


using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;



namespace MemoryGame
{
    public enum ECardState
    {
        FaceDown,
        FaceUp,
        Eliminated
    }


    [Serializable]
    public class Card
    {
        public ECardState state;
        public string content;

        public Card(){ }

        public Card(ECardState state, string content)
        {
            this.state = state;
            this.content = content;
        }

        public Card(Card original)
        {
            state = original.state;
            content = original.content;
        }
    }


    /// <summary>
    /// GameGridModel class.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameGrid", menuName = "New Game Grid")]
    public class GameGridModel : ScriptableObject
    {
#region Public serialized variables
        public List<Card> cards;
#endregion



#region Private variables

#endregion



#region Public methods and properties
        [ContextMenu("Reset Card States")]
        public void ResetCardStates()
        {
            for(int i = 0; i < cards.Count; i++)
            {
                cards[i].state = ECardState.FaceDown;
            }
        }


        public static List<int> UpdateGameGrid(GameGridModel grid)
        {
            var cardsFacingUp = grid.cards.FindAll( x => (x.state == ECardState.FaceUp));
            var updatedCardsIndex = new List<int>();

            if(cardsFacingUp.Count < 2)
            {
                cardsFacingUp.Clear();
                return updatedCardsIndex;
            }

            bool match = true;
            for(int i = 0; i < cardsFacingUp.Count - 1; i++)
            {
                if(cardsFacingUp[i].content != cardsFacingUp[i + 1].content)
                {
                    match = false;
                    break;
                }
            }

            for(int i = 0; i < cardsFacingUp.Count; i++)
            {
                cardsFacingUp[i].state = (match) ? ECardState.Eliminated : ECardState.FaceDown;
                updatedCardsIndex.Add(grid.cards.IndexOf(cardsFacingUp[i]));
            }

            return updatedCardsIndex;
        }


        public static GameGridModel GenerateRandomGrid()
        {
            var gameGrid = ScriptableObject.CreateInstance<GameGridModel>();
            int cardPairs = UnityEngine.Random.Range(4, 10);

            var cards = new List<Card>();
            for(int i = 0; i < cardPairs; i++)
            {
                var card1 = new Card(ECardState.FaceDown, i.ToString());
                cards.Add(card1);
                var card2 = new Card(card1);
                cards.Add(card2);
            }
            gameGrid.cards = cards.OrderBy( x => UnityEngine.Random.value).ToList();

            return gameGrid;
        }
#endregion



#region ScriptableObject methods
#endregion
    }
}