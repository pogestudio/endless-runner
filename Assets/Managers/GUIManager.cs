using UnityEngine;

public class GUIManager : MonoBehaviour
{
	
		public GUIText gameOverText, instructionsText, runnerText;
		// Use this for initialization
		void Start ()
		{
				GameEventManager.GameStart += GameStart;
				GameEventManager.GameOver += GameOver;
		
				gameOverText.enabled = false;
				runnerText.enabled = true;
		}
	
		void Update ()
		{
				if (Input.GetButtonDown ("Jump")) {
						GameEventManager.TriggerGameStart ();
				}
		}
		
		private void GameStart ()
		{
				gameOverText.enabled = false;
				instructionsText.enabled = false;
				runnerText.enabled = false;
				enabled = false;
		}
		
		private void GameOver ()
		{
				runnerText.enabled = true;
				gameOverText.enabled = true;
				instructionsText.enabled = true;
				enabled = true;
		}
}
