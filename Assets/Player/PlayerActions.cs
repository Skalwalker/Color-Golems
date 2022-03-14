/// <summary>
/// Class Maping player actions and results on the game
///
/// Author: Renato Avellar Nobre - 984405
/// Class Project: Artificial Inteligence for Video Games 2021-2022
/// Version 1.0.0
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class Maping player actions and results on the game
/// </summary>
public class PlayerActions : MonoBehaviour {

    // UI to disable/enable
    public GameObject canvasGroup;

    // UI status
    private bool UIStatus = true;

    // Call actions to perform. Actions here cant envolve physiscs
    // If so they needed to be on FixedUpdate
    private void Update() {
        Actions();
    }

    /// <summary>
    /// Handle Actions. The player only have one action in the game. But this could be
    /// extended for more if needed.
    /// </summary>
    private void Actions() {
        if(Input.GetKeyDown(KeyCode.M)){
            ToggleUI();
        }

    }

    /// <summary>
    /// Enables or Disables the user interface
    /// </summary>
    private void ToggleUI() {
        UIStatus = !UIStatus;
        canvasGroup.SetActive(UIStatus);
    }
}
