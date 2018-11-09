﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{

    public MusicManager musicManager;
    private Animator animator;

    [SerializeField]
    public int playerId;

    public Move currentMove;
    private Move idleMove;
    private Move blockMove;
    private Move staggerMove;

    private int currentFrame = 0;
    private int transitionTime = -1;
    private Move nextMove = null;

    private bool isStunned = false;

    public BoxCollider2D attackZone;

    private bool attackZoneActivated = false;

    private new Rigidbody2D rigidbody;

    public bool isControllable = false;

    public DamageRumble m_deathRumbel;
    public DamageRumble m_lightAttackRumbel;
    public DamageRumble m_strongAttackRumbel;
    public DamageRumble m_blockRumbel;

    float soundTimer = 0.0f;
    float soundTimerThreshold;

    void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();

        soundTimerThreshold = Random.Range(3.0f, 5.0f);
    }

    // Use this for initialization
    void Start()
    {
        blockMove = new Move("Block", 30, 25, 5, 30, null, null, null, Vector2.zero, Vector2.zero, int.MaxValue, -1, m_blockRumbel);

        Move firstLightAttack = null;
        Move firstHeavyAttack = null;

        if (playerId == 1)
        {
            MoveCreator.CreateMovesForSerialkiller(ref firstLightAttack, ref firstHeavyAttack, blockMove, m_strongAttackRumbel, m_lightAttackRumbel);
        }
        if (playerId == 2)
        {
            MoveCreator.CreateMovesForClown(ref firstLightAttack, ref firstHeavyAttack, blockMove, m_strongAttackRumbel, m_lightAttackRumbel);
        }

        staggerMove = new Move("Stagger", 15, 14, 0, 1000, firstLightAttack, firstHeavyAttack, blockMove, Vector2.zero, Vector2.zero, int.MaxValue, -1, m_blockRumbel);

        idleMove = new Move("Idle", -1, -1, -1, -1, firstLightAttack, firstHeavyAttack, blockMove, Vector2.zero, Vector2.zero, int.MaxValue, -1, null);
        idleMove.loop = true;

        currentMove = idleMove;
    }

    void FixedUpdate()
    {
        rigidbody.velocity = Vector2.zero;

        currentFrame++;

        if (currentMove.displacement != 0 && currentFrame >= currentMove.displacementStart && currentFrame <= currentMove.displacementEnd)
        {
            var displacementDur = currentMove.displacementEnd - currentMove.displacementStart;
            var dis = currentMove.displacement / displacementDur;
            if (playerId == 2)
                dis = -dis;
            rigidbody.MovePosition(rigidbody.position + new Vector2(dis, 0));
        }

        if (!attackZoneActivated)
        {
            if ((currentFrame >= currentMove.attackZoneStart))
            {
                //activate Hitbox
                attackZone.size = currentMove.attackZoneSize;
                attackZone.offset = playerId == 1 ? currentMove.attackZoneCenter : -1 * currentMove.attackZoneCenter;
                attackZone.enabled = true;
                attackZoneActivated = true;
                attackZone.gameObject.GetComponent<AttackZone>().DoOnEnable();
            }
        }
        else
        {
            if (currentFrame >= currentMove.attackZoneEnd)
            {
                attackZone.enabled = false;
                attackZone.gameObject.GetComponent<AttackZone>().DoOnDisable();
            }
        }


    }

    private void OnAttack(Move next)
    {
        if (next == null)
            return;

        if (isStunned)
            return;

        if (!currentMove.loop && (currentFrame < currentMove.inputTimeStart || currentFrame > currentMove.inputTimeEnd))
            return;
        
        if (currentFrame > currentMove.cancelTime)
            transitionTime = currentFrame;
        else
        {
            transitionTime = currentMove.cancelTime;
        }

        nextMove = next;
    }

    private void NextMove(Move m)
    {
        if (m == null)
            m = idleMove;

        animator.SetTrigger(m.name);

        transitionTime = -1;
        currentMove = m;
        if(currentMove.missName != null)
        {
            musicManager.PlaySound(currentMove.missName, transform.position);
        }

        nextMove = null;
        currentFrame = 0;
        attackZoneActivated = false;
        attackZone.enabled = false;
    }

    public void Stagger(int duration)
    {
        staggerMove.duration = duration;
        staggerMove.cancelTime = duration;
        NextMove(staggerMove);
    }

    private bool CanMove()
    {
        if (!isControllable)
            return false;

        if (currentMove == idleMove)
            return true;

        if (currentMove.cancelTime >= 0 && currentFrame >= currentMove.cancelTime)
            return true;

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove())
        {
            soundTimer += Time.deltaTime;
            if(soundTimer > soundTimerThreshold)
            {
                var sound = playerId == 1 ? "SK_Breath_Light" : "Clown_Laugh_Light";
                musicManager.PlaySound(sound, transform.position, 0.5f);

                soundTimerThreshold = Random.Range(3.0f, 5.0f);
                soundTimer = 0.0f;
            }
            
            var pos = InputManager.horizontal(playerId);
            var dir = 0;
            if (pos < 0)
                dir = -1;
            else if (pos > 0)
                dir = 1;

            rigidbody.MovePosition(rigidbody.position + new Vector2(dir, 0) * (Time.deltaTime * 20));

            if (dir != 0 && currentMove != idleMove)
                NextMove(idleMove);

            if (playerId == 2)
                dir = -dir;
            animator.SetInteger("Direction", dir);
        }


        if(isControllable)
        {
            if (InputManager.x_Button_down(playerId))
                OnAttack(currentMove.onLightAttack);
            if (InputManager.b_Button_down(playerId))
                OnAttack(currentMove.onHeavyAttack);
            if (InputManager.rb_Button_down(playerId))
                OnAttack(currentMove.onBlock);
        }


        if (currentFrame >= transitionTime && transitionTime >= 0)
        {
            NextMove(nextMove);
        }

        if(currentMove.duration >= 0 && currentFrame > currentMove.duration)
        {
            NextMove(nextMove);
        }
    }

    public Move getBlockMove()
    {
        return blockMove;
    }

    public IEnumerator Knockback(float duration, float power, int direction)
    {
        float timer = 0;

        while(duration > timer)
        {
            rigidbody.MovePosition(rigidbody.position + new Vector2(direction * power * Time.deltaTime, 0));

            timer += Time.deltaTime;
        }

        yield return 0;
    }
}
