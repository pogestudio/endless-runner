﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Runner : MonoBehaviour
{

		public static float distanceTraveled;
	
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
				

				
				distanceTraveled = transform.localPosition.x;
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
		
		public bool isTouchingPlatform ()
		{
				//Debug.Log ("coll count: " + collidingGameObjects.Count);
				return collidingGameObjects.Count > 0;
		}
	
		public bool isTouchingWall ()
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
