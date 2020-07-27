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
using System.Collections;
using UnityEngine.Events;



namespace MemoryGame
{
    /// <summary>
    /// GameGridController class.
    /// </summary>
    public class GameGridController : MonoBehaviour
    {
#region Public serialized variables
        public GameGridView gridView;
        public GameGridModel gridModel;
        public GameSetting setting;
#endregion



#region Private variables
        private UnityEvent _onVictoryConditionMet = new UnityEvent();
        private UnityEvent _onGameTickScheduled = new UnityEvent();
        private UnityEvent _onGameTickPerformed = new UnityEvent();
        private Coroutine _tickRoutine;
        private Timer _timer;
        private int _score;
#endregion



#region Public methods and properties
        [ContextMenu("Setup Board")]
        public void SetupBoard()
        {
            gridView.Clear();
            gridModel.ResetCardStates();

            for(int i = 0; i < gridModel.cards.Count; i++)
            {
                gridView.AddCard(i, gridModel.cards[i].content, new Action<int>(CardClickedHandler));
            }
        }

        [ContextMenu("Clear Board")]
        public void ClearBoard()
        {
            gridView.Clear();
        }
#endregion



#region Monobehavior methods
        private void OnEnable()
        {
            _onGameTickScheduled.AddListener( delegate { gridView.SetCardsInteractable(false);});
            _onGameTickPerformed.AddListener(GameTickHandler);
            _onVictoryConditionMet.AddListener(VictoryConditionMetHandler);
        }

        private void OnDisable()
        {
            _onGameTickScheduled.RemoveAllListeners();
            _onGameTickPerformed.RemoveAllListeners();
            _onVictoryConditionMet.RemoveAllListeners();
        }

        private void Start()
        {
            if(_timer == null)
            {
                _timer = new Timer(this, 1);
                _timer.onCountdownEnd.AddListener(TimerCountdownOverHandler);
                _timer.onTimerTick.AddListener( delegate { gridView.timeCountdown.text = _timer.TimeLeft.ToString();});
            }

            SetupBoard();
            _score = 0;
            _timer.StartCountdown(setting.timeToComplete);

            gridView.scoreValue.text = _score.ToString();
            gridView.timeCountdown.text = setting.timeToComplete.ToString();
        }
#endregion



#region Event handling methods
        private void CardClickedHandler(int index)
        {
            ECardState state = gridModel.cards[index].state;
            if(state == ECardState.Eliminated)
                return;

            var newState = (state == ECardState.FaceDown)
                ? ECardState.FaceUp : ECardState.FaceDown;
            gridModel.cards[index].state = newState;
            gridView.SetCardState(index, newState);

            ScheduleGameTick();
        }

        private void GameTickHandler()
        {
            gridView.SetCardsInteractable(true);
            gridView.scoreValue.text = _score.ToString();
            WinConditionCheck();
        }

        private void VictoryConditionMetHandler()
        {
            _timer.StopCountdown();
            Popup.Instance.ShowPopup("Victory!", "Congratulations! You have Won!", "Play Again", 
            delegate
            {
                gridModel = GameGridModel.GenerateRandomGrid();
                Popup.Instance.HidePopup();
                Start();
            });
        }

        private void TimerCountdownOverHandler()
        {
            gridView.SetCardsInteractable(false);
            Popup.Instance.ShowPopup("Defeat!", "Time is up! You have Lost!", "Try Again", 
            delegate
            {
                Popup.Instance.HidePopup();
                Start();
            });
        }
#endregion


        private void ScheduleGameTick()
        {
            if(_tickRoutine != null)
            {
                StopCoroutine(_tickRoutine);
            }
            _tickRoutine = StartCoroutine(GameTick(setting.timeCardsStayFlipped));
        }


        private IEnumerator GameTick(float waitTime)
        {
            _onGameTickScheduled.Invoke();

            yield return new WaitForSeconds(waitTime);

            List<int> updatedCards = GameGridModel.UpdateGameGrid(gridModel);
            for(int i = 0; i < updatedCards.Count; i++)
            {
                ECardState newState = gridModel.cards[updatedCards[i]].state;
                gridView.SetCardState(updatedCards[i], newState);
                if(newState == ECardState.Eliminated)
                {
                    _score += setting.eliminationScorePoints;
                }
            }

            _onGameTickPerformed.Invoke();
            _tickRoutine = null;
        }


        private void WinConditionCheck()
        {
            for(int i = 0; i < gridModel.cards.Count; i++)
            {
                if(gridModel.cards[i].state != ECardState.Eliminated)
                {
                    return;
                }
            }
            _onVictoryConditionMet.Invoke();
        }
    }
}