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
using UnityEngine.Events;
using UnityEngine.UI;



namespace MemoryGame
{
    /// <summary>
    /// Popup class.
    /// </summary>
    public class Popup : Singleton<Popup>
    {
#region Public serialized variables
#endregion



#region Private variables
        [SerializeField]
        private Transform popupRoot;
        [SerializeField]
        private Text popupHeader;
        [SerializeField]
        private Text popupMessage;
        [SerializeField]
        private Text popupButtonLabel;
        [SerializeField]
        private Button popupButton;
#endregion



#region Public methods and properties
        public void ShowPopup(string header, string message, string buttonLabel, UnityAction onClick)
        {
            popupRoot.gameObject.SetActive(true);
            popupHeader.text = header;
            popupMessage.text = message;
            popupButtonLabel.text = buttonLabel;
            popupButton.onClick.RemoveAllListeners();
            popupButton.onClick.AddListener(onClick);
        }

        public void HidePopup()
        {
            popupRoot.gameObject.SetActive(false);
        }
#endregion



#region Monobehavior methods
        public void Awake()
        {
            RegisterSingleton(this);
        }
#endregion
    }
}