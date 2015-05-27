using UnityEngine;
using System.Collections;

namespace HealthbarEx {

    public class HealthBar : MonoBehaviour {

        public float healthValue;
        public HealthBarGUI healthBarGUI;
        private float health = 100;
        private float maxHealth = 100;
        // Offset transform pivot point.
        //
        // Use it if health bar position floats away from the transform.
        public Vector3 targetOffset = new Vector3(0, 2.2f, 0);

        // the camera used to track health bar size
        public Transform cameraTracker;
        public float refRelativeDist = 5;

        private void Start () {
            // todo remove health field
            health = healthValue;
            currentValue = health;
            maxHealth = health;
        }

        private void Update () {
            health = healthValue;
            HealthBarController();
        }

        private float currentValue = 100;
        private float previousValue = 100;
        private Coroutine displayHealthBar;
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

        void OnGUI (){
            if (!healthBarGUI.texture){
                Debug.Log("ERROR: Health bar texture has not been assigned!");
                return;
            }
            /*if (healthBarGUI.texture looks shitty){
                Debug.Log("ERROR: Your fucking health bar texture is disgusting, go fuck yourself");
            }*/
            if (displayHealthBar != null){
                Vector2 barPos = 
                    Camera.main.WorldToScreenPoint(
                            transform.position +
                            targetOffset);
                DrawHealthBar(barPos + healthBarGUI.offset);
            }
        }

        IEnumerator DisplayHealthBar (float target){
            float timer = 0f;
            healthBarGUI.alpha = healthBarGUI.visibility;
            while (healthBarGUI.alpha > 0f){
                if (timer > healthBarGUI.transitionDelay){
                    healthBarGUI.alpha = Mathf.MoveTowards(
                        healthBarGUI.alpha,
                        0f,
                        Time.deltaTime * healthBarGUI.transitionSpeed);
                }
                timer += Time.deltaTime;
                yield return null;
            }
            displayHealthBar = null;
        }
        
        private void DrawHealthBar (Vector2 barPos) {
        // scale the gui if camera is available
            float GUIScale = 0;
            if (cameraTracker != null){
                float cameraDist = (cameraTracker.position-transform.position).magnitude;
                GUIScale = refRelativeDist/Cut(cameraDist, 0.001f, Mathf.Infinity);
            } else
                GUIScale = 1;
        // Holds health bar lerped value.
            currentValue = Mathf.Lerp(
                currentValue,
                Cut(health, 0, Mathf.Infinity),
                Time.deltaTime * healthBarGUI.animationSpeed);
            // Destination.
            float barWidth =
                (healthBarGUI.width * GUIScale) * (Cut(health, 0, Mathf.Infinity) / maxHealth);
            // Learped value.
            float lerpBarWidth =
                (healthBarGUI.width * GUIScale) * (currentValue / maxHealth);
        // switch the position modes
            float horizPos_available = 0f;
            float horizPos_variable = 0f;
            switch (healthBarGUI.positionMode){
                case HealthBarGUI.PositionModes.Fixed:
                    // White.
                    horizPos_available =
                        barPos.x + healthBarGUI.offset.x * Mathf.Pow(GUIScale, 2);
                    // Red.
                    horizPos_variable =
                        barPos.x + healthBarGUI.offset.x * Mathf.Pow(GUIScale, 2) + barWidth;
                break;
                case HealthBarGUI.PositionModes.Center:
                    horizPos_available =
                        barPos.x + healthBarGUI.offset.x * GUIScale - (barWidth * 0.5f);
                    horizPos_variable =
                        barPos.x + healthBarGUI.offset.x * GUIScale - (barWidth * 0.5f) + barWidth;
                break;
            }
        // draw health bar background on fixed position
            if (healthBarGUI.positionMode == HealthBarGUI.PositionModes.Fixed){
                GUI.color = new Color(0.8f, 0.8f, 0.8f, healthBarGUI.alpha * 0.5f);
                GUI.DrawTexture(
                    new Rect(
                        barPos.x + healthBarGUI.offset.x * Mathf.Pow(GUIScale, 2),
                        Screen.height - barPos.y - healthBarGUI.offset.y,
                        healthBarGUI.width * GUIScale,
                        healthBarGUI.height * GUIScale),
                    healthBarGUI.texture, 
                    ScaleMode.StretchToFill,
                    true);
            }
        // Draw the "current health" part of the health bar
            GUI.color = healthBarGUI.availableHealth;
            GUI.color =
                new Color(GUI.color.r, GUI.color.g, GUI.color.b, healthBarGUI.alpha);
            GUI.DrawTexture(
                new Rect(
                    horizPos_available,
                    Screen.height - barPos.y - healthBarGUI.offset.y,
                    barWidth,
                    healthBarGUI.height * GUIScale),
                healthBarGUI.texture, 
                ScaleMode.StretchToFill,
                true);
        // Draw the animated "decrease" part of the health bar
            if (currentValue > previousValue)
                GUI.color = healthBarGUI.drainedHealth;
            else
                GUI.color = healthBarGUI.addedHealth;
            GUI.color =
                new Color(GUI.color.r, GUI.color.g, GUI.color.b, healthBarGUI.alpha);
            GUI.DrawTexture(
                new Rect(
                    horizPos_variable,
                    Screen.height - barPos.y - healthBarGUI.offset.y,
                    lerpBarWidth - barWidth,
                    healthBarGUI.height * GUIScale),
                healthBarGUI.texture, 
                ScaleMode.StretchToFill, 
                true);
        // Draw the value
            if (healthBarGUI.displayValue){
                GUI.color = healthBarGUI.displayedValue;
                GUI.color =
                    new Color(GUI.color.r, GUI.color.g, GUI.color.b, healthBarGUI.alpha);
                GUI.Label(
                    new Rect(
                        horizPos_available + healthBarGUI.valueOffset.x, 
                        Screen.height - barPos.y - 48 +
                        healthBarGUI.offset.y - healthBarGUI.valueOffset.y - healthBarGUI.height,
                        30,
                        20), 
                    currentValue.ToString("#"),
                    healthBarGUI.textStyle);
            }
        }

        private float Cut (float input, float min, float max){
            return Mathf.Clamp(input, min, max);
        }
    }

}
