#if UNITY_EDITOR
using UnityEditor.Presets;
#endif
using UnityEngine;


public class CrosshairShape : MonoBehaviour
{
#if UNITY_EDITOR
    public Preset currentPreset;

    public Preset defaultPreset;
#endif
    [System.Serializable]
    public class Parts
    {
        public bool topPart, downPart, leftPart, rightPart, center;
    }

    public Parts parts;

    public string presetName;

#if UNITY_EDITOR
    private void Awake() => ResetCrosshair(defaultPreset);

    private void ResetCrosshair(Preset preset) => Utilities.ApplyPreset(preset, this);
#endif

}