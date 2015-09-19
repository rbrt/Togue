using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	[SerializeField] protected GameObject carObject,
										  cameraControl;

	bool accelerating = false,
		 turningRight = false,
		 turningLeft = false;

	float maxSpeed = 1,
		  forwardSpeed = 0,
		  turningSpeed = 140,
		  inertiaMaxDelta = .75f,
		  acceleration = .5f,
		  deceleration = .35f;

	Vector3 inertiaVector = Vector3.zero,
			finalForce = Vector3.zero;

	void Update () {
		HandleInput();
		HandleForwardSpeed();
		HandleCornering();

		inertiaVector = Vector3.RotateTowards(inertiaVector.normalized,
											  -carObject.transform.forward.normalized,
											  inertiaMaxDelta * Time.deltaTime,
											  0);

		finalForce = (-carObject.transform.forward * forwardSpeed * .1f) + inertiaVector * forwardSpeed;

		transform.position += finalForce;
	}

	void HandleForwardSpeed(){
		if (accelerating){
			forwardSpeed = Mathf.Min(forwardSpeed + acceleration * Time.deltaTime,
			 						 maxSpeed);
		}
		else{
			forwardSpeed = Mathf.Max(forwardSpeed - deceleration * Time.deltaTime,
									 0);
		}
	}

	void HandleCornering(){
		if (turningLeft){
			carObject.transform.Rotate(0, -turningSpeed * Time.deltaTime, 0);
		}

		if (turningRight){
			carObject.transform.Rotate(0, turningSpeed * Time.deltaTime, 0);
		}
	}

	void HandleInput(){
		if (Input.GetKeyDown(KeyCode.A)){
			turningRight = false;
			turningLeft = true;
		}
		else if (Input.GetKeyDown(KeyCode.D)){
			turningLeft = false;
			turningRight = true;
		}

		if (Input.GetKeyDown(KeyCode.W)){
			accelerating = true;
		}


		if (Input.GetKeyUp(KeyCode.A)){
			turningLeft = false;
		}
		else if (Input.GetKeyUp(KeyCode.D)){
			turningRight = false;
		}

		if (Input.GetKeyUp(KeyCode.W)){
			accelerating = false;
		}
	}

	void Awake(){
		inertiaVector = -carObject.transform.forward;
	}
}
