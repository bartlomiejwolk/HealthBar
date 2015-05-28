// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the HealthBar extension for Unity. Licensed under the
// MIT license. See LICENSE file in the project root folder. Based on HealthBar
// component made by Zero3Growlithe (zero3growlithe@gmail.com).

using System.Collections;
using UnityEngine;

namespace HealthBarEx {

    public class HealthBar : MonoBehaviour {
        #region CONSTANTS

        public const string Extension = "Healthbar";
        public const string Version = "v0.1.0";

        #endregion CONSTANTS

        #region FIELDS

#pragma warning disable 0414
        /// <summary>
        ///     Allows identify component in the scene file when reading it with
        ///     text editor.
        /// </summary>
        [SerializeField]
        private string componentName = "HealthBar";
#pragma warning restore0414

        private float currentValue = 100;

        [SerializeField]
        private string description = "Description";

        private Coroutine displayHealthBar;
        private int health = 100;
        private float maxHealth = 100;
        private float previousValue = 100;

        #endregion FIELDS

        #region INSPECTOR FIELDS

        // the camera used to track health bar size
        [SerializeField]
        private Transform cameraTracker;

        // todo make all private
        [SerializeField]
        private HealthBarGUI healthBarGUI;

        [SerializeField]
        private float refRelativeDist = 8;

        // Offset transform pivot point.
        // 
        // Use it if health bar position floats away from the transform.
        [SerializeField]
        private Vector3 targetOffset = new Vector3(0, 2.2f, 0);

        /// <summary>
        /// Force health bar to be visible all the time.
        /// </summary>
        [SerializeField]
        private bool forceDisplay;

        #endregion INSPECTOR FIELDS

        #region PROPERTIES

