using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

namespace Coliseum
{

    public class PlayerHealth : MonoBehaviour
    {
        private PhotonView photonView;
        /*[SerializeField]*/ public float health;
        private bool alive;
        
        // Shield
        public bool shield;
        public float shieldCooldown;
        public float shieldDuration;
        private float shieldTimer;
        private Animator anim;

        // Dmitry
        public TMPro.TMP_Text healthText;
        private float MinHealth = 0;
        private float MaxHealth = 100;

        // Bonus
        public string type;

        private void Start()
        {
            photonView = (PhotonView)GameManager.weapon.GetComponent<PhotonView>();
            anim = GetComponent<Animator>();
            shieldTimer = 99f;
        }

        private void Update()
        {
            shieldTimer += Time.deltaTime;
            
            if (photonView.IsMine)
            {
                if (Input.GetKeyDown(KeyCode.H) && shieldTimer > shieldCooldown)
                {
                    shield = true;
                    Invoke("SetBoolBack", shieldDuration);
                    shieldTimer = 0f;
                }
            }

            if (CompareTag("Player"))
            {
                anim.SetBool("shield", shield);
                this.healthText.text = health.ToString();
            } //Afficher les hp

            if (photonView.IsMine)
            {
                
                if (health > MaxHealth)
                {
                    health = MaxHealth;
                }

                if (health < MinHealth)
                {
                    health = MinHealth;
                    PhotonNetwork.Destroy(GameManager.weapon);
                    GameManager.RespawnPoint();
                    PhotonNetwork.Instantiate(GameManager.weapon.name, GameManager.FreePos, Quaternion.identity);

                }
            }
            
        }

        public void TakeDamage(float damage)
        {
            alive = health > 0;
            health -= damage;
            if (health <= 0)
            {
                print("Enemy has died");
                alive = false;
                transform.Translate(0,-20,0);
            }
        
            if (alive)
                print("Enemy has taken damage");
        }
        
        
        // Dmitry
        // Les HP
        public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)     // Mon personnage
            {
                stream.SendNext(health);
            }
            else if (stream.IsReading)       // Tous les autres
            {
                health = (float)stream.ReceiveNext();
            }
        }
        
        
        // New
        [PunRPC]
        public void Damage(float dmg)
        {
            Debug.Log ("Damaged");
            if (!shield)
            {
                health -= dmg;
            }
        }
        //

        private void SetBoolBack()
        {
            shield = false;
        }
    }
}
