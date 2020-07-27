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
using UnityEngine.UI;



namespace MemoryGame
{
    /// <summary>
    /// CardView class.
    /// </summary>
    public class CardView : MonoBehaviour
    {
#region Public serialized variables
        public Text textField;
        public Image image;
        public Button button;
        public Transform animBase;
        public Color faceUpTint;
#endregion



#region Private variables
        private Color _defaultTint;
#endregion



#region Public methods and properties
        public void SetState(ECardState state)
        {
            if(state == ECardState.Eliminated)
            {
                VanishCard();
            }
            else
            {
                FlipCard(state);
            }
        }
#endregion



#region Monobehavior methods
        private void Awake()
        {
            _defaultTint = image.color;
        }
#endregion


        private void FlipCard(ECardState state, float animTime = 0.1f)
        {
            StartCoroutine(FlipCardRoutine(state, animTime));
        }

        private IEnumerator FlipCardRoutine(ECardState state, float animTime)
        {
            button.enabled = false;
            float halfEndRotY = animBase.rotation.eulerAngles.y + 90;
            float fullEndRotY = animBase.rotation.eulerAngles.y + 180;
            float halfTotalTime = animTime / 2;
            float fullTotalTime = animTime;
            float timeStep = fullTotalTime / 10;
            float rotX = animBase.rotation.eulerAngles.x;
            float rotZ = animBase.rotation.eulerAngles.z;
            var halfRot = Quaternion.Euler(rotX, halfEndRotY, rotZ);
            var fullRot = Quaternion.Euler(rotX, fullEndRotY, rotZ);
            var startRot = animBase.rotation;
            var waitForTimeStep = new WaitForSeconds(timeStep);

            float t = 0;
            while(t / halfTotalTime <= 1.0f)
            {
                t += timeStep;
                animBase.rotation = Quaternion.Lerp(startRot, halfRot, t / halfTotalTime);
                yield return waitForTimeStep;
            }
            textField.gameObject.SetActive((state == ECardState.FaceUp) ? true : false);
            image.color = (state == ECardState.FaceUp) ? faceUpTint : _defaultTint;
            while(t / fullTotalTime <= 1.0f)
            {
                t += timeStep;
                animBase.rotation = Quaternion.Lerp(startRot, fullRot, t / fullTotalTime);
                yield return waitForTimeStep;
            }
            button.enabled = true;
        }

        private void VanishCard(float animTime = 0.1f)
        {
            StartCoroutine(VanishCardRoutine(animTime));
        }

        private IEnumerator VanishCardRoutine(float animTime)
        {
            button.enabled = false;

            float totalTime = animTime;
            float timeStep = totalTime / 10;
            Vector3 startScale = animBase.localScale;
            var waitForTimeStep = new WaitForSeconds(timeStep);

            float t = 0;
            while(t / totalTime <= 1.0f)
            {
                t += timeStep;
                animBase.localScale = Vector3.Lerp(startScale, Vector3.zero, t / totalTime);
                yield return waitForTimeStep;
            }
        }
    }
}