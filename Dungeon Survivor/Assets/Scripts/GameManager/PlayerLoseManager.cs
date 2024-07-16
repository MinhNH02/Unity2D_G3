using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLoseManager : MonoBehaviour
{
	[SerializeField] GameObject gameOverPanel;
	PauseManager pauseManager;

	private void Start()
	{
		{
			pauseManager = GetComponent<PauseManager>();
		}
	}
	public void Lose()
	{
		gameOverPanel.SetActive(true);
		pauseManager.PauseGame();
	}
}
