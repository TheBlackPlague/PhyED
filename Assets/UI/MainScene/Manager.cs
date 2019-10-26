using UnityEngine;
using System;
using System.Collections.Generic;

using Engine;
using Engine.Entity;
using TMPro;

namespace UI.MainScene {

	public class Manager : MonoBehaviour {

		public GameObject forcePanel;
		public GameObject objectPanel;
		public GameObject resultForcePanel;
		public GameObject graphVector;
		public TMP_InputField numberOfTotalForces;
		public TMP_InputField inputI;
		public TMP_InputField inputJ;
		public TMP_InputField inputK;
		public TMP_InputField inputMagnitude;
		public TMP_InputField inputDirection;
		public TMP_InputField inputIResult;
		public TMP_InputField inputJResult;
		public TMP_InputField inputKResult;
		public TMP_InputField inputMagnitudeResult;
		public TMP_InputField inputDirectionResult;
		public TMP_Dropdown objectTypeDropdown;
		public TMP_Dropdown forceOptionDropdown;
		// Stored values for comparison reasons and optimization.
		private int objectTypeDropdownValueStored = 0;
		private int numberOfTotalForcesInputValueStored = 1;
		// The maximum amount of forces we're going to add is 99. 
		private float[][] forceDataStored = new float[99][];
		// Multiple Graph Vectors saved here. Need to figure out how I'm going to create & destroy these. :/
		private GameObject[] graphVectorStored = new GameObject[99];

