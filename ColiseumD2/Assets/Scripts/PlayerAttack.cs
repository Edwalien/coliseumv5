using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Coliseum
{
    public class PlayerAttack : MonoBehaviour
    {
        private PhotonView photonView;
        
        public GameObject Hand;
        public Weapon myWeapon;
        public float initDmg;
        public float initVit;
        public Animator anim;
        private playerMove EnemyKb;
        private float Timer;
        private int c;
        private playerMove pM;
        private bool Hit;

        private PlayerHealth pH;

        void Start()
        {
            photonView = GetComponent<PhotonView>();
            myWeapon = Hand.GetComponentInChildren<Weapon>();
            initDmg = myWeapon.attackDamage;
            anim = GetComponent<Animator>();
            pH = GetComponent<PlayerHealth>();
            pM = GetComponent<playerMove>();
            initVit = pM.speed;
        }

        void Update()
        {
            anim.SetBool("leftClick", Input.GetMouseButtonUp(0) && Timer > myWeapon.cooldown && !pH.shield);
            if (photonView.IsMine)
            {
                Timer += Time.deltaTime;
                Debug.DrawRay(Hand.transform.position, transform.forward * myWeapon.attackRange);
                if (Input.GetMouseButtonUp(0) && Timer > myWeapon.cooldown)
                {
                    DoAttack();
                    if (myWeapon.weapon == "marteau" && Hit == false)
                    {
                        FindObjectOfType<AudioManager>().Play("marteau_miss");
                    }
                    if (myWeapon.weapon == "lance" && !Hit)
                    {
                        FindObjectOfType<AudioManager>().Play("spear_miss");
                    }
                    if (myWeapon.weapon == "lame" || myWeapon.weapon == "claymore" && !Hit)
                    {
                        FindObjectOfType<AudioManager>().Play("blade_miss");
                    }
                    c++;
                    Timer = 0f;
                }
            }
        }

        private void DoAttack()
        {
            float damage = myWeapon.attackDamage;

            if (c > 2)
            {
                damage *= 1.5f;
                c = 0;
            }
            
            Ray ray1 = new Ray(Hand.transform.position, transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray1, out hit, myWeapon.attackRange))
            {
                if(hit.collider.CompareTag("Player"))
                {
                    RpcTarget target = hit.collider.GetComponent<RpcTarget>();
                    PhotonView enemyView = hit.transform.GetComponent<PhotonView>();
                    PlayerHealth enemyHealth = hit.transform.GetComponent<PlayerHealth>();
                    Hit = true;
                    if (myWeapon.weapon == "marteau" && Hit)
                    {
                        FindObjectOfType<AudioManager>().Play("marteau_hit");
                    }
                    if (myWeapon.weapon == "lance" && Hit)
                    {
                        FindObjectOfType<AudioManager>().Play("spear_hit");
                    }
                    if (myWeapon.weapon == "lame" || myWeapon.weapon == "claymore" && !Hit)
                    {
                        FindObjectOfType<AudioManager>().Play("blade_hit");
                    }
                    Hit = false;
                    enemyView.RPC("Damage", target, damage);
                    enemyView.RPC("Knockback", target,(Hand.transform.forward * myWeapon.knockback));
                }

                if (hit.collider.CompareTag("Bonus"))
                {
                    PlayerHealth bonusHealth = hit.collider.GetComponent<PlayerHealth>();
                    string type = bonusHealth.type;

                    if (bonusHealth.health <= damage)
                    {
                        if (type == "vie")
                        {
                            pH.health += 30f;
                        }

                        if (type == "force")
                        {
                            initDmg *= 2;
                            Invoke("SetDamageNormal", 300);
                        }

                        if (type == "vitesse")
                        {
                            pM.speed *= 1.5f;
                            Invoke("SetSpeedNormal", 30);
                        }
                        
                        Debug.Log("Bonus died !");
                    }

                    bonusHealth.health -= damage;
                }
             
            }
        }

        private void SetDamageNormal()
        {
            initDmg = myWeapon.attackDamage;
        }

        private void SetSpeedNormal()
        {
            pM.speed = initVit;
        }
    }
}