using System.Collections.Generic;
using UnityEngine;
using FAS.UI;

namespace FAS
{
	[RequireComponent(typeof(HeadshotTextPool))]
	public class HeadshotView : UIScreen, IHeadshotView
	{
		[SerializeField] private List<Transform> _spawnPoints = new();

		private List<Transform> _shuffledPoints = new();
		private HeadshotTextPool _pool;

		private int _currentPointIndex;
		
		private float _verticalMoveOffset;
		
		private const float SCREEN_HEIGHT_MULTIPLIER = 0.2f;

		protected override void Awake()
		{
			base.Awake();
			_pool = GetComponent<HeadshotTextPool>();
			_verticalMoveOffset = Screen.height * SCREEN_HEIGHT_MULTIPLIER;
			ShuffleSpawnPoints();
		}

		public void ShowView()
		{
			if (_currentPointIndex >= _shuffledPoints.Count)
				ShuffleSpawnPoints();

			var origin = _shuffledPoints[_currentPointIndex++].position;
			var target = origin + Vector3.up * _verticalMoveOffset;
			_pool.Get().Play(origin, target);
		}

		private void ShuffleSpawnPoints()
		{
			_shuffledPoints = new List<Transform>(_spawnPoints);

			for (int i = _shuffledPoints.Count - 1; i > 0; i--)
			{
				var j = Random.Range(0, i + 1);
				(_shuffledPoints[i], _shuffledPoints[j]) = (_shuffledPoints[j], _shuffledPoints[i]);
			}

			_currentPointIndex = 0;
		}
	}
}