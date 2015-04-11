﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

public class Snake : MonoBehaviour {
	Vector2 dir = Vector2.right;
	List<Transform> tail = 	new List<Transform>();

	// Grow in next movement?
	bool ate = false;
	
	// Tail Prefab
	public GameObject tailPrefab;

	//GameOver Object
	public GameObject gameOver;

	public GameObject topWall, bottomWall, rightWall, leftWall;



	// Use this for initialization
	void Start () {
		InvokeRepeating("Move", 0.1f, 0.1f);  
	}



	// Update is called once per frame
	void Update () {
		// Move in a new Direction?
		if ((Input.GetKey (KeyCode.RightArrow)) || (Input.GetKey(KeyCode.D))) {
			if (dir != -Vector2.right) {
				dir = Vector2.right;
			}
		} else if ((Input.GetKey (KeyCode.DownArrow)) || (Input.GetKey(KeyCode.S))) {
			if (dir != Vector2.up) {
				dir = -Vector2.up;    // '-up' means 'down'
			}
		} else if ((Input.GetKey (KeyCode.LeftArrow))  || (Input.GetKey(KeyCode.A))){
			if (dir != Vector2.right) {
				dir = -Vector2.right; // '-right' means 'left'
			}
		} else if ((Input.GetKey (KeyCode.UpArrow))  || (Input.GetKey(KeyCode.W))){
			if (dir != -Vector2.up) {
				dir = Vector2.up;
			}
		}
		if ((Time.timeScale == 0) && (Input.GetKey(KeyCode.Space)))
		    {
			Time.timeScale=1;
		}

	}

	void Move() {
		// Save current position (gap will be here)
		Vector2 v = transform.position;
		
		// Move head into new direction (now there is a gap)
		transform.Translate(dir);
		
		// Ate something? Then insert new Element into gap
		if (ate) {
			// Load Prefab into the world
			GameObject g =(GameObject)Instantiate(tailPrefab,
			                                      v,
			                                      Quaternion.identity);
			
			// Keep track of it in our tail list
			tail.Insert(0,g.transform);

			
			// Reset the flag
			ate = false;
		}
		// Do we have a Tail?
		else if (tail.Count > 0) {

			// Move last Tail Element to where the Head was
			tail.Last().position = v;
			
			// Add to front of list, remove from the back
			tail.Insert(0, tail.Last());
			tail.RemoveAt(tail.Count-1);

		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		// Food?

		if (coll.name.StartsWith ("foodPrefab")) {
			// Get longer in next Move call
			ate = true;
//			Debug.Log(coll.transform);
			// Remove the Food
			Destroy (coll.gameObject);
		}
		// Collided with Tail or Border
		else if (coll.name.StartsWith ("TailPrefab")) {
			//Debug.Log("hit with tail");
			Time.timeScale = 0;
		} else if ((coll.name.StartsWith ("Top")) || (coll.name.StartsWith ("Bottom")) || 
			(coll.name.StartsWith ("Left")) || (coll.name.StartsWith ("Right"))) {
			Time.timeScale = 0;
		}



	}
}