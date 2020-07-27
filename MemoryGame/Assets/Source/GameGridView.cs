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
using UnityEngine.UI;
using UnityEngine.Events;



namespace MemoryGame
{
    /// <summary>
    /// GameGridView class.
    /// </summary>
    public class GameGridView : MonoBehaviour
    {
#region Public serialized variables
        public CardView cardPrefab;
        public GridLayoutGroup layoutGroup;
        public Text timeCountdown;
        public Text scoreValue;
#endregion



#region Private variables
        private Dictionary<int, CardView> _cards = new Dictionary<int, CardView>();
#endregion



#region Public methods and properties
        public void Clear()
        {
            for(int i = 0; i < _cards.Count; i++)
            {
                DestroyImmediate(_cards[i].gameObject);
            }
            _cards.Clear();
        }

        public void AddCard(int index, string content, Action<int> onClick)
        {
            var newCardGO = Instantiate(cardPrefab.gameObject, layoutGroup.transform);
            var newCardView = newCardGO.GetComponent<CardView>();
            newCardView.button.onClick.AddListener( delegate { onClick(index);});
            newCardView.textField.text = content;
            _cards.Add(index, newCardView);
        }

        public void SetCardState(int index, ECardState state)
        {
            var card = _cards[index];
            card.SetState(state);
        }

        public void SetCardsInteractable(bool interactable)
        {
            for(int i = 0; i < _cards.Count; i++)
            {
                _cards[i].button.interactable = interactable;
            }
        }
#endregion



#region Monobehavior methods
#endregion
    }
}