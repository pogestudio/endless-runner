using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

		public static float distanceTraveled;
		public Vector3 jumpVelocity;
	
		public float acceleration;
		public float maxSpeed;
		
		private int platformContacts;
	
		public float gameOverY;
		private Vector3 startPosition;
	
		void Start ()
		{
				GameEventManager.GameStart += GameStart;
				GameEventManager.GameOver += GameOver;
				startPosition = transform.localPosition;
				renderer.enabled = false;
				rigidbody.isKinematic = true;
				enabled = false;
		}
		void FixedUpdate ()
		{
				if (transform.localPosition.y < gameOverY) {
						GameEventManager.TriggerGameOver ();
				}
				
				deciceIfWeShouldMove ();
				
				if (this.touchingPlatform () && Input.GetButtonDown ("Jump")) {
						rigidbody.AddForce (jumpVelocity, ForceMode.VelocityChange);
				}
				distanceTraveled = transform.localPosition.x;
				//transform.Translate (5f * Time.deltaTime, 0f, 0f);	
				distanceTraveled = transform.localPosition.x;
		}
		
		void deciceIfWeShouldMove ()
		{
				float directionUserWantsToMove = Input.GetAxis ("Horizontal");
				bool oneAboveOneBelow = rigidbody.velocity.x > 0 && 0 > directionUserWantsToMove;
				bool oneBelowOneAbove = rigidbody.velocity.x < 0 && 0 < directionUserWantsToMove;
				if (Mathf.Abs (rigidbody.velocity.x) < maxSpeed || oneAboveOneBelow || oneBelowOneAbove) {
						Vector3 accelerationVector = new Vector3 (directionUserWantsToMove * acceleration, 0, 0);
						rigidbody.AddForce (accelerationVector, ForceMode.VelocityChange);
				}
		}
	
		void OnCollisionEnter (Collision collision)
		{
				++platformContacts;
		}
	
		void OnCollisionExit (Collision collision)
		{
				--platformContacts;
		}
		
		private bool touchingPlatform ()
		{
				return platformContacts > 0;
		}
		
		private void GameStart ()
		{
				distanceTraveled = 0f;
				transform.localPosition = startPosition;
				renderer.enabled = true;
				rigidbody.isKinematic = false;
				enabled = true;
				Debug.Log ("GameStart for Runner is called!");
		}
	
		private void GameOver ()
		{
				renderer.enabled = false;
				rigidbody.isKinematic = true;
				enabled = false;
		}
		
}
