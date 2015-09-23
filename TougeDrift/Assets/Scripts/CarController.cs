using UnityEngine;
using System.Collections;

public class CarController : MonoBehaviour {

	[SerializeField] protected GameObject carObject,
										  cameraControl;

	bool accelerating = false,
		 turningRight = false,
		 turningLeft = false;

	float maxSpeed = .8f,
		  forwardSpeed = 0,
		  turningSpeed = 140,
		  currentTurnSpeed = 0,
		  turnAcceleration = 150f,
		  turndeceleration = 200f,
		  turnBoost = 600,
		  inertiaMaxDelta = 1f,
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

		finalForce = (-carObject.transform.forward * forwardSpeed * .35f) + inertiaVector * forwardSpeed * .7f;

		transform.position += finalForce;
		cameraControl.transform.localPosition = carObject.transform.localPosition;
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
		float modifier = Mathf.Min(1, difference / 30f);

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
		Debug.DrawRay(carObject.transform.position, inertiaVector * 10, Color.yellow);
		Debug.DrawRay(carObject.transform.position, -carObject.transform.forward * forwardSpeed *10, Color.red);

		float forwardVectorInvertiaVectorDifference = Vector3.Angle(inertiaVector,
																	carObject.transform.forward);

  		float modifier = 1;
		float maxDifference = 50;
		if (forwardVectorInvertiaVectorDifference > maxDifference){
			modifier = 1 - ((forwardVectorInvertiaVectorDifference - maxDifference) / 3f);
		}

		bool boostTurn = false;
		if (turningLeft){
			if (currentTurnSpeed > 0){
				boostTurn = true;
			}
			currentTurnSpeed = Mathf.Max(currentTurnSpeed - (turnAcceleration + (boostTurn ? turnBoost : 0)) *
															Time.deltaTime,
										 -turningSpeed);
		}
		else if (turningRight){
			if (currentTurnSpeed < 0){
				Debug.Log("Boost right");
				boostTurn = true;
			}
			currentTurnSpeed = Mathf.Min(currentTurnSpeed + (turnAcceleration + (boostTurn ? turnBoost : 0)) *
															Time.deltaTime,
										 turningSpeed);
		}
		else{
			if (currentTurnSpeed < 0){
				currentTurnSpeed = Mathf.Min(currentTurnSpeed + turndeceleration * Time.deltaTime, 0);
			}
			else if (currentTurnSpeed > 0){
				currentTurnSpeed = Mathf.Max(currentTurnSpeed - turndeceleration * Time.deltaTime, 0);
			}
		}

		//Debug.Log(currentTurnSpeed);

		carObject.transform.Rotate(0, currentTurnSpeed * Time.deltaTime, 0);
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

	public Collider GetCarCollider(){
		return carObject.GetComponent<Collider>();
	}

	public void HitTrackBounds(){
		forwardSpeed = 0;
		inertiaVector = -carObject.transform.forward;
	}
}
