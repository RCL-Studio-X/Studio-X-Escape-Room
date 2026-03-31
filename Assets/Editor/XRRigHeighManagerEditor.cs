using UnityEditor;
using UnityEngine;
using StudioXRCL.EscapeRoom.XR;

[CustomEditor(typeof(XRRigHeightManager))]
public class XRRigHeightManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw default inspector first
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Debug Controls", EditorStyles.boldLabel);

        XRRigHeightManager manager = (XRRigHeightManager)target;

        GUI.enabled = Application.isPlaying;

        EditorGUILayout.BeginVertical("box");

        // Mode buttons
        EditorGUILayout.LabelField("Modes", EditorStyles.boldLabel);

        if (GUILayout.Button("Set Standing"))
        {
            manager.SetStandingMode();
        }

        if (GUILayout.Button("Set Sitting"))
        {
            manager.SetSittingMode();
        }

        EditorGUILayout.Space();

        // Offset controls
        EditorGUILayout.LabelField("YOffset Controls", EditorStyles.boldLabel);

        float newOffset = EditorGUILayout.FloatField("Sitting Y Offset", manager.CurrentYOffset);

        if (GUILayout.Button("Apply Offset"))
        {
            manager.SetSittingOffsetValue(newOffset);
        }

        EditorGUILayout.Space();

        // Reset
        if (GUILayout.Button("Reset To Initial Height"))
        {
            manager.ResetToInitialHeight();
        }

        EditorGUILayout.EndVertical();

        GUI.enabled = true;

        // Info display
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Runtime Info", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("Current Mode", manager.CurrentMode.ToString());
        EditorGUILayout.LabelField("Current Y Offset", manager.CurrentYOffset.ToString("F3"));
    }
}
