using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VInspector;

namespace FAS.Projectiles
{
	public class ProjectilePool : MonoBehaviour
	{
		[SerializeField] private List<Projectile> _projectiles = new ();
		[SerializeField] private Projectile _projectilePrefab;

		private readonly Queue<Projectile> _pool = new ();

		private void Awake()
		{
			foreach (var projectile in _projectiles)
			{
				_pool.Enqueue(projectile);
				projectile.Initialize();
				projectile.OnMoveComplete += Set;
			}
		}

		private void OnDisable()
		{
			if (_pool.Any())
				foreach (var projectile in _projectiles)
					projectile.OnMoveComplete -= Set;
		}

		public Projectile Get()
		{
			Projectile projectile;

			if (_pool.Any())
			{
				projectile = _pool.Dequeue();
			}
			else
			{
				projectile = Instantiate(_projectilePrefab, transform);
				_projectiles.Add(projectile);
				projectile.Initialize();
				projectile.OnMoveComplete += Set;
			}

			projectile.gameObject.SetActive(true);
			return projectile;
		}

		private void Set(Projectile projectile)
		{
			projectile.gameObject.SetActive(false);
			_pool.Enqueue(projectile);
		}
		
#if UNITY_EDITOR
		[Button]
		private void FindProjectiles()
		{
			_projectiles.Clear();
			_projectiles.AddRange(GetComponentsInChildren<Projectile>());
		}
#endif
	}
}