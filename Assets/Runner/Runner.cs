using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Runner : MonoBehaviour
{

		public static float distanceTraveled;
		public float jumpVelocity;
	
		public float acceleration;
		public float maxSpeed;
		
		private int platformContacts;
	
		public float gameOverY;
		private Vector3 startPosition;
		
		private List<GameObject> collidingGameObjects;
	
		void Start ()
		{
				collidingGameObjects = new List<GameObject> ();
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
				
				decideIfWeShouldJump ();
				distanceTraveled = transform.localPosition.x;
				//transform.Translate (5f * Time.deltaTime, 0f, 0f);	
				distanceTraveled = transform.localPosition.x;
		}
		
		void decideIfWeShouldJump ()
		{
				if (Input.GetButtonDown ("Jump") && this.touchingPlatform ()) {
						rigidbody.velocity = new Vector3 (rigidbody.velocity.x, jumpVelocity, 0);
				}
		}
		
		void deciceIfWeShouldMove ()
		{
				//HORIZONTAL
				float directionUserWantsToMove = Input.GetAxis ("Horizontal");
				bool oneAboveOneBelow = rigidbody.velocity.x > 0 && 0 > directionUserWantsToMove;
				bool oneBelowOneAbove = rigidbody.velocity.x < 0 && 0 < directionUserWantsToMove;
				if (Mathf.Abs (rigidbody.velocity.x) < maxSpeed || oneAboveOneBelow || oneBelowOneAbove) {
						Vector3 accelerationVector = new Vector3 (directionUserWantsToMove * acceleration, 0, 0);
						rigidbody.AddForce (accelerationVector, ForceMode.VelocityChange);
				}
				
				//VERTICAL
				float userUpInput = Input.GetAxis ("Vertical");
		
				//do we want to move. are we allowed to move?
				if (userUpInput > 0 && isTouchingWall ()) {
						bool deacceleratingFall = rigidbody.velocity.y < 0;
						bool belowMaxSpeed = Mathf.Abs (rigidbody.velocity.y) < maxSpeed;
						bool shouldMoveUpwards = belowMaxSpeed || deacceleratingFall;
						if (shouldMoveUpwards) {
								Debug.Log ("Adding upwards force " + Time.time);
								Vector3 jumpVector = new Vector3 (0, userUpInput * acceleration, 0);
								rigidbody.AddForce (jumpVector, ForceMode.VelocityChange);
						}
				}
		}
	
		void OnCollisionEnter (Collision collision)
		{
		
				Vector3 collisionpoint = collision.contacts [0].point;
				Vector3 runnerPoint = transform.position;
				Vector3 direction = runnerPoint - collisionpoint;
				bool isFloor = Mathf.Abs (direction.y) >= 0.9;
				Debug.Log ("direction.y: " + direction.y);
				Debug.Log ("OCE: direction for aCollision is:: " + direction + " WHICH IS FLOOR??? " + isFloor);
		
		
				collidingGameObjects.Add (collision.gameObject);
				Debug.Log ("collision enter: " + collision);
		}
	
		void OnCollisionExit (Collision collision)
		{
				collidingGameObjects.Remove (collision.gameObject);
				Debug.Log ("collision exit: " + collision);
		}
		
		private bool touchingPlatform ()
		{
				//Debug.Log ("coll count: " + collidingGameObjects.Count);
				return collidingGameObjects.Count > 0;
		}
	
		private bool isTouchingWall ()
		{
				//loop through all collisions
				//check if any of the objects have a 90degree diff towards player
				bool isSomeWall = false;
				foreach (GameObject collidingObject in collidingGameObjects) {
						Vector3 collisionpoint = collidingObject.transform.position;
						Vector3 runnerPoint = transform.position;
						Vector3 direction = runnerPoint - collisionpoint;
						bool isFloor = Mathf.Abs (direction.y) >= 0.9;
						if (!isFloor) {
								isSomeWall = true;
								break;
						}
				}
				return isSomeWall;
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
