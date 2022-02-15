using System;
using Orders;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Order)), CanEditMultipleObjects]
public class OrderInspector : Editor
{
	
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		Order order = (Order) target;
		order.numberExtern=EditorGUILayout.IntField("Order Number",order.numberExtern);
		
		EditorGUILayout.LabelField("Article Orders:");
		
		EditorList.Show(serializedObject.FindProperty("articleOrders"), EditorListOption.Buttons);
		serializedObject.ApplyModifiedProperties();
	}
}
