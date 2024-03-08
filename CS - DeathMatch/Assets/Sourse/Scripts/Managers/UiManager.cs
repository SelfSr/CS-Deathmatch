using System;
using System.Collections.Generic;
using System.Net.Mail;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class UiManager : MonoBehaviour
{
    public Camera mainCamera;

    [Tooltip("Use image bars to display player statistics.")] public bool barHealthDisplay;

    [Tooltip("Use text to display player statistics.")] public bool numericHealthDisplay;

    private Action<float> healthDisplayMethod;

    [Tooltip("Slider that will display the health on screen"), SerializeField] private Slider healthSlider;

    [SerializeField, Tooltip("UI Element ( TMPro text ) that displays current and maximum health.")] private TextMeshProUGUI healthTextDisplay;

    [Tooltip("This image shows damage and heal states visually on your screen, you can change the image" +
            "to any you like, but note that color will be overriden by the script"), SerializeField]
    private Image healthStatesEffect;

    [Tooltip(" Color of healthStatesEffect on different actions such as getting hurted or healed"), SerializeField] private Color damageColor, healColor, coinCollectColor;

    [Tooltip("Time for the healthStatesEffect to fade out"), SerializeField] private float fadeOutTime;

    [Tooltip("An object showing death events will be displayed on kill")] public bool displayEvents;

    [Tooltip("UI element which contains the killfeed. Where the kilfeed object will be instantiated and parented to"), SerializeField]
    private GameObject killfeedContainer;

    [Tooltip("Object to spawn"), SerializeField] private GameObject killfeedObject;

    [Tooltip("Attach the UI you want to use as your interaction UI")] public GameObject interactUI;

    [Tooltip("Displays the current progress of your interaction"), SerializeField] private Image interactUIProgressDisplay;

    [SerializeField, Tooltip("UI that displays incompatible interactions.")] private GameObject forbiddenInteractionUI;

    [Tooltip("Inside the interact UI, this is the text that will display the object you want to interact with " +
       "or any custom method you would like." +
       "Do check Interactable.cs for that or, if you want, read our documentation or contact the cowsins support " +
       "in order to make custom interactions."), SerializeField]
    private TextMeshProUGUI interactText;

    [Tooltip("UI enabled when inspecting.")] public CanvasGroup inspectionUI;

    [SerializeField, Tooltip("Text that displays the name of the current weapon when inspecting.")] private TextMeshProUGUI weaponDisplayText_AttachmentsUI;

    [SerializeField, Tooltip("Prefab of the UI element that represents an attachment on-screen when inspecting")] private GameObject attachmentDisplay_UIElement;

    [SerializeField, Tooltip("Group of attachments. Attachment UI elements are wrapped inside these.")]
    private GameObject
        barrels_AttachmentsGroup,
        scopes_AttachmentsGroup,
        stocks_AttachmentsGroup,
        grips_AttachmentsGroup,
        magazines_AttachmentsGroup,
        flashlights_AttachmentsGroup,
        lasers_AttachmentsGroup;

    [SerializeField, Tooltip("Color of an attachment UI element when it is equipped.")] private Color usingAttachmentColor;

    [SerializeField, Tooltip("Color of an attachment UI element when it is unequipped. This is the default color.")] private Color notUsingAttachmentColor;

    [SerializeField, Tooltip("Contains dashUIElements in game.")] private Transform dashUIContainer;

    [SerializeField, Tooltip("Displays a dash slot in-game. This keeps stored at dashUIContainer during runtime.")] private Transform dashUIElement;

    [Tooltip("Attach the appropriate UI here")] public TextMeshProUGUI bulletsUI, magazineUI, reloadUI, lowAmmoUI;

    [Tooltip("Display an icon of your current weapon")] public Image currentWeaponDisplay;

    [Tooltip("Image that represents heat levels of your overheating weapon"), SerializeField] private Image overheatUI;

    [Tooltip(" Attach the CanvasGroup that contains the inventory")] public CanvasGroup inventoryContainer;

    [SerializeField] private GameObject coinsUI;

    [SerializeField] public TextMeshProUGUI coinsText;

    public Crosshair crosshair;
    public static UiManager instance { get; set; }
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (healthStatesEffect.color != new Color(healthStatesEffect.color.r,
            healthStatesEffect.color.g,
            healthStatesEffect.color.b, 0)) healthStatesEffect.color -= new Color(0f, 0f, 0f, Time.deltaTime * fadeOutTime);
    }

    // HEALTH SYSTEM /////////////////////////////////////////////////////////////////////////////////////////
    private void UpdateHealthUI(float health, bool damaged)
    {
        healthDisplayMethod?.Invoke(health);

        Color colorSelected = damaged ? damageColor : healColor;
        healthStatesEffect.color = colorSelected;

    }
    public void UpdateCoinsPanel()
    {
        healthStatesEffect.color = coinCollectColor;
    }
    private void HealthSetUp(float health, float maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }
        healthDisplayMethod?.Invoke(health);
    }
    private void BarHealthDisplayMethod(float health)
    {
        if (healthSlider != null)
            healthSlider.value = health;
    }
    private void NumericHealthDisplayMethod(float health)
    {
        if (healthTextDisplay != null)
            healthTextDisplay.text = health.ToString("F0");
        if (health <= 25)
        {
            healthTextDisplay.color = Color.red;
            healthSlider.fillRect.GetComponent<Image>().color = Color.red;
        }
    
    }

    // UI EVENTS /////////////////////////////////////////////////////////////////////////////////////////
    public void AddKillfeed(string name)
    {
        GameObject killfeed = Instantiate(killfeedObject, transform.position, Quaternion.identity, killfeedContainer.transform);
        killfeed.transform.GetChild(0).Find("Text").GetComponent<TextMeshProUGUI>().text = "You killed: " + name;
    }
    public void Hitmarker()
    {
        Instantiate(crosshair.hitmarkerObj, transform.position, Quaternion.identity, transform);
    }

    // WEAPON    /////////////////////////////////////////////////////////////////////////////////////////
    private void DetectReloadMethod(bool enable, bool useOverheat)
    {
        bulletsUI.gameObject.SetActive(enable);
        magazineUI.gameObject.SetActive(enable);
        overheatUI.transform.parent.gameObject.SetActive(useOverheat);
    }
    private void UpdateHeatRatio(float heatRatio)
    {
        overheatUI.fillAmount = heatRatio;
    }
    private void UpdateBullets(int bullets, int mag, bool activeReloadUI, bool activeLowAmmoUI)
    {
        bulletsUI.text = bullets.ToString();
        magazineUI.text = " / " + mag.ToString();
        reloadUI.gameObject.SetActive(activeReloadUI);
        lowAmmoUI.gameObject.SetActive(activeLowAmmoUI);
    }
    private void DisableWeaponUI()
    {
        overheatUI.transform.parent.gameObject.SetActive(false);
        bulletsUI.gameObject.SetActive(false);
        magazineUI.gameObject.SetActive(false);
        currentWeaponDisplay.gameObject.SetActive(false);
        reloadUI.gameObject.SetActive(false);
        lowAmmoUI.gameObject.SetActive(false);
    }
    //private void SetWeaponDisplay(Weapon_SO weapon) => currentWeaponDisplay.sprite = weapon.icon;
    private void EnableDisplay() => currentWeaponDisplay.gameObject.SetActive(true);

    // OTHERS    /////////////////////////////////////////////////////////////////////////////////////////
    public void ChangeScene(int scene) => SceneManager.LoadScene(scene);
    //public void UpdateCoins(int amount) => coinsText.text = CoinManager.Instance.coins.ToString() + " $";

    private void OnEnable()
    {
        UIEvents.onHealthChanged += UpdateHealthUI;
        UIEvents.basicHealthUISetUp += HealthSetUp;
        if (barHealthDisplay) healthDisplayMethod += BarHealthDisplayMethod;
        if (numericHealthDisplay) healthDisplayMethod += NumericHealthDisplayMethod;
        UIEvents.onEnemyHit += Hitmarker;
        UIEvents.onEnemyKilled += AddKillfeed;
        UIEvents.onDetectReloadMethod += DetectReloadMethod;
        UIEvents.onHeatRatioChanged += UpdateHeatRatio;
        UIEvents.onBulletsChanged += UpdateBullets;
        UIEvents.disableWeaponUI += DisableWeaponUI;
        //UIEvents.setWeaponDisplay += SetWeaponDisplay;
        UIEvents.enableWeaponDisplay += EnableDisplay;
        //UIEvents.onCoinsChange += UpdateCoins;

        //interactUI.SetActive(false);
    }
    private void OnDisable()
    {
        UIEvents.onHealthChanged = null;
        UIEvents.basicHealthUISetUp = null;
        healthDisplayMethod = null;
        UIEvents.allowedInteraction = null;
        UIEvents.onInteractionProgressChanged = null;
        //UIEvents.onGenerateInspectionUI = null;
        UIEvents.onInitializeDashUI = null;
        UIEvents.onDashUsed = null;
        UIEvents.onEnemyHit = null;
        UIEvents.onEnemyKilled = null;
        UIEvents.onDetectReloadMethod = null;
        UIEvents.onHeatRatioChanged = null;
        UIEvents.onBulletsChanged = null;
        UIEvents.disableWeaponUI = null;
        //UIEvents.setWeaponDisplay = null;
        UIEvents.enableWeaponDisplay = null;
    }

}
#if UNITY_EDITOR
[System.Serializable]
[CustomEditor(typeof(UiManager))]
public class UIControllerEditor : Editor
{
    private string[] tabs = { "Health", "Weapon", "Others", "UI Events" };
    private int currentTab = 0;

