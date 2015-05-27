// Copyright (c) 2015 Bartlomiej Wolk (bartlomiejwolk@gmail.com)
// 
// This file is part of the HealthBar extension for Unity. Licensed under the
// MIT license. See LICENSE file in the project root folder. Based on HealthBar
// component made by Zero3Growlithe.

using UnityEditor;
using UnityEngine;

namespace HealthBarEx {

    [CustomEditor(typeof (HealthBar))]
    [CanEditMultipleObjects]
    public sealed class HealthBarEditor : Editor {
        #region FIELDS

        private HealthBar Script { get; set; }

        #endregion FIELDS

        #region SERIALIZED PROPERTIES

        private SerializedProperty cameraTracker;
        private SerializedProperty description;
        private SerializedProperty healthBarGUI;
        private SerializedProperty refRelativeDist;
        private SerializedProperty targetOffset;

        #endregion SERIALIZED PROPERTIES

        #region UNITY MESSAGES

        public override void OnInspectorGUI() {
            serializedObject.Update();

            DrawVersionLabel();
            DrawDescriptionTextArea();

            EditorGUILayout.Space();

            DrawCameraField();
            DrawRelativeDistanceField();
            DrawTargetOffsetField();
            DrawHealthBarGUI();

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable() {
            Script = (HealthBar) target;

            healthBarGUI = serializedObject.FindProperty("healthBarGUI");
            targetOffset = serializedObject.FindProperty("targetOffset");
            cameraTracker = serializedObject.FindProperty("cameraTracker");
            refRelativeDist = serializedObject.FindProperty("refRelativeDist");

            description = serializedObject.FindProperty("description");
        }

        #endregion UNITY MESSAGES

        #region INSPECTOR CONTROLS

        private void DrawCameraField() {
            EditorGUILayout.PropertyField(
                cameraTracker,
                new GUIContent(
                    "Camera",
                    "Camera used to calculate health bar size."));
        }

        private void DrawDescriptionTextArea() {
            description.stringValue = EditorGUILayout.TextArea(
                description.stringValue);
        }

        private void DrawHealthBarGUI() {
            EditorGUILayout.PropertyField(
                healthBarGUI,
                new GUIContent(
                    "Health Bar GUI",
                    ""),
                true);
        }

        private void DrawRelativeDistanceField() {
            EditorGUILayout.PropertyField(
                refRelativeDist,
                new GUIContent(
                    "Relative Distance",
                    ""));
        }

        private void DrawTargetOffsetField() {
            EditorGUILayout.PropertyField(
                targetOffset,
                new GUIContent(
                    "Target Offset",
                    ""));
        }

        private void DrawVersionLabel() {
            EditorGUILayout.LabelField(
                string.Format(
                    "{0} ({1})",
                    HealthBar.Version,
                    HealthBar.Extension));
        }

        #endregion INSPECTOR CONTROLS

        #region METHODS

        [MenuItem("Component/HealthBar")]
        private static void AddEntryToComponentMenu() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(typeof (HealthBar));
            }
        }

        #endregion METHODS
    }

}