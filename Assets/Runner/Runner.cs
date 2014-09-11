using UnityEngine;
using System.Collections;

public class Runner : MonoBehaviour
{

		public static float distanceTraveled;
		public Vector3 jumpVelocity;
	
		public float acceleration;
		
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
				if (this.touchingPlatform ()) {
						rigidbody.AddForce (acceleration, 0f, 0f, ForceMode.Acceleration);
				}
				if (this.touchingPlatform () && Input.GetButtonDown ("Jump")) {
						rigidbody.AddForce (jumpVelocity, ForceMode.VelocityChange);
				}
				distanceTraveled = transform.localPosition.x;
		
				transform.Translate (5f * Time.deltaTime, 0f, 0f);	
				distanceTraveled = transform.localPosition.x;
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
