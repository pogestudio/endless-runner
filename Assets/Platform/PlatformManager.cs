using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlatformManager : MonoBehaviour
{
	
		public Transform prefab;
		public int numberOfObjects;
		public float recycleOffset = 0.0f;
	
		public Vector3 startPosition;
	
		private Vector3 nextPosition;
		private Queue<Transform> objectQueue;
	
		public Vector3 minSize, maxSize;
		
		
		public float runningGapBetweenBlocks;
		
		public float maxMoveBetweenBlocks;
	
		public Material[] materials;
		public PhysicMaterial[] physicMaterials;
	
	
	
		void Start ()
		{
				GameEventManager.GameStart += GameStart;
				GameEventManager.GameOver += GameOver;
		
				objectQueue = new Queue<Transform> (numberOfObjects);
		
				for (int i = 0; i < numberOfObjects; i++) {
						objectQueue.Enqueue ((Transform)Instantiate (
				prefab, new Vector3 (0f, 0f, -100f), Quaternion.identity));
				}
				enabled = false;
		}
	
		private void GameStart ()
		{
				nextPosition = startPosition;
				for (int i = 0; i < numberOfObjects; i++) {
						Recycle ();
				}
				enabled = true;
		}
	
		private void GameOver ()
		{
				enabled = false;
		}
	
		void FixedUpdate ()
		{
				if (objectQueue.Peek ().localPosition.x + recycleOffset < Runner.distanceTraveled) {
						Recycle ();
				}
		}
	
		private Vector3 oldScale = new Vector3 (0, 0, 0);
		private void Recycle ()
		{
				
				float randomisedY = Random.Range (minSize.y, maxSize.y);
				if (randomisedY > oldScale.y + maxMoveBetweenBlocks) {
						randomisedY = oldScale.y + maxMoveBetweenBlocks;
				}
		
				if (randomisedY < oldScale.y - maxMoveBetweenBlocks) {
						randomisedY = oldScale.y - maxMoveBetweenBlocks;
				}		
				
				randomisedY = Mathf.Min (randomisedY, maxSize.y);
				randomisedY = Mathf.Max (randomisedY, minSize.y);
				

				Vector3 scale = new Vector3 (
			Random.Range (minSize.x, maxSize.x), 
			randomisedY,
			Random.Range (minSize.z, maxSize.z));
		
				Vector3 position = nextPosition;
				position.x += scale.x * 0.5f;
				position.y = startPosition.y + scale.y * 0.5f;
		
				Transform objectToHandle = objectQueue.Dequeue ();
				objectToHandle.localScale = scale;
				objectToHandle.localPosition = position;
		
				int materialIndex = Random.Range (0, materials.Length);
				objectToHandle.renderer.material = materials [materialIndex];
				objectToHandle.collider.material = physicMaterials [materialIndex];
		
		
				objectQueue.Enqueue (objectToHandle);
				
				nextPosition.x += scale.x + runningGapBetweenBlocks;
				//Debug.Log ("HaveMoved: " + (oldPosition.y - nextPosition.y));
				oldScale = objectToHandle.localScale;
		}
	
}