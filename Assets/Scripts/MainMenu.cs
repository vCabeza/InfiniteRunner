﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Firebase.Auth;

using UnityEngine;

public class MainMenu : MonoBehaviour {

    public void newGame() {
        SceneManager.LoadScene(1);
    }
}
