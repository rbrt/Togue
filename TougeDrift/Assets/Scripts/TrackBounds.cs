using UnityEngine;
using System.Collections;

public class TrackBounds : MonoBehaviour {

	[SerializeField] protected PhysicsCarController carController;

	void OnCollisionEnter(Collision info){
		if (info.collider == carController.GetCarCollider()){
			carController.HitTrackBounds();
		}
	}
}
