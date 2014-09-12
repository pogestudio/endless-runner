using UnityEngine;
using System.Collections;

public class MovementController : MonoBehaviour
{

		public float maxSpeed;
		public float acceleration;
		public float jumpVelocity;
		private Runner kRunner;

		void Start ()
		{
				kRunner = gameObject.GetComponent<Runner> ();
		}
		void FixedUpdate ()
		{
				deciceIfWeShouldMove ();
				decideIfWeShouldJump ();
		}
		
	
		void decideIfWeShouldJump ()
		{
				if (Input.GetButtonDown ("Jump") && kRunner.isTouchingFloor ()) {
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
				if (userUpInput > 0 && kRunner.isTouchingWall ()) {
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
	
}