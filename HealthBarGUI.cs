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
        public Texture texture;
        public Color addedHealth;
        public Color availableHealth;
        public Color displayedValue;
        public Color drainedHealth;
        public float animationSpeed = 3f;
        public float transitionSpeed = 5f;
        public float transitionDelay = 3f;
        public float visibility = 1;
        public int width = 100;
        public int height = 30;
        public Vector2 offset = new Vector2(0, 30);
        public Vector2 valueOffset = new Vector2(0, 30);
        public GUIStyle textStyle;

        public enum PositionModes {

            Fixed,
            Center

        };

    }

}