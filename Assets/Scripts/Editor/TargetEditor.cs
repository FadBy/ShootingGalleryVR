#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Target))]
public class TargetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        var targetScript = (Target)target;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Target Controls", EditorStyles.boldLabel);

        using (new EditorGUI.DisabledScope(!Application.isPlaying))
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Open"))
            {
                targetScript.Open();
            }
            if (GUILayout.Button("Close"))
            {
                targetScript.Close();
            }
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Hit"))
            {
                targetScript.OnHit();
            }

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Movement Controls", EditorStyles.miniBoldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Start Movement"))
            {
                targetScript.SetActive(true);
            }
            if (GUILayout.Button("Stop Movement"))
            {
                targetScript.SetActive(false);
            }
            EditorGUILayout.EndHorizontal();
        }

        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("Кнопки доступны в Play Mode.", MessageType.Info);
        }
    }
}
#endif

