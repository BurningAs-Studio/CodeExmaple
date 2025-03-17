using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FAS.Players
{
	public class PlayerSoundEffects : MonoBehaviour
	{
		[SerializeField] private List<AudioClip> _takeDamageSounds;
		[SerializeField] private List<AudioClip> _footstepsSounds;

		[Inject] private AudioSource _audioSource;
		
		public void PlayTakeDamageSound() => TryPlayRandomSoundEffect(_takeDamageSounds);
		
		public void PlayFootstepsSound() => TryPlayRandomSoundEffect(_footstepsSounds);

		public void TryPlayRandomSoundEffect(IEnumerable<AudioClip> audioClips)
		{
			AudioClip selectedClip = null;
			var count = 0;

			foreach (var clip in audioClips)
			{
				count++;
				if (Random.Range(0, count) == 0)
					selectedClip = clip;
			}

			if (selectedClip != null)
				_audioSource.PlayOneShot(selectedClip);
		}

		public void TryPlayRandomSoundEffect(List<AudioClip> audioClips)
		{
			_audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Count)]);
		}
	}
}