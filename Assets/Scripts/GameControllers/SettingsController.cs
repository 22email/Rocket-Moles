using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Events;

// This is mostly for the in-game settings menu & pause menu interactions
public class SettingsController : MonoBehaviour
{
    private static SettingsController instance;
    public static SettingsController Instance { get => instance; }
    public UserSettings userSettings;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public void AdjustMouseSensitivity(float sensitivity){}
    public void AdjustVolume(float volume){}
    public void ToggleCameraBobbing(bool hey){print(hey);}
    public void ToggleCameraShake(){}
    public void ToggleScreams(){}

}
