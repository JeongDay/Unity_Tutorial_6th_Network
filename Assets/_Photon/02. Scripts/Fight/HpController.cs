using Photon.Pun;
using UnityEngine;

public class HpController : MonoBehaviourPun
{
    private Animator anim;
    private IHitbox hitbox;

    public float hp = 100f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hp <= 0)
            return;
        
        hitbox = other.GetComponent<IHitbox>();
        if (hitbox != null)
            photonView.RPC("OnGetDamage", RpcTarget.All, hitbox.Damage);
    }

    [PunRPC]
    private void OnGetDamage(float damage)
    {
        hp -= damage;
        Debug.Log($"{photonView.Owner.NickName}의 남은 체력 : {hp}");
        
        if (hp <= 0)
            Death();
    }

    private void Death()
    {
        Debug.Log($"{photonView.Owner.NickName} Die");
        anim.SetTrigger("Death");
        
        GetComponent<FightController>().SetDeath();
    }
}