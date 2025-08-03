using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject planetsCanvas;

    void Start()
    {
        SetPlanetInteractivity(false);
    }

    public void OnPlayButtonPressed()
    {
        SetPlanetInteractivity(true);
    }

    private void SetPlanetInteractivity(bool enabled)
    {
        PlanetController[] planets = planetsCanvas.GetComponentsInChildren<PlanetController>();
        foreach (PlanetController planet in planets)
        {
            planet.SetInteractive(enabled);
        }
    }
}
