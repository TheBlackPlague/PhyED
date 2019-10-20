using UnityEngine;
using System.Collections;

using Engine.Entity;

namespace Engine {

	public class PhyEngine : MonoBehaviour {

		/**
		 * A class to manage all thr physics engines needed for necessary simulation and only to use those for optimzation.
		 */

		// Start is called before the first frame update
		void Start() {
			/*
			PhyObject obj = new PhyObject(10, 0, 5);
			Force force = NewtonianPhyEngine.GetForceGivenMassAndAcceleration(obj);
			Debug.Log(force.magnitude);
			Force weight = NewtonianPhyEngine.GetForceByGravityGivenMass(obj);
			Debug.Log(weight.magnitude);
			Force[] forces = new Force[] {
				force,
				weight
			};
			Force netForce = ForceEngine.AddArrayOfForce(forces);
			Debug.Log(netForce.magnitude);
			float acceleration = NewtonianPhyEngine.GetAccelerationGivenMassAndForce(obj, netForce);
			Debug.Log(acceleration);
			*/
		}

		// Update is called once per frame
		void Update() {

		}
	}

}
