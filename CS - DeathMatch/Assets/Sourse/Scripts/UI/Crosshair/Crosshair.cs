using UnityEngine;

[System.Serializable]
public class Parts
{
    public bool topPart, downPart, leftPart, rightPart, center;
}
public class Crosshair : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [SerializeField, Range(1f, 100f)] private float size = 10f;
    [SerializeField, Range(1f, 25f)] private float width = 2f;
    [SerializeField, Range(0f, 100f)] private float defaultSpread = 20f;
    private float resizeSpeed = 3;
    private float spread;

    private UnityEngine.Color color = UnityEngine.Color.white;
    [SerializeField] private UnityEngine.Color defaultColor = UnityEngine.Color.green;
    [SerializeField] private UnityEngine.Color enemySpottedColor = UnityEngine.Color.red;

    [SerializeField] private bool resizeCrosshair = true;
    [SerializeField] private bool hideCrosshairOnPaused;

    [SerializeField] private bool hitmarker;
    public GameObject hitmarkerObj;

    RaycastHit hit;

    [System.Serializable]
    public class Parts
    {
        public bool topPart, downPart, leftPart, rightPart, center;
    }
    public Parts parts;

    private void Start()
    {
        spread = defaultSpread;
    }

    private void Update()
    {
        if (Physics.Raycast(uiManager.mainCamera.transform.position, uiManager.mainCamera.transform.forward, out hit, 50) && hit.transform.CompareTag("Enemy"))
            color = enemySpottedColor;
        else
            color = defaultColor;

        if (spread != defaultSpread)
        {
            defaultSpread = Mathf.Lerp(defaultSpread, spread, resizeSpeed * Time.deltaTime);
        }
    }
    public void Resize(float newSize)
    {
        if (defaultSpread <= 1000 && resizeCrosshair)
            defaultSpread = Mathf.Lerp(defaultSpread, newSize, resizeSpeed * 5 * Time.deltaTime);
    }

    private void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.wrapMode = TextureWrapMode.Repeat;
        texture.Apply();

        if (parts.downPart) GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - size / 2) + defaultSpread / 2, width, size), texture);
        if (parts.topPart) GUI.DrawTexture(new Rect(Screen.width / 2 - width / 2, (Screen.height / 2 - size / 2) - defaultSpread / 2, width, size), texture);
        if (parts.rightPart) GUI.DrawTexture(new Rect((Screen.width / 2 - size / 2) + defaultSpread / 2, Screen.height / 2 - width / 2, size, width), texture);
        if (parts.leftPart) GUI.DrawTexture(new Rect((Screen.width / 2 - size / 2) - defaultSpread / 2, Screen.height / 2 - width / 2, size, width), texture);
        if (parts.center)
        {
            float radius = Mathf.Min(width, size) / 2;
            Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
            Rect circleRect = new Rect(center.x - radius, center.y - radius, radius * 2, radius * 2);

            GUI.DrawTexture(circleRect, texture);
        }
    }
}