        public Transform CameraTracker {
            get { return cameraTracker; }
            set { cameraTracker = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        public HealthBarGUI HealthBarGui {
            get { return healthBarGUI; }
            set { healthBarGUI = value; }
        }

        public float RefRelativeDist {
            get { return refRelativeDist; }
            set { refRelativeDist = value; }
        }

        public Vector3 TargetOffset {
            get { return targetOffset; }
            set { targetOffset = value; }
        }

        public int Health {
            get { return health; }
            set { health = value; }
        }

        public bool ForceDisplay {
            get { return forceDisplay; }
            set { forceDisplay = value; }
        }

        #endregion PROPERTIES

        #region UNITY MESSAGES

        private void OnGUI() {
            if (!HealthBarGui.texture) {
                Debug.Log("ERROR: Health bar texture has not been assigned!");
                return;
            }

            /*if (healthBarGUI.texture looks shitty){
                Debug.Log("ERROR: Your fucking health bar texture is disgusting, go fuck yourself");
            }*/

            // Return if health bar should not be drawn.
            if (displayHealthBar == null
                && !ForceDisplay) return;

            // Health bar coroutine is running - draw health bar.
            if (displayHealthBar != null) {
                DrawHealthBar();
            }
            else {
                // Health bar coroutine is not running but force display option
                // is checked - start the coroutine.
                displayHealthBar = StartCoroutine(DisplayHealthBar(Health));
            }
        }

        private void Start() {
            currentValue = Health;
            maxHealth = Health;
        }

        private void Update() {
            HealthBarController();
        }

        #endregion UNITY MESSAGES

        #region METHODS

        private float Cut(float input, float min, float max) {
            return Mathf.Clamp(input, min, max);
        }

        private IEnumerator DisplayHealthBar(float target) {
            var timer = 0f;
            HealthBarGui.alpha = HealthBarGui.visibility;
            while (HealthBarGui.alpha > 0f) {
                if (timer > HealthBarGui.transitionDelay) {
                    HealthBarGui.alpha = Mathf.MoveTowards(
                        HealthBarGui.alpha,
                        0f,
                        Time.deltaTime * HealthBarGui.transitionSpeed);
                }
                timer += Time.deltaTime;
                yield return null;
            }
            displayHealthBar = null;
        }

        private void DrawHealthBar() {
            Vector2 barPos =
                Camera.main.WorldToScreenPoint(
                    transform.position +
                    TargetOffset);

            barPos += HealthBarGui.offset;

            // scale the gui if camera is available
            float GUIScale;
            if (CameraTracker != null) {
                var cameraDist =
                    (CameraTracker.position - transform.position).magnitude;
                GUIScale = RefRelativeDist
                           / Cut(cameraDist, 0.001f, Mathf.Infinity);
            }
            else
                GUIScale = 1;
            // Holds health bar lerped value.
            currentValue = Mathf.Lerp(
                currentValue,
                Cut(Health, 0, Mathf.Infinity),
                Time.deltaTime * HealthBarGui.animationSpeed);
            // Destination.
            var barWidth =
                (HealthBarGui.width * GUIScale)
                * (Cut(Health, 0, Mathf.Infinity) / maxHealth);
            // Learped value.
            var lerpBarWidth =
                (HealthBarGui.width * GUIScale) * (currentValue / maxHealth);
            // switch the position modes
            var horizPos_available = 0f;
            var horizPos_variable = 0f;
            switch (HealthBarGui.positionMode) {
                case HealthBarGUI.PositionModes.Fixed:
                    // White.
                    horizPos_available =
                        barPos.x
                        + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2);
                    // Red.
                    horizPos_variable =
                        barPos.x
                        + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2)
                        + barWidth;
                    break;

                case HealthBarGUI.PositionModes.Center:
                    horizPos_available =
                        barPos.x + HealthBarGui.offset.x * GUIScale
                        - (barWidth * 0.5f);
                    horizPos_variable =
                        barPos.x + HealthBarGui.offset.x * GUIScale
                        - (barWidth * 0.5f) + barWidth;
                    break;
            }
            // draw health bar background on fixed position
            if (HealthBarGui.positionMode == HealthBarGUI.PositionModes.Fixed) {
                GUI.color = new Color(
                    0.8f,
                    0.8f,
                    0.8f,
                    HealthBarGui.alpha * 0.5f);
                GUI.DrawTexture(
                    new Rect(
                        barPos.x
                        + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2),
                        Screen.height - barPos.y - HealthBarGui.offset.y,
                        HealthBarGui.width * GUIScale,
                        HealthBarGui.height * GUIScale),
                    HealthBarGui.texture,
                    ScaleMode.StretchToFill,
                    true);
            }
            // Draw the "current health" part of the health bar
            GUI.color = HealthBarGui.availableHealth;
            GUI.color =
                new Color(
                    GUI.color.r,
                    GUI.color.g,
                    GUI.color.b,
                    HealthBarGui.alpha);
            GUI.DrawTexture(
                new Rect(
                    horizPos_available,
                    Screen.height - barPos.y - HealthBarGui.offset.y,
                    barWidth,
                    HealthBarGui.height * GUIScale),
                HealthBarGui.texture,
                ScaleMode.StretchToFill,
                true);
            // Draw the animated "decrease" part of the health bar
            if (currentValue > previousValue)
                GUI.color = HealthBarGui.drainedHealth;
            else
                GUI.color = HealthBarGui.addedHealth;
            GUI.color =
                new Color(
                    GUI.color.r,
                    GUI.color.g,
                    GUI.color.b,
                    HealthBarGui.alpha);
            GUI.DrawTexture(
                new Rect(
                    horizPos_variable,
                    Screen.height - barPos.y - HealthBarGui.offset.y,
                    lerpBarWidth - barWidth,
                    HealthBarGui.height * GUIScale),
                HealthBarGui.texture,
                ScaleMode.StretchToFill,
                true);
            // Draw the value
            if (HealthBarGui.displayValue) {
                GUI.color = HealthBarGui.displayedValue;
                GUI.color =
                    new Color(
                        GUI.color.r,
                        GUI.color.g,
                        GUI.color.b,
                        HealthBarGui.alpha);
                GUI.Label(
                    new Rect(
                        horizPos_available + HealthBarGui.valueOffset.x,
                        Screen.height - barPos.y - 48 +
                        HealthBarGui.offset.y - HealthBarGui.valueOffset.y
                        - HealthBarGui.height,
                        30,
                        20),
                    currentValue.ToString("#"),
                    HealthBarGui.textStyle);
            }
        }

        private void HealthBarController() {
            // if there was a change in health, display the health bar todo
            // compare with FloatsEqual()
            if (previousValue != Health) {
                if (displayHealthBar != null) {
                    StopAllCoroutines();
                }
                displayHealthBar = StartCoroutine(DisplayHealthBar(Health));
            }
            previousValue = Health;
        }

        /// <summary>
        /// Sets health value.
        /// </summary>
        /// <param name="newValue"></param>
        public void AssignHealthValue(int newValue) {
            Health = newValue;
        }

        #endregion METHODS
    }

}