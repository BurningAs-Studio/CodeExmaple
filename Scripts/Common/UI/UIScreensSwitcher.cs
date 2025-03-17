using UnityEngine;
using FAS.UI;

public class UIScreensSwitcher : MonoBehaviour
{
	[SerializeField] private UIScreen _inputScreen;
	[SerializeField] private UIScreen _shootingScreen;
	[SerializeField] private UIScreen _aimScreen;
	[SerializeField] private UIScreen _winScreen;
	[SerializeField] private UIScreen _loseScreen;

	public void ShowLoseScreen()
	{
		_inputScreen.Hide();
		_shootingScreen.Hide();
		_aimScreen.Hide();
		_winScreen.Hide();
		_loseScreen.Show();
	}

	public void ShowWinScreen()
	{
		_inputScreen.Hide();
		_shootingScreen.Hide();
		_aimScreen.Hide();
		_winScreen.Show();
		_loseScreen.Hide();
	}
}
