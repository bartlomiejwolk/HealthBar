using UnityEngine;
using System.Collections;

namespace HealthBarEx {

    public class HealthBar : MonoBehaviour {

        #region CONSTANTS

        public const string Version = "v0.1.0";
        public const string Extension = "Healthbar";

        #endregion
        
        #region FIELDS
        public float healthValue;
        private Coroutine displayHealthBar;
        private float previousValue = 100;
        private float currentValue = 100;
        private float health = 100;
        private float maxHealth = 100;

        /// <summary>
        /// Allows identify component in the scene file when reading it with
        /// text editor.
        /// </summary>
#pragma warning disable 0414
        [SerializeField]
        private string componentName = "MyClass";
#pragma warning restore0414

        [SerializeField]
        private string description = "Description";
  
        #endregion

        #region INSPECTOR FIELDS
        // todo make all private
        public HealthBarGUI healthBarGUI;
        // Offset transform pivot point.
        //
        // Use it if health bar position floats away from the transform.
        public Vector3 targetOffset = new Vector3(0, 2.2f, 0);

        // the camera used to track health bar size
        public Transform cameraTracker;
        public float refRelativeDist = 5;
        #endregion

        #region PROPERTIES

        public float HealthValue {
            get { return healthValue; }
            set { healthValue = value; }
        }

        public HealthBarGUI HealthBarGui {
            get { return healthBarGUI; }
            set { healthBarGUI = value; }
        }

        public Vector3 TargetOffset {
            get { return targetOffset; }
            set { targetOffset = value; }
        }

        public Transform CameraTracker {
            get { return cameraTracker; }
            set { cameraTracker = value; }
        }

        public float RefRelativeDist {
            get { return refRelativeDist; }
            set { refRelativeDist = value; }
        }

        public string Description {
            get { return description; }
            set { description = value; }
        }

        #endregion

        #region UNITY MESSAGES
        void OnGUI() {
            if (!HealthBarGui.texture) {
                Debug.Log("ERROR: Health bar texture has not been assigned!");
                return;
            }
            /*if (healthBarGUI.texture looks shitty){
                Debug.Log("ERROR: Your fucking health bar texture is disgusting, go fuck yourself");
            }*/
            if (displayHealthBar != null) {
                Vector2 barPos =
                    Camera.main.WorldToScreenPoint(
                            transform.position +
                            TargetOffset);
                DrawHealthBar(barPos + HealthBarGui.offset);
            }
        }


        private void Start () {
            // todo remove health field
            health = HealthValue;
            currentValue = health;
            maxHealth = health;
        }

        private void Update () {
            health = HealthValue;
            HealthBarController();
        }

        #endregion
        #region METHODS
        void HealthBarController (){
            // if there was a change in health, display the health bar
            if (previousValue != health){
                if (displayHealthBar != null) {
                    StopAllCoroutines();
                }
                displayHealthBar = StartCoroutine(DisplayHealthBar(health));
            }
            previousValue = health;
        }
        IEnumerator DisplayHealthBar (float target){
            float timer = 0f;
            HealthBarGui.alpha = HealthBarGui.visibility;
            while (HealthBarGui.alpha > 0f){
                if (timer > HealthBarGui.transitionDelay){
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
        
        private void DrawHealthBar (Vector2 barPos) {
        // scale the gui if camera is available
            float GUIScale = 0;
            if (CameraTracker != null){
                float cameraDist = (CameraTracker.position-transform.position).magnitude;
                GUIScale = RefRelativeDist/Cut(cameraDist, 0.001f, Mathf.Infinity);
            } else
                GUIScale = 1;
        // Holds health bar lerped value.
            currentValue = Mathf.Lerp(
                currentValue,
                Cut(health, 0, Mathf.Infinity),
                Time.deltaTime * HealthBarGui.animationSpeed);
            // Destination.
            float barWidth =
                (HealthBarGui.width * GUIScale) * (Cut(health, 0, Mathf.Infinity) / maxHealth);
            // Learped value.
            float lerpBarWidth =
                (HealthBarGui.width * GUIScale) * (currentValue / maxHealth);
        // switch the position modes
            float horizPos_available = 0f;
            float horizPos_variable = 0f;
            switch (HealthBarGui.positionMode){
                case HealthBarGUI.PositionModes.Fixed:
                    // White.
                    horizPos_available =
                        barPos.x + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2);
                    // Red.
                    horizPos_variable =
                        barPos.x + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2) + barWidth;
                break;
                case HealthBarGUI.PositionModes.Center:
                    horizPos_available =
                        barPos.x + HealthBarGui.offset.x * GUIScale - (barWidth * 0.5f);
                    horizPos_variable =
                        barPos.x + HealthBarGui.offset.x * GUIScale - (barWidth * 0.5f) + barWidth;
                break;
            }
        // draw health bar background on fixed position
            if (HealthBarGui.positionMode == HealthBarGUI.PositionModes.Fixed){
                GUI.color = new Color(0.8f, 0.8f, 0.8f, HealthBarGui.alpha * 0.5f);
                GUI.DrawTexture(
                    new Rect(
                        barPos.x + HealthBarGui.offset.x * Mathf.Pow(GUIScale, 2),
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
                new Color(GUI.color.r, GUI.color.g, GUI.color.b, HealthBarGui.alpha);
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
                new Color(GUI.color.r, GUI.color.g, GUI.color.b, HealthBarGui.alpha);
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
            if (HealthBarGui.displayValue){
                GUI.color = HealthBarGui.displayedValue;
                GUI.color =
                    new Color(GUI.color.r, GUI.color.g, GUI.color.b, HealthBarGui.alpha);
                GUI.Label(
                    new Rect(
                        horizPos_available + HealthBarGui.valueOffset.x, 
                        Screen.height - barPos.y - 48 +
                        HealthBarGui.offset.y - HealthBarGui.valueOffset.y - HealthBarGui.height,
                        30,
                        20), 
                    currentValue.ToString("#"),
                    HealthBarGui.textStyle);
            }
        }

        private float Cut (float input, float min, float max){
            return Mathf.Clamp(input, min, max);
        }
        #endregion
    }

}
