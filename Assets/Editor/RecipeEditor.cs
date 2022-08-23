using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Recipe))]
public class RecipeEditor : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		Recipe cityBuilder = (Recipe)target;
		if(GUILayout.Button("Fill recipe"))
		{
			cityBuilder.FillRecipe();
		}
		if(GUILayout.Button("Clear"))
		{
			cityBuilder.Clear();
		}
	}
}