		public void OnValueChangedOfObjectTypeDropdown() {
			// Check if the value is same so CPU resources aren't wasted.
			if (objectTypeDropdown.value == objectTypeDropdownValueStored) {
				return;
			}
			switch (objectTypeDropdown.value) {
				case 0:
					// Return to default panel.
					forcePanel.SetActive(false);
					objectPanel.SetActive(false);
					break;
				case 1:
					// Activate Force Panel.
					forcePanel.SetActive(true);
					objectPanel.SetActive(false);
					break;
				case 2:
					// Activate Object Panel.
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
				// Set the graph vector's directions.
				graphVector.transform.rotation = Quaternion.Euler(0, 0, 0);
				// Turn off the graph vector because it isn't valuable here. 
				graphVector.SetActive(false);
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
				// Make sure the field is empty and that the value was changed.
				if (input.text != "" && ForceDataInputFieldValueWasChanged(input, Array.IndexOf(forceDataInputArray, input))) {
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
					// Check if vector needs to be displayed or not.
					if (calculatedForce.magnitude > 0) {
						// Turn on the graph vector. 
						graphVector.SetActive(true);
					} else {
						// Turn off the graph vector because it isn't valuable here. 
						graphVector.SetActive(false);
					}
					// Set the graph vector's directions.
					graphVector.transform.rotation = Quaternion.Euler(0, 0, calculatedForce.direction2D);
					// Save all the values.
					SaveValueOfForceData(forceDataInputArray);
					return;
				}
			}
			foreach (TMP_InputField input in forceDataInputArrayOptionTwo) {
				// Make sure the field is empty and that the value was changed.
				if (input.text != "" && ForceDataInputFieldValueWasChanged(input, Array.IndexOf(forceDataInputArray, input))) {
					// Resolve the magnitude and direction.
					float magnitude = float.Parse(forceDataInputArrayOptionTwo[0].text);
					float direction = float.Parse(forceDataInputArrayOptionTwo[1].text);
					// If force is given with negative value on the horizontal just via magnitude.
					if (Math.Sign(magnitude) == -1 && direction == 0) {
						direction = 180;
					}
					// Get the calculated force.
					Force calculatedForce = ForceEngine.GetForceGivenMagnitudeAndDirection(magnitude, direction);
					// Convert the values to string (because text only displays strings) and set them to display.
					inputI.text = calculatedForce.x.ToString();
					inputJ.text = calculatedForce.y.ToString();
					inputK.text = calculatedForce.z.ToString();
					// Check if vector needs to be displayed or not.
					if (magnitude > 0) {
						// Turn on the graph vector. 
						graphVector.SetActive(true);
					} else {
						// Turn off the graph vector because it isn't valuable here. 
						graphVector.SetActive(false);
					}
					// Set the graph vector's directions.
					graphVector.transform.rotation = Quaternion.Euler(0, 0, direction);
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
				// In case it hasn't been saved, checks if the value of it was different from the original values.
				if (input.text == "0") {
					return false;
				}
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
			// Check if the values have been saved before. If not, no need to search it up.
			int indexToCheck = forceOptionDropdown.value;
			if (forceDataStored[indexToCheck] == null) {
				// Turn off the graph vector because it isn't valuable here. 
				graphVector.SetActive(false);
				// Set the graph vector's directions.
				graphVector.transform.rotation = Quaternion.Euler(0, 0, 0);
				return;
			}
			// Since the values are saved. Display the previous values.
			// Convert the values to string (because text only displays strings) and set them to display.
			inputI.text = forceDataStored[indexToCheck][0].ToString();
			inputJ.text = forceDataStored[indexToCheck][1].ToString();
			inputK.text = forceDataStored[indexToCheck][2].ToString();
			inputMagnitude.text = forceDataStored[indexToCheck][3].ToString();
			inputDirection.text = forceDataStored[indexToCheck][4].ToString();
			// Check if vector needs to be displayed or not.
			if (forceDataStored[indexToCheck][3] > 0) {
				// Turn on the graph vector. 
				graphVector.SetActive(true);
			} else {
				// Turn off the graph vector because it isn't valuable here. 
				graphVector.SetActive(false);
			}
			// Set the graph vector's directions.
			graphVector.transform.rotation = Quaternion.Euler(0, 0, forceDataStored[indexToCheck][4]);
		}

		public void OnAddForceButtonClick() {
			// Check if more than one force has been selected.
			if (int.Parse(numberOfTotalForces.text) > 1) {
				// Create an array which will only store defined values.
				float[][] forceDataStoredDefined = new float[99][];
				foreach (float[] set in forceDataStored) {
					// Check if values were actually saved or not.
					if (set != null) {
						// Find a not yet defined value in the array.
						int indexToSaveOn = Array.IndexOf(forceDataStoredDefined, null);
						// In case the array is full - logically speaking, it should never be full for all values to be added.
						if (indexToSaveOn != -1) {
							forceDataStoredDefined[indexToSaveOn] = set;
						}
					}
				}
				// Check if more than one value was defined.
				if (Array.IndexOf(forceDataStoredDefined, null) > 1) {
					// Create an array of forces.
					Force[] arrayOfForce = new Force[99];
					foreach (float[] forceValue in forceDataStoredDefined) {
						// Checks if the value is null and all forces have beem created.
						if (forceValue == null) {
							break;
						}
						float x = forceValue[0];
						float y = forceValue[1];
						float z = forceValue[2];
						// Find a not yet defined value in the array.
						int indexToSaveOn = Array.IndexOf(arrayOfForce, null);
						// In case the array is full - logically speaking, it should never be full for all values to be added.
						if (indexToSaveOn != -1) {
							// Create the force and store.
							arrayOfForce[indexToSaveOn] = new Force(x, y, z);
						}
					}
					// Check if more than one value was defined.
					if (Array.IndexOf(arrayOfForce, null) > 1) {
						// Calculated the Resultant Force.
						Force resultForce = ForceEngine.AddArrayOfForce(arrayOfForce);
						// Turn on the result panel.
						resultForcePanel.SetActive(true);
						// Convert the values to string (because text only displays strings) and set them to display.
						inputIResult.text = resultForce.x.ToString();
						inputJResult.text = resultForce.y.ToString();
						inputKResult.text = resultForce.z.ToString();
						inputMagnitudeResult.text = resultForce.magnitude.ToString();
						inputDirectionResult.text = resultForce.direction2D.ToString();
						// Check if vector needs to be displayed or not.
						if (resultForce.magnitude > 0) {
							// Turn on the graph vector. 
							graphVector.SetActive(true);
						} else {
							// Turn off the graph vector because it isn't valuable here. 
							graphVector.SetActive(false);
						}
						// Set the graph vector's directions.
						graphVector.transform.rotation = Quaternion.Euler(0, 0, resultForce.direction2D);
					}
				}
			}
		}

	}

}