﻿using UnityEngine;
using System.Collections;

using Engine.Entity;

namespace Engine {

	public class ForceEngine {

		/**
		 * An engine to manage the resolving of forces.
		 */

		public static Force AddTwoForce(Force f1, Force f2) {
			// Add the relevant components of the two forces to create resultant components.
			float r_x = f1.x + f2.x;
			float r_y = f1.y + f2.y;
			float r_z = f1.z + f2.z;
			// Create & return the resultatnt force using resultant components.
			Force resultant = new Force(r_x, r_y, r_z);
			return resultant;
		}

		public static Force AddArrayOfForce(Force[] forces) {
			// Create a point force.
			Force resultant = new Force(0, 0, 0);
			// Add the forces together.
			foreach (Force force in forces) {
				// In case the array has 1 or more undefined indexes. 
				if (force == null) {
					return resultant;
				}
				resultant = AddTwoForce(resultant, force);
			}
			// Return the resultant. 
			return resultant;
		}

		public static Force GetForceGivenComponent(float[] component) {
			// Components are in order (array order matters!)
			// component[0] => i
			// component[1] => j
			// component[2] => k
			return new Force(component[0], component[1], component[2]);
		}

		public static Force GetForceGivenMagnitudeAndDirection(float magnitude, float direction) {
			// Return the force created.
			return new Force(direction, magnitude);
		}

	}

}
