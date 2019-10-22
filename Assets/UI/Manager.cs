using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Engine;
using Engine.Entity;
using TMPro;

namespace UI {

	public class Manager : MonoBehaviour {

		public GameObject forcePanel;
		public GameObject objectPanel;
		public TMP_InputField numberOfTotalForces;
		public TMP_InputField inputI;
		public TMP_InputField inputJ;
		public TMP_InputField inputK;
		public TMP_InputField inputMagnitude;
		public TMP_InputField inputDirection;
		public TMP_Dropdown objectTypeDropdown;
		public TMP_Dropdown forceOptionDropdown;
		// Stored values for comparison reasons and optimization.
		private int objectTypeDropdownValueStored = 0;
		private int numberOfTotalForcesInputValueStored = 1;
		// The maximum amount of forces we're going to add is 99. 
		private float[][] forceDataStored = new float[99][];

		public void OnValueChangedOfObjectTypeDropdown() {
			// Check if the value is same so CPU resources aren't wasted.
			if (objectTypeDropdown.value == objectTypeDropdownValueStored) {
				return;
			}
			switch (objectTypeDropdown.value) {
				case 1:
					// Activate Force Panel
					forcePanel.SetActive(true);
					objectPanel.SetActive(false);
					break;
				case 2:
					// Activate Object Panel
					objectPanel.SetActive(true);
					forcePanel.SetActive(false);
					break;
			}
		}

		public void OnValueChangedOfNumberOfForcesInputField() {
			// Get text value of the input field.
			string txt_value = numberOfTotalForces.text;
			// Check if text value is empty before trying to parse to an integer.
			if (txt_value == "") {
				return;
			}
			// Parse (convert) text value to an integer.
			int value = int.Parse(txt_value);
			// Check if the value is same so CPU resources aren't wasted.
			if (value != numberOfTotalForcesInputValueStored) {
				// Store the value for future checking.
				numberOfTotalForcesInputValueStored = value;
				// Clear the options to create new ones.
				forceOptionDropdown.ClearOptions();
				// Create a list in which to store the new option data.
				List<TMP_Dropdown.OptionData> listOfOptions = new List<TMP_Dropdown.OptionData>();
				for (int i = 1; i <= value; i++) {
					// Fill the list with the option data we create.
					TMP_Dropdown.OptionData optData = new TMP_Dropdown.OptionData("#" + i);
					listOfOptions.Add(optData);
				}
				// Add the options.
				forceOptionDropdown.AddOptions(listOfOptions);

				// Create an array with all the force data input fields.
				TMP_InputField[] forceDataInputArray = new TMP_InputField[] {
				inputI,
				inputJ,
				inputK,
				inputMagnitude,
				inputDirection
				};
				// Clear all the force data input fields without saving the data.
				foreach (TMP_InputField input in forceDataInputArray) {
					input.text = "0";
					forceDataStored = new float[99][];
				}
			}
		}

		public void SaveValueOfForceData(TMP_InputField[] forceDataInputArray) {
			// Get the float values of each force data input field.
			float i = float.Parse(forceDataInputArray[0].text);
			float j = float.Parse(forceDataInputArray[1].text);
			float k = float.Parse(forceDataInputArray[2].text);
			float magnitude = float.Parse(forceDataInputArray[3].text);
			float direction = float.Parse(forceDataInputArray[4].text);
			// Get the force # as index to save.
			int indexToSaveOn = forceOptionDropdown.value;
			Debug.Log(indexToSaveOn);
			// Save on the index.
			forceDataStored[indexToSaveOn] = new float[] {
			i,
			j,
			k,
			magnitude,
			direction
			};
		}

		public void OnValueChangedOfAnyForceDataInputField() {
			// Create an array with all the force data input fields.
			TMP_InputField[] forceDataInputArray = new TMP_InputField[] {
			inputI,
			inputJ,
			inputK,
			inputMagnitude,
			inputDirection
			};
			// Create two seperate arrays for optional inputs.
			TMP_InputField[] forceDataInputArrayOptionOne = new TMP_InputField[] {
			inputI,
			inputJ,
			inputK
			};
			TMP_InputField[] forceDataInputArrayOptionTwo = new TMP_InputField[] {
			inputMagnitude,
			inputDirection
			};
			foreach (TMP_InputField input in forceDataInputArrayOptionOne) {
				if (input.text != "" && float.Parse(input.text) != 0 && ForceDataInputFieldValueWasChanged(input, Array.IndexOf(forceDataInputArray, input))) {
					// Resolve the components.
					float i = float.Parse(forceDataInputArrayOptionOne[0].text);
					float j = float.Parse(forceDataInputArrayOptionOne[1].text);
					float k = float.Parse(forceDataInputArrayOptionOne[2].text);
					// Create a component array.
					float[] component = new float[] {
						i,
						j,
						k
					};
					// Get the calculated force.
					Force calculatedForce = ForceEngine.GetForceGivenComponent(component);
					// Convert the values to string (because text only displays strings) and set them to display.
					inputMagnitude.text = calculatedForce.magnitude.ToString();
					inputDirection.text = calculatedForce.direction2D.ToString();
					// Save all the values.
					SaveValueOfForceData(forceDataInputArray);
					return;
				}
			}
			foreach (TMP_InputField input in forceDataInputArrayOptionTwo) {
				if (input.text != "" && float.Parse(input.text) != 0 && ForceDataInputFieldValueWasChanged(input, Array.IndexOf(forceDataInputArray, input))) {
					// Resolve the magnitude and direction.
					float magnitude = float.Parse(forceDataInputArrayOptionTwo[0].text);
					float direction = float.Parse(forceDataInputArrayOptionTwo[1].text);
					// Get the calculated force.
					Force calculatedForce = ForceEngine.GetForceGivenMagnitudeAndDirection(magnitude, direction);
					// Convert the values to string (because text only displays strings) and set them to display.
					inputI.text = calculatedForce.x.ToString();
					inputJ.text = calculatedForce.y.ToString();
					inputK.text = calculatedForce.z.ToString();
					// Save all the values.
					SaveValueOfForceData(forceDataInputArray);
					return;
				}
			}
		}

		private bool ForceDataInputFieldValueWasChanged(TMP_InputField input, int type) {
			// Get which force we should check.
			int indexToCheck = forceOptionDropdown.value;
			// Check if the force's data was even saved.
			if (forceDataStored[indexToCheck] == null) {
				return true;
			}
			// If the data is saved, check if the value is different.
			if (forceDataStored[indexToCheck][type] != float.Parse(input.text)) {
				return true;
			}
			// If it isn't different, say it hasn't changed.
			return false;
		}

		public void OnValueChangedOfForceOptionDropdown() {
			// Create an array with all the force data input fields.
			TMP_InputField[] forceDataInputArray = new TMP_InputField[] {
			inputI,
			inputJ,
			inputK,
			inputMagnitude,
			inputDirection
			};
			// Clear all the fields.
			foreach (TMP_InputField input in forceDataInputArray) {
				input.text = "0";
			}
			int value = forceOptionDropdown.value;
		}

	}

}