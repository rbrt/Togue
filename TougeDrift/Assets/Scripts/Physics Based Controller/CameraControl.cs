using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField] protected GameObject cameraObject,
										  carObject,
										  cameraViewTarget,
										  followObject;

	float rotateSpeed = 40f,
		  angleScale = .75f,
		  displacementSpeed = 2.5f,
		  followSpeed = 50;

	Vector3 forwardVector = Vector3.zero,
			inertiaVector = Vector3.zero,
			rightVector = Vector3.zero,
			cameraDisplacement = Vector3.zero,
			cameraBeginningLocalPosition;

	void Awake(){
		cameraBeginningLocalPosition = cameraObject.transform.localPosition;
	}

	void FixedUpdate () {
		float speedModifier = Vector3.Distance(cameraObject.transform.position, 
											   carObject.transform.position);

		cameraObject.transform.LookAt(cameraViewTarget.transform.position);
		cameraObject.transform.position = Vector3.MoveTowards(cameraObject.transform.position,
															  followObject.transform.position, 
															  followSpeed * (speedModifier / 30) * Time.deltaTime);
		
	}

	void SetCameraDisplacement(float angle){
		float x = 0,
			  y = 1,
			  z = 0,
			  zMax = 2.5f,
			  xMax = 1.5f;

		if (angle > 15){
			x = -xMax;
			z = zMax;
		}
		else if (angle < -15){
			x = xMax;
			z = zMax;
		}
		else{
			y = 0;
		}

		cameraDisplacement.x = x;
		cameraDisplacement.y = y;
		cameraDisplacement.z = z;

		Debug.Log(cameraDisplacement + " " + angle);
	}

	public void SetForceVectors(Vector3 inertiaVector, Vector3 forwardVector, Vector3 rightVector){
		inertiaVector.y = 0;
		this.inertiaVector = inertiaVector;
		this.forwardVector = forwardVector;
		this.rightVector = rightVector;
	}
}
