using UnityEditor;
using UnityEngine;

// i got rid of that stupid ondrawgizmos thing and added a simpler thing
// actually allows you to edit the notes rather than just see them
[CustomEditor(typeof(MidiMapper))]
public class RhythmRecorderEditor : Editor
{
    private void OnSceneGUI()
    {
        MidiMapper recorder = (MidiMapper)target;

        if (recorder.songMap == null || recorder.songMap.beats == null) return;

        // just prints out all the notes
        for (int i = 0; i < recorder.songMap.beats.Count; i++)
        {
            var beat = recorder.songMap.beats[i];

            Vector3 pos = new Vector3((int)beat.noteDirection * 2f, 0, beat.timestamp * recorder.visualSpacing);
            pos = recorder.transform.TransformPoint(pos);

            Handles.color = GetDirectionColor(beat.noteDirection);

            if (Handles.Button(pos, Quaternion.identity, 0.7f, 0.7f, Handles.CubeHandleCap))
            {
                recorder.selectedNoteIndex = i;
                Repaint();
            }

            // highlights selected note
            if (recorder.selectedNoteIndex == i)
            {
                Handles.color = Color.white;
                Handles.DrawWireCube(pos, Vector3.one * 1.1f);
            }

            // draws hold note tails
            if (beat.duration > 0)
            {
                Vector3 endPos = pos + new Vector3(0, 0, beat.duration * recorder.visualSpacing);
                Handles.DrawLine(pos, endPos);
                Handles.DrawWireCube(endPos, Vector3.one * 0.5f);
            }
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MidiMapper recorder = (MidiMapper)target;

        if (recorder.selectedNoteIndex >= 0 && recorder.selectedNoteIndex < recorder.songMap.beats.Count)
        {
            GUILayout.Space(20);
            EditorGUILayout.LabelField($"selected note #{recorder.selectedNoteIndex}", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            // Grab the data of the clicked note
            var beat = recorder.songMap.beats[recorder.selectedNoteIndex];

            // things you can change
            beat.timestamp = EditorGUILayout.FloatField("timestamp", beat.timestamp);
            beat.duration = EditorGUILayout.FloatField("hold duration", beat.duration);
            beat.noteDirection = (NoteDirection)EditorGUILayout.EnumPopup("direction", beat.noteDirection);

            GUILayout.Space(10);

            // note delete
            // BUG FIX: Unity uses 0f to 1f for colors. Color.red is a built-in shortcut.
            if (GUILayout.Button("note delete"))
            {
                recorder.songMap.beats.RemoveAt(recorder.selectedNoteIndex);
                recorder.selectedNoteIndex = -1;
                EditorUtility.SetDirty(recorder.songMap); // saves the deleted note essentially
            }
            GUI.backgroundColor = Color.white; // Reset color

            EditorGUILayout.EndVertical();

            // this is just to update changed notes
            if (GUI.changed)
            {
                EditorUtility.SetDirty(recorder.songMap);
            }
        }
    }

    private Color GetDirectionColor(NoteDirection dir)
    {
        switch (dir)
        {
            case NoteDirection.left: return Color.red;
            case NoteDirection.down: return Color.blue;
            case NoteDirection.up: return Color.green;
            case NoteDirection.right: return Color.yellow;
            default: return Color.white;
        }
    }
}