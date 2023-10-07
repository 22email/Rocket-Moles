using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;

// This is mostly for the in-game settings menu & pause menu interactions
public class SettingsMenu : MonoBehaviour
{
    private bool showingSettings;
    public bool ShowingSettings {get => showingSettings; set => showingSettings = value;}

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private UnityEvent disableSettings;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(showingSettings)
            {
                disableSettings.Invoke();
            }

        }
    }
}
