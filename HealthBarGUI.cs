// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
//  
// This file is part of the HealthBar extension for Unity.
// Licensed under the MIT license. See LICENSE file in the project root folder.
// Based on HealthBar component made by Zero3Growlithe.

using UnityEngine;

namespace HealthBarEx {

    [System.Serializable]
    public class HealthBarGUI {
        public Texture texture;

        public Color availableHealth;
        public Color drainedHealth;
        public Color addedHealth;
        public Color displayedValue;

        [HideInInspector] public float alpha;
        public float visibility = 1;
        public bool displayValue = true;
        public GUIStyle textStyle;

        public float transitionDelay = 3f;
        public float transitionSpeed = 5f;
        public float animationSpeed = 3f;

        public Vector2 offset = new Vector2(0, 30);
        public Vector2 valueOffset = new Vector2(0, 30);
        public int width = 100;
        public int height = 30;

        public enum PositionModes {Fixed, Center};
        public PositionModes positionMode;
    }

}