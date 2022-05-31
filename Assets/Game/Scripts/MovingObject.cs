using UnityEngine;
using System.Collections;

namespace Completed
{
	//The abstract keyword enables you to create classes and class members that are incomplete and must be implemented in a derived class.
	public abstract class MovingObject : MonoBehaviour
	{
		public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
		public LayerMask blockingLayer;			//Layer on which collision will be checked.
		
		
		private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
		private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
		private float inverseMoveTime;			//Used to make movement more efficient.
		private bool isMoving;					//Is the object currently moving.
		
		
		//Protected, virtual functions can be overridden by inheriting classes.
		protected virtual void Start ()
		{
			boxCollider = GetComponent <BoxCollider2D> ();
			rb2D = GetComponent <Rigidbody2D> ();
			inverseMoveTime = 1f / moveTime;
		}
		protected bool Move (int xDir, int yDir, out RaycastHit2D hit)
		{
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2 (xDir, yDir);
			boxCollider.enabled = false;
			hit = Physics2D.Linecast (start, end, blockingLayer);
			boxCollider.enabled = true;
			if(hit.transform == null && !isMoving)
			{
				StartCoroutine (SmoothMovement (end));
				return true;
			}
			return false;
		}
		protected IEnumerator SmoothMovement (Vector3 end)
		{
			isMoving = true;
			float sqrRemainingDistance = (transform.position - end).sqrMagnitude;
			while(sqrRemainingDistance > float.Epsilon)
			{
				Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);
				rb2D.MovePosition (newPostion);
				sqrRemainingDistance = (transform.position - end).sqrMagnitude;
				yield return null;
			}
			rb2D.MovePosition (end);
			isMoving = false;
		}
		protected virtual void AttemptMove <T> (int xDir, int yDir)
			where T : Component
		{
			RaycastHit2D hit;
			bool canMove = Move (xDir, yDir, out hit);
			if(hit.transform == null)
				return;
			T hitComponent = hit.transform.GetComponent <T> ();
			if(!canMove && hitComponent != null)
				OnCantMove (hitComponent);
		}
		protected abstract void OnCantMove <T> (T component)
			where T : Component;
	}
}
