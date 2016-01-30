using UnityEngine;
using System.Collections;

[System.Serializable]
public class Boundary {
	//create a separate class to make the code useable
	public float xMin, xMax, yMin, yMax;
}

public class AIMechanics : MonoBehaviour {

	private Vector3[] directions = {new Vector3(0.3f,0,0), new Vector3(-0.3f,0,0), new Vector3(0,0.3f,0), new Vector3(0,-0.3f,0)};
	private string[] directionNames = {"East", "West", "North", "South"};
	public Vector3 curDirection;
	public string curDirectionName;

	public float speed;
	public float dirSpeed;
	
	private float nextDir;
	private float nextMovement;

	public Boundary boundary;

	// Use this for initialization
	void Start () {
		curDirection = new Vector3(0.3f,0,0);
		curDirectionName = "East";
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextMovement) {
			Move();
			nextMovement = Time.time + speed;
		}

		if (Time.time > nextDir && isAtBranch()) {
			changeDir();
			nextDir = Time.time + dirSpeed;
		}

	}

	public void Move(){
		transform.position += curDirection;

		transform.position = new Vector3 
			(
				Mathf.Clamp(transform.position.x, boundary.xMin, boundary.xMax), 
				Mathf.Clamp(transform.position.y, boundary.yMin, boundary.yMax),
				-0.368f
			);
	}

	public void changeDir(){

		int index = generateIndex ();

		if (canMove(index)) {
			curDirection = directions[index];
			curDirectionName = directionNames [index];
		}

	}

	public int generateIndex(){
		int index = Random.Range (0,4);
		if (curDirectionName.Equals ("East")) {
			while (index == 1) // don't go west
				index = Random.Range (0,4);
		} else if (curDirectionName.Equals ("West")) {
			index = Random.Range (1,4);
		} else if (curDirectionName.Equals ("North")) {
			index = Random.Range (1,3);
		} else if (curDirectionName.Equals ("South")) {
			while (index == 2) // don't go north
				index = Random.Range (0,4);
		}
		return index;

	}

	public bool isAtBranch ()
	{
		if (curDirectionName.Equals ("East")) {
			return (canMove (2) || canMove (3));
		} else if (curDirectionName.Equals ("West")) {
			return (canMove (2) || canMove (3));
		} else if (curDirectionName.Equals ("North")) {
			return (canMove (0) || canMove (1));
		} else if (curDirectionName.Equals ("South")) {
			return (canMove (0) || canMove (1));
		}

		return false;
	}	

	public bool canMove(int index){
		return this.gameObject.transform.GetChild (index).gameObject.GetComponent<detection> ().canMove;
	}
	
	public int convert (string dir){
		if (dir.Equals ("East")) {
			return 0;
		} else if (dir.Equals ("West")) {
			return 1;
		} else if (dir.Equals ("North")) {
			return 2;
		} else if (dir.Equals ("South")) {
			return 3;
		} else {
			return 0;
		}

	}
}	