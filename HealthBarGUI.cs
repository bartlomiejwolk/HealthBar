// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the HealthBar extension for Unity. Licensed under the
// MIT license. See LICENSE file in the project root folder. Based on HealthBar
// component made by Zero3Growlithe.

using UnityEngine;

namespace HealthBarEx {

    [System.Serializable]
    // todo encapsulate fields
    public class HealthBarGUI {

        public Color addedHealth;

        [HideInInspector]
        public float alpha;

        public float animationSpeed = 3f;
        public Color availableHealth;
        public Color displayedValue;
        public bool displayValue = true;
        public Color drainedHealth;
        public int height = 30;
        public Vector2 offset = new Vector2(0, 30);
        public PositionModes positionMode;
        public GUIStyle textStyle;
        public Texture texture;
        public float transitionDelay = 3f;
        public float transitionSpeed = 5f;
        public Vector2 valueOffset = new Vector2(0, 30);
        public float visibility = 1;
        public int width = 100;

        public enum PositionModes {

            Fixed,
            Center

        };

    }

}