    override public void OnInspectorGUI()
    {
        serializedObject.Update();
        UiManager myScript = target as UiManager;

        EditorGUILayout.BeginVertical();
        EditorGUILayout.Space(10f);
        EditorGUILayout.Space(10f);
        currentTab = GUILayout.Toolbar(currentTab, tabs);
        EditorGUILayout.Space(10f);
        EditorGUILayout.EndVertical();


        if (currentTab >= 0 || currentTab < tabs.Length)
        {
            switch (tabs[currentTab])
            {
                case "Health":
                    EditorGUILayout.LabelField("HEALTH AND SHIELD", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("barHealthDisplay"));
                    if (myScript.barHealthDisplay)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("healthSlider"));
                        EditorGUI.indentLevel--;
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("numericHealthDisplay"));

                    if (myScript.numericHealthDisplay)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("healthTextDisplay"));
                        EditorGUI.indentLevel--;
                    }

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healthStatesEffect"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("damageColor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("healColor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("coinCollectColor"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeOutTime"));

                    break;
                case "Weapon":

                    EditorGUILayout.LabelField("WEAPON", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("magazineUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("overheatUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("lowAmmoUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("currentWeaponDisplay"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("inventoryContainer"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("crosshair"));
                    break;

                case "Others":
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("coinsUI"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("coinsText"));
                    break;
                case "UI Events":

                    EditorGUILayout.LabelField("EVENTS", EditorStyles.boldLabel);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("displayEvents"));
                    if (myScript.displayEvents)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("killfeedContainer"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("killfeedObject"));
                    }

                    break;
            }
        }
        serializedObject.ApplyModifiedProperties();

    }
}
#endif