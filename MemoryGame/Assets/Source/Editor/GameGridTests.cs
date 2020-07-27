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
using NUnit.Framework;



namespace MemoryGame.Tests
{
    /// <summary>
    /// GameGridTests class.
    /// </summary>
    public class GameGridTests
    {
        [Test]
        public static void AllCardsHaveMatch()
        {
            var randomGrid = GameGridModel.GenerateRandomGrid();
            List<Card> cardList = randomGrid.cards;
            Assert.IsTrue(cardList.Count % 2 == 0);
            
            int cycles = cardList.Count / 2;
            for(int i = 0; i < cycles; i++)
            {
                Card card = cardList[0];
                cardList.RemoveAt(0);
                Card match = cardList.Find( x => (x.content == card.content));
                Assert.IsNotNull(match);
                Assert.IsTrue(cardList.Remove(match));
            }
            Assert.IsTrue(cardList.Count == 0);
        }


        [Test]
        public static void MatchingCardsEliminated()
        {
            var randomGrid = GameGridModel.GenerateRandomGrid();
            randomGrid.ResetCardStates();
            Card selectedCard = randomGrid.cards[0];
            List<Card> matchingCards = randomGrid.cards.FindAll( x => (x.content == selectedCard.content));
            for(int i = 0; i < matchingCards.Count; i++)
            {
                matchingCards[i].state = ECardState.FaceUp;
            }

            GameGridModel.UpdateGameGrid(randomGrid);

            for(int i = 0; i < matchingCards.Count; i++)
            {
                Assert.IsTrue(matchingCards[i].state == ECardState.Eliminated);
            }
        }


        [Test]
        public static void NonMatchingCardsFlipDown()
        {
            var randomGrid = GameGridModel.GenerateRandomGrid();
            randomGrid.ResetCardStates();
            Card selectedCard = randomGrid.cards[0];
            Card nonMatchingCard = randomGrid.cards.Find( x => (x.content != selectedCard.content));
            selectedCard.state = ECardState.FaceUp;
            nonMatchingCard.state = ECardState.FaceUp;
            GameGridModel.UpdateGameGrid(randomGrid);

            Assert.IsTrue(selectedCard.state == ECardState.FaceDown);
            Assert.IsTrue(nonMatchingCard.state == ECardState.FaceDown);
        }


        [Test]
        public static void SingleCardStaysFaceUp()
        {
            var randomGrid = GameGridModel.GenerateRandomGrid();
            randomGrid.ResetCardStates();
            Card selectedCard = randomGrid.cards[0];
            selectedCard.state = ECardState.FaceUp;
            GameGridModel.UpdateGameGrid(randomGrid);
            Assert.IsTrue(selectedCard.state == ECardState.FaceUp);
        }


        [Test]
        public static void MatchingAllCards()
        {
            var randomGrid = GameGridModel.GenerateRandomGrid();
            randomGrid.ResetCardStates();
            Assert.IsTrue(randomGrid.cards.Count % 2 == 0);
            
            int cycles = randomGrid.cards.Count / 2;
            for(int i = 0; i < cycles; i++)
            {
                Card selectedCard = randomGrid.cards.Find( x => (x.state != ECardState.Eliminated));
                Assert.IsNotNull(selectedCard);
                List<Card> match = randomGrid.cards.FindAll( x => (x.content == selectedCard.content));
                Assert.IsTrue(match != null && match.Count == 2);
                match[0].state = ECardState.FaceUp;
                match[1].state = ECardState.FaceUp;
                GameGridModel.UpdateGameGrid(randomGrid);
                Assert.IsTrue(match[0].state == ECardState.Eliminated);
                Assert.IsTrue(match[1].state == ECardState.Eliminated);
            }

            for(int i = 0; i < randomGrid.cards.Count; i++)
            {
                Assert.IsTrue(randomGrid.cards[i].state == ECardState.Eliminated);
            }
        }
    }
}