using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class UserSettings
{
    private float mouseSensitivity;
    private float volume;
    private bool cameraBobbing;
    private bool cameraShake;
    private bool screams;

    public UserSettings(float mouseSensitivity, float volume, bool cameraBobbing, bool cameraShake, bool screams)
    {
        this.mouseSensitivity = mouseSensitivity;
        this.volume = volume;
        this.cameraBobbing = cameraBobbing;
        this.cameraShake = cameraShake;
        this.screams = screams;
    }
}