using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

using TMPro;

namespace UI.MenuScene {

	public class Manager : MonoBehaviour {

		private float currentTime;
		private float loadingTime = 15f;

		public void Start() {
			currentTime = loadingTime;
		}

		public void Update() {
			currentTime -= 1 * Time.deltaTime;
			if (currentTime <= 0) {
				SceneManager.LoadSceneAsync(1);
			}
			if (currentTime <= -5) {
				SceneManager.UnloadSceneAsync(0);
			}
		}

	}

}