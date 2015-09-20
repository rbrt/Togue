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
		  deceleration = .35f,
		  chaseCamFollowDelta = 75f;

	Vector3 inertiaVector = Vector3.zero,
			finalForce = Vector3.zero;

	void Update () {
		HandleInput();
		HandleForwardSpeed();
		HandleCornering();
		HandleChaseCamera();
		HandleInertia();

		finalForce = (-carObject.transform.forward * forwardSpeed * .75f) + inertiaVector * forwardSpeed;

		transform.position += finalForce;
	}

	void HandleChaseCamera(){
		float difference = Quaternion.Angle(cameraControl.transform.localRotation,
											carObject.transform.localRotation);

		float modifier = Mathf.Min(1, difference / 45f);

		cameraControl.transform.localRotation = Quaternion.RotateTowards(cameraControl.transform.localRotation,
																		 carObject.transform.localRotation,
																		 (chaseCamFollowDelta * modifier) * Time.deltaTime);

		cameraControl.transform.localPosition = Vector3.zero;
	}

	void HandleInertia(){
		float difference = Vector3.Angle(inertiaVector, carObject.transform.forward);
		float modifier = Mathf.Min(1, difference / 50f);

		inertiaVector = Vector3.RotateTowards(inertiaVector,
											  -carObject.transform.forward,
											  inertiaMaxDelta * modifier * Time.deltaTime,
											  0);
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
		float cameraRotationCarRotationDifference = Quaternion.Angle(cameraControl.transform.localRotation,
																	 carObject.transform.localRotation);

  		float modifier = 1;
		float maxDifference = 50;
		if (cameraRotationCarRotationDifference > maxDifference){
			modifier = 1 - ((cameraRotationCarRotationDifference - maxDifference) / 3f);
			Debug.Log("Noooope");
			return;
		}

		if (turningLeft){
			carObject.transform.Rotate(0, -turningSpeed * modifier * Time.deltaTime, 0);
		}

		if (turningRight){
			carObject.transform.Rotate(0, turningSpeed * modifier * Time.deltaTime, 0);
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
