#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VInspector;

public class TreesCreatorEditor : MonoBehaviour
{
	[SerializeField] private List<Transform> _targetTransforms = new List<Transform>();
	[SerializeField] private List<Transform> _createdTrees = new List<Transform>();
	[SerializeField] private GameObject _prefab;

	[Button]
	public void CreateTrees()
	{
		_createdTrees.Clear();
		foreach (var targetTransform in _targetTransforms)
		{
			var newObject = (GameObject)PrefabUtility.InstantiatePrefab(_prefab, targetTransform);

			newObject.transform.position = targetTransform.position;
			newObject.transform.rotation = targetTransform.rotation;
			newObject.transform.SetParent(transform, true);
			_createdTrees.Add(newObject.transform);
		}
	}

	[Button]
	public void DestroyTrees()
	{
		foreach (var targetTransform in _createdTrees)
			DestroyImmediate(targetTransform.gameObject);
		
		_createdTrees.Clear();
	}
}
#endif