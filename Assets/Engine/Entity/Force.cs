﻿using UnityEngine;
using System;
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
			// Right Angle conditions and inverse tangent.
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
				// Direction using the conditions and the formula: direction = inverse_tan(y / x)
				direction2D = (Mathf.Atan2(y_component, x_component)) * (180 / Mathf.PI);
			}
		}

		public Force(float direction, float _magnitude) {
			direction2D = direction;
			magnitude = _magnitude;
			// Right Angle conditions and sine cosine.
			if (direction == 0) {
				// Force is horizontally right.
				x = magnitude;
				y = 0;
			} else if (direction == 180) {
				// Force is horizontally left.
				x = -magnitude;
				y = 0;
			} else if (direction == 90) {
				// Force is vertically upwards.
				x = 0;
				y = magnitude;
			} else if (direction == 270) {
				// Force is vertically downwards.
				x = 0;
				y = -magnitude;
			} else if (direction >= 360) {
				// Values repeat after 360.
				direction = direction % 360;
				// Try the construction again after finding the non-repeated value.
				Force recursiveForce = new Force(direction, _magnitude);
				x = recursiveForce.x;
				y = recursiveForce.y;
			} else {
				// X component using the formula: V_x = V * cos(dir)
				x = magnitude * Mathf.Cos(direction * (Mathf.PI / 180));
				// Y component using the formula: V_y = V * sin(dir)
				y = magnitude * Mathf.Sin(direction * (Mathf.PI / 180));
			}
		}

	}

}
