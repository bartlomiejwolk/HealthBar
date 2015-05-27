// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the HealthBar extension for Unity. Licensed under the
// MIT license. See LICENSE file in the project root folder. Based on HealthBar
// component made by Zero3Growlithe (zero3growlithe@gmail.com).

using UnityEngine;

namespace HealthBarEx {

    [System.Serializable]
    // todo encapsulate fields
    public class HealthBarGUI {

        [HideInInspector]
        public float alpha;

        public bool displayValue = true;
        public PositionModes positionMode;
        /// <summary>
        /// Put here white texture.
        /// </summary>
        public Texture texture;
        public Color addedHealth = Color.green;
        public Color availableHealth = Color.white;
        public Color displayedValue = Color.white;
        public Color drainedHealth = Color.red;
        public float animationSpeed = 3f;
        public float transitionSpeed = 2f;
        public float transitionDelay = 3f;
        public float visibility = 1;
        public int width = 120;
        public int height = 16;
        public Vector2 offset = new Vector2(0, 0);
        public Vector2 valueOffset = new Vector2(0, -46);
        public GUIStyle textStyle;

        public enum PositionModes {

            Fixed,
            Center

        };

    }

}