// ========================================================
// 作者：娇娇 
// 创建时间：2026-04-11 18:08:25
// 版本：V1.1
// 描述：
// ========================================================

using UnityEditor;
using UnityEngine;

public class SkillObject_TimeEcho : SkillObject_Base
{
    [SerializeField] private float wispMoveSpeed = 15;
    [SerializeField] private GameObject onDeathVfx;
    [SerializeField] private LayerMask whatIsGround;
    private bool shouldMoveToPLayer;

    private Transform playerTransform;
    private Skill_TimeEcho echoManager;
    private TrailRenderer wispTrail;
    private Entity_Health echoHealth;
    private Entity_Health playerhealth;
    private Player_SkillManager skillManager;
    private Entity_StatusHandler statusHandler;

    public int maxAttacks {  get; private set; }

    public void SetupEcho(Skill_TimeEcho echoManager)
    {
        this.echoManager = echoManager;
        playerStats = echoManager.player.stats;
        damageScaleData = echoManager.damageScaleData;
        maxAttacks = echoManager.GetMaxAttacks();
        playerTransform = echoManager.transform.root;
        playerhealth = echoManager.player.health;
        skillManager = echoManager.skillManager;
        statusHandler = echoManager.player.statusHandler;   

        Invoke(nameof(HandleDeath), echoManager.GetEchoDuration());
        FilpToTarget();

        echoHealth = GetComponent<SKillObject_Health>();
        wispTrail = GetComponentInChildren<TrailRenderer>();
        wispTrail.gameObject.SetActive(false);

        anim.SetBool("canAttack",maxAttacks > 0);
    }
    private void Update()
    {
        if (shouldMoveToPLayer)
            HandleWispMovement();
        else
        {
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
            StopHorizontalMovement();
        }
    }

    private void HandlePlayerTouch()
    {
        float healAmount = echoHealth.lastDamageTaken * echoManager.GetPercentOfDamageHealed();
        playerhealth.IncreaseHealth(healAmount);

        float amountInSeconds = echoManager.GetCooldownReduceInSeconds();
        skillManager.ReduceAllSkillCooldownBy(amountInSeconds);

        if(echoManager.CanRemoveNegativeEffects())
            statusHandler.RemoveAllNegativeEffects();
    }
    private void HandleWispMovement()
    {
        transform.position = Vector2.MoveTowards(transform.position,playerTransform.position,wispMoveSpeed * Time.deltaTime);
        
        if(Vector2.Distance(transform.position,playerTransform.position) < .5f)
        {
            HandlePlayerTouch();
            Destroy(gameObject);
        }
    }

    private void FilpToTarget()
    {
        Transform target = FindClosestTarget();
        if (target.position.x < transform.position.x)
            transform.Rotate(1, 180, 0);
    }
    public void PerformAttack()
    {
        DamageEnemiesInRadius(targetCheck, 1);

        if (targetGotHit == false)
            return;

        bool canDuplicate = Random.value < echoManager.GetDuplicateChance();
        float xOffset = transform.position.x < lastTarget.position.x ? 1 : -1;

        if (canDuplicate)
            echoManager.CreateTimeEcho(lastTarget.position + new Vector3(xOffset, 0));
    }
    public void HandleDeath()
    {
        Instantiate(onDeathVfx, transform.position, Quaternion.identity);

        if (echoManager.ShouldBeWisp())
            TurnIntoWisp();
        else
            Destroy(gameObject);
    }

    private void TurnIntoWisp()
    {
        shouldMoveToPLayer = true;
        anim.gameObject.SetActive(false);
        wispTrail.gameObject.SetActive(true);
        rb.simulated = false;
    }

    private void StopHorizontalMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, whatIsGround);

        if(hit.collider != null)
            rb.linearVelocity = new Vector2(0,rb.linearVelocity.y);
    }
}
