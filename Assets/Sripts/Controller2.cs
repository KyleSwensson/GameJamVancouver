using UnityEngine;
using System.Collections;

//[System.Serializable]
//public class Boundary {
//	//create a separate class to make the code useable
//	public float xMin, xMax, yMin, yMax;
//}

public class Controller2 : MonoBehaviour {

	public float maxSpeed = 10f;

	public Boundary boundary;

	private bool foundtheFuckingAI = false;

	private GameObject thatSHittyAI = null;

	public int MYAWESOMESCORE = 0;
	
	//	public bool facingRight = true;
//	
//	Animator anim;
	
	// Use this for initialization
	void Start () {
		
		//anim = GetComponent<Animator>();
		
	}

	void Update(){
		if(Input.GetKeyDown (KeyCode.Space))
		{
			//check for player's collision with game object tagged Dock
			if (foundtheFuckingAI && (thatSHittyAI != null)){
				Destroy(thatSHittyAI);
				MYAWESOMESCORE += 1;
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate () {

			move ();


	}

	void move()
	{


		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		transform.position += new Vector3 (moveHorizontal, moveVertical, 0).normalized * Time.deltaTime * maxSpeed;
		
		GetComponent<Rigidbody2D>().position = new Vector3 
			(
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax),
				0.0f
				);

//		if (moveHorizontal  > 0 && ! facingRight)
//			Flip ();
//		else if (moveHorizontal < 0 && facingRight)
//			Flip ();
		
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "People") {
			//Destroy (coll.gameObject);
			foundtheFuckingAI = true;	
			thatSHittyAI = coll.gameObject;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if (coll.gameObject.tag == "People") {
			//Destroy (coll.gameObject);
			foundtheFuckingAI = false;	
			thatSHittyAI = null;
		}
	}





//	void Flip()
//	{
//		facingRight = ! facingRight;
//		Vector3 theScale = transform.localScale;
//		theScale.x *= -1;
//		transform.localScale = theScale;
//	}
}

