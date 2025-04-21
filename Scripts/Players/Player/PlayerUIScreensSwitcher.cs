using UnityEngine;
using FAS.UI;

namespace FAS.Players
{
	public class PlayerUIScreensSwitcher : MonoBehaviour
	{
		[SerializeField] private UIScreen _inputScreen;
		[SerializeField] private UIScreen _healthViewScreen;
		[SerializeField] private UIScreen _settingsScreen;

		private void Start()
		{
			ShowDefaultScreen();
		}

		public void ShowDefaultScreen()
		{
			_healthViewScreen.Show();
			_settingsScreen.Hide();
			_inputScreen.Show();
		}	
		
		public void ShowFatalityScreen()
		{
			_healthViewScreen.Hide();
			_settingsScreen.Hide();
			_inputScreen.Hide();
		}

		public void ShowSettingsScreen()
		{
			_healthViewScreen.Hide();
			_settingsScreen.Show();
			_inputScreen.Hide();
		}
	}	
}
