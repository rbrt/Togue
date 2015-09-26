using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	[SerializeField] protected GameObject cameraObject,
										  carObject,
										  cameraViewTarget;

	float rotateSpeed = 40f,
		  angleScale = .75f,
		  displacementSpeed = 2.5f;

	Vector3 forwardVector = Vector3.zero,
			inertiaVector = Vector3.zero,
			rightVector = Vector3.zero,
			cameraDisplacement = Vector3.zero,
			cameraBeginningLocalPosition;

	void Awake(){
		cameraBeginningLocalPosition = cameraObject.transform.localPosition;
	}

	void Update () {
		cameraObject.transform.LookAt(cameraViewTarget.transform.position);

		if (Vector3.Distance(inertiaVector, Vector3.zero) > .01f){
			float angle = Vector3.Angle(inertiaVector, forwardVector) * angleScale;

			if (Vector3.Angle(inertiaVector, rightVector) <= 90){
				angle *= -1;
			}

			if (Quaternion.Angle(transform.localRotation, Quaternion.Euler(Vector3.up * angle)) > 10){
				Quaternion targetRotation = Quaternion.RotateTowards(transform.localRotation,
																 	 Quaternion.Euler(Vector3.up * angle),
															   	 	 rotateSpeed * Time.deltaTime); 
				transform.localRotation = targetRotation;
			}
			//SetCameraDisplacement(angle);
		}
		else{
			//SetCameraDisplacement(0);
			Quaternion targetRotation = Quaternion.RotateTowards(transform.localRotation,
																 	 Quaternion.Euler(Vector3.zero),
															   	 	 rotateSpeed * Time.deltaTime); 
				transform.localRotation = targetRotation;
		}

//		cameraObject.transform.localPosition = Vector3.MoveTowards(cameraObject.transform.localPosition,
//																   cameraBeginningLocalPosition + cameraDisplacement,
//																   displacementSpeed * Time.deltaTime);
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
