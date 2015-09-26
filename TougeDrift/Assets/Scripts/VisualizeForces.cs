using UnityEngine;
using System.Collections;

public class VisualizeForces : MonoBehaviour {

	[SerializeField] protected LineRenderer forwardForce,
								  			inertiaForce;

	Vector3 inertiaVector = Vector3.zero,
			forwardVector = Vector3.zero;

	public void SetForces(Vector3 forwardVector, Vector3 inertiaVector){
		this.forwardVector = forwardVector;
		this.inertiaVector = inertiaVector;
	}

	void Update () {
		forwardForce.SetPosition(0, (transform.position) + Vector3.up);
		inertiaForce.SetPosition(0, (transform.position) + Vector3.up);

		forwardForce.SetPosition(1, (transform.position + forwardVector * 10) + Vector3.up);
		inertiaForce.SetPosition(1, (transform.position + inertiaVector * 10) + Vector3.up);
	}
}
