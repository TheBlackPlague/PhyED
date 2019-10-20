using UnityEngine;
using System.Collections;

using Engine.Entity;

namespace Engine {

	public class NewtonianPhyEngine {

		/**
		 * A physics engine for running calculations based on Newtonian Physics.
		 * 
		 * Newton's Laws of Motions:
		 * - 1st Law of Motion:
		 * An object remains at rest unless acted on by a force.
		 * - 2nd Law of Motion:
		 * Vector Sum of the forces applying on an object is equal to its mass and the acceleration it is experiencing. 
		 * F = ma
		 * - 3rd Law of Motion:
		 * When a force is exerted on a body, the body exerts a force equal in magnitude but opposite in direction.
		 * Every reaction has an equal but opposite reaction.
		 */

		// Gravitational Constant
		public static float G = 6.67408f * Mathf.Pow(10, -11);
		public static Force gravityForce = new Force(0, -9.80665f, 0);

		/**
		 * F = ma
		 * => a = F / m
		 * => m = F / a
		 */

		public static Force GetForceGivenMassAndAcceleration(PhyObject phyObject) {
			return GetForceGivenMassAndAcceleration(phyObject.mass, phyObject.nonGravitationalAcceleration);
		}

		public static Force GetForceGivenMassAndAcceleration(float mass, float acceleration) {
			float direction = 0;
			// If acceleration is zero, the magnitude of the force will be zero and hence it'll be facing every direction. 
			if (acceleration == 0) {
				return new Force(0, 0, 0);
			}
			if (Mathf.Sign(acceleration) == 1) {
				direction = 0;
			} else if (Mathf.Sign(acceleration) == -1) {
				direction = 180;
			}
			// using F = ma
			float magnitude = mass * Mathf.Abs(acceleration);
			return new Force(direction, magnitude);
		}
		
		
		public static float GetAccelerationGivenMassAndForce(PhyObject phyObject, Force force) {
			return GetAccelerationGivenMassAndForce(phyObject.mass, force);
		}

		public static float GetAccelerationGivenMassAndForce(float mass, Force force) {
			// using a = F / m
			return force.magnitude / mass;
		}

		public static float GetMassGivenAccelerationAndForce(PhyObject phyObject, Force force) {
			return GetMassGivenAccelerationAndForce(phyObject.acceleration, force);
		}

		public static float GetMassGivenAccelerationAndForce(float acceleration, Force force) {
			// using m = F / a
			return force.magnitude / acceleration;
		}

		// Force by Gravity => Weight 

		public static Force GetForceByGravityGivenMass(PhyObject phyObject) {
			return GetForceByGravityGivenMass(phyObject.mass);
		}

		public static Force GetForceByGravityGivenMass(float mass) {
			// Calculate Gravitational Acceleration.
			float g = gravityForce.magnitude;
			// Gravity always points vertically downwards in respect to Newtonian Physics.
			float direction = 270;
			// using F = ma = mg (since a = g)
			float magnitude = mass * Mathf.Abs(g);
			return new Force(direction, magnitude);
		}

	}

}
