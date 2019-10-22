using UnityEngine;
using System.Collections;

namespace Engine.Entity {

	public class Force {

		/**
		 * A force entity acting on a body.
		 */

		// X, Y, Z components of the force.
		public float x;
		public float y;
		public float z;
		// The direction of the force.
		public float direction2D;
		// The magnitude of the force.
		public float magnitude;

		public Force(float x_component, float y_component, float z_component = 0) {
			x = x_component;
			y = y_component;
			z = z_component;
			// Magnitude using the formula: |v| = sqrt(x^2 + y^2 + z^2)
			magnitude = Mathf.Sqrt(Mathf.Pow(x_component, 2) + Mathf.Pow(y_component, 2) + Mathf.Pow(z_component, 2));
			// Direction using the conditions and the formula: direction = inverse_tan(y / x)
			if (x_component == 0 && Mathf.Sign(y_component) == 1) {
				// Force is vertically upwards.
				direction2D = 90;
			} else if (x_component == 0 && Mathf.Sign(y_component) == -1) {
				// Force is vertically downwards.
				direction2D = 270;
			} else if (y_component == 0 && Mathf.Sign(x_component) == 1) {
				// Force is horizontally right.
				direction2D = 0;
			} else if (y_component == 0 && Mathf.Sign(x_component) == -1) {
				// Force is horizontally left.
				direction2D = 180;
			} else if (x_component != 0 && y_component != 0) {
				// Using inverse tangent.
				direction2D = (Mathf.Atan2(y_component, x_component)) * (180 / Mathf.PI);
			}
		}

		public Force(float direction, float _magnitude) {
			direction2D = direction;
			magnitude = _magnitude;
			// X component using the formula: V_x = V * cos(dir)
			x = magnitude * Mathf.Cos(direction);
			// Y component using the formula: V_y = V * sin(dir)
			y = magnitude * Mathf.Sin(direction);
		}

	}

}