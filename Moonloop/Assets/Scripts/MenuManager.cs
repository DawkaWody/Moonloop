using System.Collections;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject planetsCanvas;
    [SerializeField] private Crossfade _crossfade;

    IEnumerator Start()
    {
        SetPlanetInteractivity(false);
        yield return new WaitForEndOfFrame();
        _crossfade.initialFadein = true;
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
