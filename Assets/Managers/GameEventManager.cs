

public class GameEventManager
{
		public delegate void GameEvent ();
		public static event GameEvent GameStart, GameOver;
	
		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
		
		public static void TriggerGameStart ()
		{
				if (GameStart != null) {
						GameStart ();
				}
		}
	
		public static void TriggerGameOver ()
		{
				if (GameOver != null) {
						GameOver ();
				}
		}
}
