using UnityEngine;
using System.Collections;

public class particlesLogic : MonoBehaviour {


	void Start () {
	
	}
	

	void Update () {
		Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		position.z = 10f;
		transform.position = position; 
	}
}
