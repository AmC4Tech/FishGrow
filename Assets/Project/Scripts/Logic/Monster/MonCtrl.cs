using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonCtrl : MonoBehaviour {

    public int maxHp = 100;
    public int hp = 100;
    public int atk = 3;
    public float attackCD = 1f;

    public int hungryRecv = 30;

    public int expPoint = 100;
    public int evolExp = 100;
    public int evolMonId = 1;
    public int curExp = 0;

    public float speed = 1f;
    public SpriteRenderer spriterRender;
    public Animator animator;
    public StateMachine stateMachine;

    public int hungry = 0;
    private float hungryCount = 0f;
    private MonCtrl target;

    Vector3 targetPos;
    Vector3 dir;

	// Use this for initialization
	void Start () {
        spriterRender = transform.Find("Sprite").GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        RandomTargetPos();
        //hungry = UnityEngine.Random.Range(30, 100);
        hungry = UnityEngine.Random.Range(5, 20);

        InitState();
    }
	
	// Update is called once per frame
	void Update () {
        //Wander();
        stateMachine.SMUpdate();
        UpdateData();
    }

    private void FixedUpdate()
    {
        stateMachine.SMFixedUpdate();
    }

    void RandomTargetPos() {
        targetPos = new Vector3(UnityEngine.Random.Range(-LocalData.SCENE_HALF_WEIGHT, 
            LocalData.SCENE_HALF_WEIGHT), UnityEngine.Random.Range(-LocalData.SCENE_HALF_HEIGHT, 
            LocalData.SCENE_HALF_HEIGHT), 0);
        dir = (targetPos - transform.position).normalized;
        if (dir.x < 0f)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void GetTarget() {
        target = GrowGameCtrl.GetInstance().GetCloseMon(this);
    }

    public bool IsReachTarget() {
        return Vector3.Distance(transform.position, targetPos) <= 0.5f;
    }

    public void AttackTarget() {
        if (isHasTarget()) {
            target.TakeDamage(this, atk);
        }
    }

    public void Wander() {
        if (IsReachTarget())
        {
            RandomTargetPos();
        }
        else {
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    public bool isDead() {
        if (hp <= 0)
            return true;
        return false;
    }

    public bool isHasTarget() {
        if (target && !target.isDead())
            return true;
        return false;
    }

    public void FollowTarget() {
        if (isHasTarget() && !IsReachTarget()) {
            targetPos = transform.position.x > targetPos.x ? target.transform.position + Vector3.right*3:
                target.transform.position - Vector3.right*3;
            dir = (targetPos - transform.position).normalized;
            if (dir.x < 0f)
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    public bool IsHungry() {
        return hungry <= 0;
    }

    public void UpdateData() {
        if (!IsHungry() && hungryCount >= 1f) {
            hungry--;
            hungryCount = 0f;
        }

        hungryCount += Time.deltaTime;
    }

    public void InitState() {
        stateMachine = gameObject.AddComponent<StateMachine>();

        List<SMEvent> events = new List<SMEvent>();
        Dictionary<string, Func<SMEvent, bool>> callbacks = new Dictionary<string, Func<SMEvent, bool>>();

        MonMoveState moveState = new MonMoveState();
        moveState.statemachine = stateMachine;
        moveState.monCtrl = this;
        moveState.smEvent = new SMEvent("move", new List<string>() {  "attack" }, "move");
        events.Add(moveState.smEvent);
        moveState.SetEventArray(ref callbacks);

        MonAttackState attackState = new MonAttackState();
        attackState.statemachine = stateMachine;
        attackState.monCtrl = this;
        attackState.smEvent = new SMEvent("attack", new List<string>() {"move" }, "attack");
        events.Add(attackState.smEvent);
        attackState.SetEventArray(ref callbacks);

        string initial = "move";
        stateMachine.SetupState(events, callbacks, initial, "", false);
    }

    public void TakeDamage(MonCtrl attacker, int damage) {
        hp = (hp - damage) < 0 ? 0 : hp - damage;
        if (isDead()) {
            attacker.GetExp(expPoint);
            attacker.Eat(hungryRecv);
            Dead();
        }
    }

    public void Dead() {
        GrowGameCtrl.GetInstance().RemoveMon(this);
        Destroy(gameObject);
    }

    public void GetExp(int exp) {
        curExp += exp;
        if (curExp >= evolExp && evolMonId != -1) {
            GrowGameCtrl.GetInstance().CreatMon(evolMonId, gameObject.transform.position);
            Dead();
        }
    }

    public void Eat(int recv) {
        hungry += recv;
    }

    public void PlayAnim(string animName) {
        animator.Play(animName);
    }
}
