using UnityEngine;
using System.Collections;

namespace Engine.Entity {

	public class PhyObject {

		/**
		 * An object that'll obey the laws of physics.
		 */

		public float mass;
		public float velocity;
		public float nonGravitationalAcceleration;
		public float acceleration;

		public PhyObject(float _mass = 0, float _velocity = 0, float _nonGravitationalAcceletation = 0) {
			mass = _mass;
			velocity = _velocity;
			nonGravitationalAcceleration = _nonGravitationalAcceletation;
		}

		public void setTotalAcceleration(float _acceleration) {
			acceleration = _acceleration;
		}

	}

}
