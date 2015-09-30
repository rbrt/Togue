using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhysicsCarController : MonoBehaviour {

	[SerializeField] protected Rigidbody carRigidbody;	
	[SerializeField] protected GameObject carObject;

	[SerializeField] protected CameraControl cameraControl;

	bool accelerating = false,
		 turningRight = false,
		 turningLeft = false;

	Transform carTransform;
	Vector3 forceVector;
	Vector3 inertiaForce;

	SafeCoroutine inertiaForceCoroutine;

	float acceleration = 0,
		  steeringAmount = 0,
		  maxAcceleration = 20,
		  maxSteering = 50,
		  accelerationIncrease = 10f,
		  accelerationDecrease = .15f,
		  topSpeed = 30,
		  topRotation = 3;

	void Awake(){
		carTransform = carObject.GetComponent<Transform>();
		inertiaForce = Vector3.zero;
		carRigidbody.maxAngularVelocity = 2;
	}

	void Update () {
		HandleInput();
		cameraControl.SetForceVectors(carRigidbody.velocity, -carTransform.forward, carTransform.right);
	}

	void FixedUpdate(){
		ApplyForces();
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
			if (turningRight || turningLeft){
				if (inertiaForceCoroutine == null || !inertiaForceCoroutine.IsRunning){
					inertiaForce = carRigidbody.velocity;
					inertiaForceCoroutine = this.StartSafeCoroutine(InertiaFalloff());
				}
			}
			accelerating = false;
		}
	}

	void ApplyForces(){
		forceVector = -carTransform.forward;

		if (accelerating){
			acceleration = Mathf.Max(acceleration + accelerationIncrease * Time.deltaTime, maxAcceleration);
		}
		else{
			acceleration = Mathf.Min(acceleration - accelerationDecrease * Time.deltaTime, 0);
		}

		if (turningLeft){
			steeringAmount = -maxSteering;
		}
		else if (turningRight){
			steeringAmount = maxSteering;
		}
		else{
			steeringAmount = 0;
		}

		if (steeringAmount == 0){
			carRigidbody.velocity = Vector3.RotateTowards(carRigidbody.velocity, 
														  -carTransform.forward, 
														  Mathf.Deg2Rad * 10 * Time.deltaTime,
														  0);
		}

		forceVector += inertiaForce;

		if (carRigidbody.velocity.magnitude < topSpeed){
			carRigidbody.AddForce(forceVector * acceleration);
		}

		if (carRigidbody.angularVelocity.magnitude < topRotation){
			carRigidbody.AddTorque(Vector3.up * steeringAmount);
		}

		Debug.DrawRay(carObject.transform.position, carRigidbody.velocity, Color.red);
	}

	public Collider GetCarCollider(){
		return carObject.GetComponent<BoxCollider>();
	}

	public void HitTrackBounds(Vector3 collisionPoint){
		Vector3 collisionForce = (collisionPoint - carTransform.position) * -50;
		carRigidbody.AddForce(collisionForce);
	}

	IEnumerator InertiaFalloff(){
		float time = 1f;
		Vector3 originalVector = inertiaForce;
		for (float i = 0; i <= 1; i += Time.deltaTime / time){
			inertiaForce = Vector3.Lerp(originalVector, Vector3.zero, i);
			yield return null;
		}

		inertiaForce = Vector3.zero;
	}
}
