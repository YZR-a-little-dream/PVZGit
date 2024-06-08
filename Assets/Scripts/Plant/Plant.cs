using UnityEngine;

public class Plant : MonoBehaviour
{
    [Header("基础属性")]
    public float health = 100;
    [SerializeField] protected float currentHealth;
    //开始执行动画参数
    protected bool start;
    protected Animator animator;
    //禁用碰撞器
    protected BoxCollider2D boxCollider2D;
    protected virtual void Start() {
        currentHealth = health;
        start = false;
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        //禁用动画
        animator.speed = 0;
        //禁用碰撞器
        boxCollider2D.enabled = false;
    }

    //种植完成后启动植物
    public void SetPlantStart()
    {
        start = true;
        //将动画速度设置为1
        animator.speed = 1;
        //启用碰撞器
        boxCollider2D.enabled = true;
    }

    public float ChangeHealth(float num)
    {
        currentHealth  = Mathf.Clamp(currentHealth + num , 0 ,health );
        if(currentHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
        return currentHealth;
    }
}
