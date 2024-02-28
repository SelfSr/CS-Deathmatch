public class AiAttackEnemy : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.AttackEnemy;
    }
    public void Enter(Bot bot)
    {
        bot.aiWeapon.ActivateWeapon();
        bot.aiWeapon.SetTarget(bot.enemyTarget);
        bot.aiWeapon.SetFiring(true);
    }
    public void Update(Bot bot)
    {

    }
    public void Exit(Bot bot)
    {
        bot.aiWeapon.SetFiring(false);
        bot.aiWeapon.SetTarget(null);
        bot.aiWeapon.DeactivateWeapon();
    }
}
