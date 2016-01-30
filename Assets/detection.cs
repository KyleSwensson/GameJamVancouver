using UnityEngine;
using System.Collections;

public class detection : MonoBehaviour {

	public bool canMove;
	// Use this for initialization
	void Start () {
		canMove = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D (Collider2D other){

		canMove = false;
		if (other.CompareTag("People")){
			canMove = false;
		} else if (other.CompareTag("Player")){
			//TODO
		}

	}

	void OnTriggerExit2D (Collider2D other){
		canMove = true;
	}


}
