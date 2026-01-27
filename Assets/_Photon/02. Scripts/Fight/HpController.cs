using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviourPun
{
    private Animator anim;
    private IHitbox hitbox;

    public TextMeshPro nicknameText;
    public Image hpBar;

    public float currentHp = 100f;
    public float maxHp = 100f;

    void Start()
    {
        anim = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            nicknameText.text = PhotonNetwork.NickName;
            nicknameText.color = Color.green;
        }
        else
        {
            nicknameText.text = photonView.Owner.NickName;
            nicknameText.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentHp <= 0)
            return;

        hitbox = other.GetComponent<IHitbox>();
        if (hitbox != null)
            photonView.RPC("OnGetDamage", RpcTarget.All, hitbox.Damage);
    }

    [PunRPC]
    private void OnGetDamage(float damage)
    {
        currentHp -= damage;

        hpBar.fillAmount = currentHp / maxHp;

        if (currentHp <= 0)
            Death();
    }

    private void Death()
    {
        anim.SetTrigger("Death");
        GetComponent<FightController>().SetDeath();

        if (photonView.IsMine)
            Fade.onFadeAction?.Invoke(3f, Color.black, true);
    }
}