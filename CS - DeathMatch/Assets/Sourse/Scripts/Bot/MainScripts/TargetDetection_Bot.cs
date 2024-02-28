using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDetection_Bot : MonoBehaviour
{
    private const string PLAYER = "Player";
    private const string ENEMY = "Enemy";
    private const string ENVIRONMENT = "Environment";
    private const string BOT_LIBM_SHOT = "BotLimbShot";

    private Bot mainBotScript;

    [HideInInspector] public List<Collider> targetList;
    private Coroutine checkVisiableCoroutine;

    private float distanceRay = 50f;

    private void Start()
    {
        mainBotScript = GetComponentInParent<Bot>();
        checkVisiableCoroutine = StartCoroutine(CheckVisiable());
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(ENEMY) || other.CompareTag(PLAYER)) && other != mainBotScript.thisBotCollaider)
        {
            if (targetList.Count < mainBotScript.botConfig.targetCount)
            {
                targetList.Add(other);
            }
            else
            {
                CheckDistanceToTarget(other);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        targetList.Remove(other);
    }
    IEnumerator CheckVisiable()
    {
        while (mainBotScript.thisBotCollaider.gameObject.activeSelf == true)
        {
            yield return null /*new WaitForSeconds(botSettings.timeCheckVisiableTarget)*/;

            if (targetList.Count != 0)
            {
                foreach (var target in targetList)
                {
                    var coor = mainBotScript.thisBotCollaider.transform.position;
                    Ray rayUP = new Ray(new Vector3(coor.x, coor.y + 1.9f, coor.z), target.transform.position - new Vector3(coor.x, coor.y + 0.3f, coor.z));
                    Debug.DrawRay(new Vector3(coor.x, coor.y + 1.9f, coor.z), target.transform.position - new Vector3(coor.x, coor.y + 0.3f, coor.z), Color.blue);

                    RaycastHit hitUP;
                    if (Physics.Raycast(rayUP, out hitUP, distanceRay, mainBotScript.botConfig.layerMask) && CheckTag(hitUP.collider))
                    {
                        //mainBotScript.stateMachine.ChangeState(AiStateId.AttackEnemy);
                    }
                    else
                    {
                        RaycastHit hitDOWN;
                        Ray rayDOWN = new Ray(new Vector3(coor.x, coor.y + 1.9f, coor.z), target.transform.position - new Vector3(coor.x, coor.y + 1.65f, coor.z));
                        Debug.DrawRay(new Vector3(coor.x, coor.y + 1.9f, coor.z), target.transform.position - new Vector3(coor.x, coor.y + 1.65f, coor.z), Color.cyan);

                        if (Physics.Raycast(rayDOWN, out hitDOWN, distanceRay, mainBotScript.botConfig.layerMask) && CheckTag(hitDOWN.collider))
                        {
                            //Debug.Log(hitDOWN.collider.name);
                            break;
                        }
                    }
                }
            }
        }
    }
    private bool CheckTag(Collider hit)
    {
        return !hit.CompareTag(ENVIRONMENT) && !hit.CompareTag(BOT_LIBM_SHOT);
    }
    private void CheckDistanceToTarget(Collider enemy)
    {
        float distanceTofarthestTarget = 0;
        Collider farthestObject = null;
        float distanceToNewTarget = Vector3.Distance(mainBotScript.thisBotCollaider.transform.position, enemy.transform.position);
        foreach (var target in targetList)
        {
            float distanceToOldTarget = Vector3.Distance(mainBotScript.thisBotCollaider.transform.position, target.transform.position);
            if (distanceTofarthestTarget < distanceToOldTarget)
            {
                distanceTofarthestTarget = distanceToOldTarget;
                farthestObject = target;
            }
        }
        if (distanceTofarthestTarget > distanceToNewTarget)
        {
            targetList.Remove(farthestObject);
            targetList.Add(enemy);
        }
    }
}