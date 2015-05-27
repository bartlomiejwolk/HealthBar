﻿using UnityEditor;
using UnityEngine;

namespace HealthBarEx {

    [CustomEditor(typeof(HealthBar))]
    [CanEditMultipleObjects]
    public sealed class HealthBarEditor : Editor {
        #region FIELDS

        private HealthBar Script { get; set; }

        #endregion FIELDS

        #region SERIALIZED PROPERTIES

        private SerializedProperty description;
        private SerializedProperty healthBarGUI;
        private SerializedProperty targetOffset;
        private SerializedProperty cameraTracker;
        private SerializedProperty refRelativeDist;

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
            Script = (HealthBar)target;

            healthBarGUI = serializedObject.FindProperty("healthBarGUI");
            targetOffset = serializedObject.FindProperty("targetOffset");
            cameraTracker = serializedObject.FindProperty("cameraTracker");
            refRelativeDist = serializedObject.FindProperty("refRelativeDist");

            description = serializedObject.FindProperty("description");
        }

        #endregion UNITY MESSAGES

        #region INSPECTOR CONTROLS
        private void DrawRelativeDistanceField() {
            EditorGUILayout.PropertyField(
                refRelativeDist,
                new GUIContent(
                    "Relative Distance",
                    ""));
        }

        private void DrawCameraField() {
            EditorGUILayout.PropertyField(
                cameraTracker,
                new GUIContent(
                    "Camera",
                    "Camera used to calculate health bar size."));
        }

        private void DrawTargetOffsetField() {
            EditorGUILayout.PropertyField(
                targetOffset,
                new GUIContent(
                    "Target Offset",
                    ""));
        }

        private void DrawHealthBarGUI() {
            EditorGUILayout.PropertyField(
                healthBarGUI,
                new GUIContent(
                    "Health Bar GUI",
                    ""),
                    true);
        }


        private void DrawVersionLabel() {
            EditorGUILayout.LabelField(
                string.Format(
                    "{0} ({1})",
                    HealthBar.Version,
                    HealthBar.Extension));
        }

        private void DrawDescriptionTextArea() {
            description.stringValue = EditorGUILayout.TextArea(
                description.stringValue);
        }

        #endregion INSPECTOR

        #region METHODS

        [MenuItem("Component/HealthBar")]
        private static void AddEntryToComponentMenu() {
            if (Selection.activeGameObject != null) {
                Selection.activeGameObject.AddComponent(typeof(HealthBar));
            }
        }

        #endregion METHODS
    }

}