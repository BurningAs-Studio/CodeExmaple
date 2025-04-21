using UnityEngine;

namespace FAS.Weapons
{
	public class Axe : RangeWeapon
	{
		[SerializeField] private MeshRenderer _meshRenderer;
		[SerializeField] private Material _detailedMaterial;
		[SerializeField] private Material _simpleMaterial;

		public override WeaponAnimType AnimType => WeaponAnimType.ThrowMeleeWeapon;
		public override WeaponName Name => WeaponName.Axe;

		private Material[] _materials;

		private void Awake()
		{
			_materials = _meshRenderer.materials;
		}

		public override void EnableDetailedMaterials()
		{
			_materials[0] = _detailedMaterial;
			_meshRenderer.materials = _materials;
		}

		public override void EnableSimpleMaterials()
		{
			_materials[0] = _simpleMaterial;
			_meshRenderer.materials = _materials;
		}

		public override void Attack()
		{
		}
	}
}