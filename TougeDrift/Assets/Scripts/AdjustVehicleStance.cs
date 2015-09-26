using UnityEngine;
using System.Collections;

public class AdjustVehicleStance : MonoBehaviour {

	[SerializeField] protected Transform carBody,
										 frontRightWheel,
										 frontLeftWheel,
										 rearRightWheel,
										 rearLeftWheel;

	SafeCoroutine wheelsSpinningCoroutine;

	public void SpinWheels(){
	}

	IEnumerator SpinWheelsAnimation(){
		Vector3 rotationVector = new Vector3(0,10,0);
		while (true){
			frontRightWheel.Rotate(rotationVector);
			frontLeftWheel.Rotate(rotationVector);
			rearRightWheel.Rotate(rotationVector);
			rearLeftWheel.Rotate(rotationVector);
			yield return null;
		}
	}

	void Start(){
		
	}
}
