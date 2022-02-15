using System;
using Orders;
using UnityEditor;
using UnityEngine;
using Stock;

[CustomEditor(typeof(Stock.Stock)), CanEditMultipleObjects]
public class StockInspector : Editor
{
	
	
	public override void OnInspectorGUI ()
	{
		serializedObject.Update();

		Stock.Stock stock = (Stock.Stock) target;
		//fix references
		stock.racks=(GameObject) EditorGUILayout.ObjectField("Racks",stock.racks, typeof(GameObject),true);
		stock.boxPrefab =
			(GameObject) EditorGUILayout.ObjectField("Box Prefab", stock.boxPrefab, typeof(GameObject), false);
		EditorGUILayout.LabelField("Small Items in Stock:");
		
		EditorList.Show(serializedObject.FindProperty("stockItems"), EditorListOption.Buttons);
		
		EditorGUILayout.LabelField("Big Items in Stock");
		stock.palletSpawns =
			(GameObject) EditorGUILayout.ObjectField("Pallet Spawn Points", stock.palletSpawns, typeof(GameObject),
				true);
		stock.undefinedPrefab =
			(GameObject) EditorGUILayout.ObjectField("Undefined Box Prefab", stock.undefinedPrefab, typeof(GameObject), false);
		EditorList.Show(serializedObject.FindProperty("bigItems"),EditorListOption.Buttons);
		serializedObject.ApplyModifiedProperties();
	}
}
