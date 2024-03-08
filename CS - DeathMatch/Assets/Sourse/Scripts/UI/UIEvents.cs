using System;
using UnityEngine;

public class UIEvents
{
    public static Action<float, bool> onHealthChanged;

    public static Action<float, float> basicHealthUISetUp;

    public static Action onEnemyHit, disableWeaponUI, enableWeaponDisplay;

    public static Action<float> onInteractionProgressChanged, onHeatRatioChanged;

    public static Action<string> allowedInteraction, onEnemyKilled;

    //public static Action<WeaponController> onGenerateInspectionUI;

    public static Action<int> onInitializeDashUI, onDashUsed;

    public static Action<int, int, bool, bool> onBulletsChanged;

    public static Action<int> onUnholsteringWeapon;

    public static Action<bool, bool> onDetectReloadMethod;

    //public static Action<Weapon_SO> setWeaponDisplay;

    public static Action<GameObject> onEnableAttachmentUI;

    public static Action<int> onCoinsChange;
}
