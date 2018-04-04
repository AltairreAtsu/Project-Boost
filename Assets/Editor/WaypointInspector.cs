using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WaypointObject))]
public class WaypointInspector : Editor {
	private Vector2 waypointScrollPos;

	private int waypointsLength;

	GUIContent behaviorLabel;
	SerializedProperty behavior;
	private bool looping;
	private bool backTracking;

	SerializedProperty durrations;
	SerializedProperty wayPoints;

	GUIContent manualLabel;
	SerializedProperty manualDurration;

	GUIContent activeLabel;
	SerializedProperty activeAtStart;

	private string[] options = new string[] { "Loop", "Backtrack"};
	private int behaviorIndex;

	public void OnEnable()
	{
		WaypointObject myTarget = (WaypointObject)target;
		behavior = serializedObject.FindProperty("behavior");
		wayPoints = serializedObject.FindProperty("wayPoints");
		durrations = serializedObject.FindProperty("durrations");
		manualDurration = serializedObject.FindProperty("manualTuning");
		activeAtStart = serializedObject.FindProperty("active");

		behaviorLabel = new GUIContent("Behavior Mode:");
		activeLabel = new GUIContent("Active at Start");
		manualLabel = new GUIContent("Manually Tune Durrations");

		looping = myTarget.behavior == WaypointObject.Behavior.Looping;
		backTracking = myTarget.behavior == WaypointObject.Behavior.Backtracking;

		if (looping)
		{
			behaviorIndex = 0;
		}
		else if (backTracking)
		{
			behaviorIndex = 1;
		}
	}

	public override void OnInspectorGUI()
	{
		WaypointObject myTarget = (WaypointObject)target;
		serializedObject.Update();

		looping = myTarget.behavior == WaypointObject.Behavior.Looping;
		backTracking = myTarget.behavior == WaypointObject.Behavior.Backtracking;

		DrawBehaviorFoldout(myTarget);
		DrawTriggerFoldout(myTarget);
		EditorGUILayout.PropertyField(wayPoints, true);
		DrawDurration(myTarget, wayPoints, durrations);

		serializedObject.ApplyModifiedProperties();
	}

	private void DrawDurration(WaypointObject myTarget, SerializedProperty wayPoints, SerializedProperty durrations)
	{
		if (!myTarget.manualTuning)
		{
			myTarget.totalDurration = EditorGUILayout.FloatField("Total Durration", myTarget.totalDurration);

		}
		else
		{
			EditorGUILayout.PropertyField(durrations, true);
			if (looping && durrations.arraySize != wayPoints.arraySize + 1)
			{
				EditorGUILayout.LabelField("WARNING! when looping you must have " + (wayPoints.arraySize+1) + " entries in your durrations Array!");
			}
			else if (backTracking && durrations.arraySize != wayPoints.arraySize)
			{
				EditorGUILayout.LabelField("WARNING! when backtracking you must have " + (wayPoints.arraySize) + " entries in your durrations Array!");
			}
		}
	}

	private void DrawBehaviorFoldout(WaypointObject myTarget)
	{
		EditorGUILayout.LabelField("Behavior");

		EditorGUI.indentLevel += 1;

		EditorGUILayout.PropertyField(behavior, behaviorLabel);

		EditorGUILayout.PropertyField(manualDurration, manualLabel);
		EditorGUI.indentLevel -= 1;
	}

	private void DrawTriggerFoldout(WaypointObject myTarget)
	{
		EditorGUILayout.LabelField("Trigger");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.PropertyField(activeAtStart, activeLabel);
		EditorGUI.indentLevel -= 1;
	}
}
