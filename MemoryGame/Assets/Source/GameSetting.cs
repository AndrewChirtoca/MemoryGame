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



namespace MemoryGame
{
    /// <summary>
    /// GameSettings class.
    /// </summary>
    [CreateAssetMenu(fileName = "NewGameSetting", menuName = "New Game Setting")]
    public class GameSetting : ScriptableObject
    {
#region Public serialized variables
        [Range(10, 60)]
        public float timeToComplete;
        [Range(0.3f, 1.0f)]
        public float timeCardsStayFlipped;
        [Range(1, 20)]
        public int eliminationScorePoints;
#endregion
    }
}