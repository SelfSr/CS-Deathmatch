using System.Collections;
using UnityEngine;
public class AiAttackTargetState : AiState
{
    private bool hasSwitchedWeapon = false;

    private float shootingDelayTimer = 0f;

    public AiStateId GetId()
    {
        return AiStateId.AttackTarget;
    }

    public void Enter(Ai_Bot bot)
    {
        bot.navMeshAgent.stoppingDistance = bot.stoppingDistance;
    }

    public void Update(Ai_Bot bot)
    {
        if (!bot.targetingSystem.HasTarget)
        {
            bot.aiWeapon.SetTarget(null);
            bot.navMeshAgent.ResetPath();

            bot.aiWeapon.SwitchWeapon(AiWeapon.WeaponSlot.Primary);

            bot.stateMachine.ChangeState(AiStateId.FindTarget);
            hasSwitchedWeapon = false;
            return;
        }

        bot.aiWeapon.SetTarget(bot.targetingSystem.Target.transform);
        bot.navMeshAgent.destination = bot.targetingSystem.TargetPosition;

        float distance = bot.targetingSystem.TargetDistance;
        if (!hasSwitchedWeapon && distance <= bot.closeDistance && bot.aiWeapon.currentWeapon.ammoCount <= 3)
        {
            bot.aiWeapon.SwitchWeapon(AiWeapon.WeaponSlot.Secondary);
            hasSwitchedWeapon = true;
        }

        if (bot.isCanShoot)
        {
            ReloadWeapon(bot);

            shootingDelayTimer += Time.deltaTime;
            if (shootingDelayTimer >= bot.shootingDelayDuration)
            {
                UpdateFiring(bot);
                shootingDelayTimer = 0f;
            }
        }
    }
    public void Exit(Ai_Bot bot)
    {
        bot.navMeshAgent.stoppingDistance = 0.0f;
    }

    private void UpdateFiring(Ai_Bot bot)
    {
        if (bot.sensor.IsInSight(bot.targetingSystem.Target))
        {
            bot.aiWeapon.SetFiring(true);
        }
        else
        {
            bot.aiWeapon.SetTarget(null);
            bot.aiWeapon.SetFiring(false);
        }
    }

    private void ReloadWeapon(Ai_Bot bot)
    {
        var weapon = bot.aiWeapon.currentWeapon;
        if (weapon && weapon.ammoCount <= 0)
        {
            bot.aiWeapon.ReloadWeapon();
        }
    }
}