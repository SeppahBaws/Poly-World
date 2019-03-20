// ================================================
//
//     EditorStateVisualizer By Seppe Dekeyser.
//
// ================================================

using UnityEngine;
using UnityEditor;

enum EditorState
{
	Ready,
	Compiling,
	Paused,
	Playing,
	Updating
}

public class EditorStateVisualizer : EditorWindow
{
	private EditorState state;

	[MenuItem("General Utils/Editor State")]
	static void Init()
	{
		EditorWindow window = EditorWindow.GetWindowWithRect(typeof(EditorStateVisualizer), new Rect(0, 0, 200, 30));
		window.Show();
	}

	void OnGUI()
	{
		if (EditorApplication.isCompiling)
			state = EditorState.Compiling;

		else if (EditorApplication.isPaused)
			state = EditorState.Paused;

		else if (EditorApplication.isPlaying)
			state = EditorState.Playing;

		else if (EditorApplication.isUpdating)
			state = EditorState.Updating;

		else
			state = EditorState.Ready;

		EditorGUILayout.LabelField("Editor state:", state.ToString());
		this.Repaint();
	}
}
