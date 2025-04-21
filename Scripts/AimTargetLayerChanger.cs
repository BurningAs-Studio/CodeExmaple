using System.Collections.Generic;
using UnityEngine;
using VInspector;
using System;

namespace FAS
{
	public class AimTargetLayerChanger : MonoBehaviour
	{
		[SerializeField] private List<DamageableCollider> _colliders = new ();
		[SerializeField] private bool _isUseSkinnedMesh;
		[ShowIf(nameof(_isUseSkinnedMesh))]
		[SerializeField] private List<MeshRenderer> _meshRenderers;
		[HideIf(nameof(_isUseSkinnedMesh))]
		[SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;

		private void OnEnable()
		{
			throw new NotImplementedException();
		}
	}
}