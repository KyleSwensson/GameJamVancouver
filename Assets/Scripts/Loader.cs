using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;
    public GameObject AStar;

    void Awake()
    {
        if (GameManagerScript.instance == null)
        {
            Instantiate(gameManager);
        }
        Instantiate(AStar);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
