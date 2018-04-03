using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(WaypointObject))]
public class WaypointInspector : Editor {
	private Vector2 waypointScrollPos;

	private int waypointsLength;

	SerializedProperty durrations;
	SerializedProperty wayPoints;

	private string[] options = new string[] { "Loop", "Backtrack"};
	private int behaviorIndex = 0;

	public void OnEnable()
	{
		wayPoints = serializedObject.FindProperty("wayPoints");
		durrations = serializedObject.FindProperty("durrations");
	}

	public override void OnInspectorGUI()
	{
		WaypointObject myTarget = (WaypointObject)target;
		serializedObject.Update();

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
			if (myTarget.doLoop && durrations.arraySize != wayPoints.arraySize + 1)
			{
				EditorGUILayout.LabelField("WARNING! when looping you must have " + (wayPoints.arraySize+1) + " entries in your durrations Array!");
			}
			else if (myTarget.doBackTrack && durrations.arraySize != wayPoints.arraySize)
			{
				EditorGUILayout.LabelField("WARNING! when backtracking you must have " + (wayPoints.arraySize) + " entries in your durrations Array!");
			}
		}
	}

	private void DrawBehaviorFoldout(WaypointObject myTarget)
	{
		EditorGUILayout.LabelField("Behavior");

		EditorGUI.indentLevel += 1;
			
		behaviorIndex = EditorGUILayout.Popup("Behavior Mode", behaviorIndex, options);
		switch (behaviorIndex)
		{
			case 0:
				myTarget.doLoop = true;
				myTarget.doBackTrack = false;
				break;
			case 1:
				myTarget.doBackTrack = true;
				myTarget.doLoop = false;
				break;
		}
	
		myTarget.manualTuning = EditorGUILayout.Toggle("Manually Tune Durrations", myTarget.manualTuning);
		EditorGUI.indentLevel -= 1;
	}

	private void DrawTriggerFoldout(WaypointObject myTarget)
	{
		EditorGUILayout.LabelField("Trigger");
		EditorGUI.indentLevel += 1;
		myTarget.active = EditorGUILayout.Toggle("Active at Start", myTarget.active);
		EditorGUI.indentLevel -= 1;
	}
}
