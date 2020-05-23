using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Coliseum
{
    public class playerMove : MonoBehaviour
    {
        private PhotonView photonView;
        
        // Jump
        private float verticalVelocity;
        public float jumpForce = 5.0f;
        private float gravity = 14.0f;
        public Animator anim;
        private bool jumpAnim;
        private PlayerHealth pH;

        // Move
        public float speed = 15.0f;
        public Camera cam;
        
        // Nickaname au-dessus des tetes
        public TMPro.TMP_Text playerUsername;
        
        // Knockback
        public Vector3 knockback;

        private CharacterController player;

        // Start is called before the first frame update
        void Start()
        {
            photonView = GetComponent<PhotonView>();
            //if (!photonView.IsMine) 
            //    Destroy(camera);
            if (photonView.IsMine)
            {
                ThirdPersonCamera.lookAt = this.transform;
                // Camera.main.transform.position =
                //      this.transform.position - this.transform.forward * 10 + this.transform.up * 3;
                //  Camera.main.transform.LookAt(this.transform.position);
                //  Camera.main.transform.parent = this.transform;
            }
            
            player = GetComponent<CharacterController>();
            cam = Camera.main;
            anim = GetComponent<Animator>();
            pH = GetComponent<PlayerHealth>();
        }

        // Update is called once per frame
        void Update()
        {
            if (photonView.IsMine)
            {
                photonView.RPC("SyncProfile", RpcTarget.All, PhotonNetwork.NickName);
                
                jumpAnim = false;

                Vector3 forward = cam.transform.forward;
                Vector3 right = cam.transform.right;

                forward.y = 0;
                right.y = 0;

                forward *= speed;
                right *= speed;

                bool UnlockedRotation = false;

                Vector3 move = new Vector3();

                if (Input.GetKey(KeyCode.D))
                {
                    move = right;
                    UnlockedRotation = true;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    move = -right;
                    UnlockedRotation = true;
                }

                if (Input.GetKey(KeyCode.Z))
                {
                    if (Input.GetKey(KeyCode.Q))
                        forward = (forward - right) * 0.5f;
                    if (Input.GetKey(KeyCode.D))
                        forward = (forward + right) * 0.5f;
                    move = forward;
                    UnlockedRotation = true;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    if (Input.GetKey(KeyCode.Q))
                        forward = (forward + right) * 0.5f;
                    if (Input.GetKey(KeyCode.D))
                        forward = (forward - right) * 0.5f;
                    move = -forward;
                    UnlockedRotation = true;
                }

                move *= 2f;
                if (Input.GetKey(KeyCode.R))
                    move *= 2f;
                
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Vector3 roulade = new Vector3();
                    roulade = transform.forward * 250f;
                    roulade.y = 0;
                    move += roulade;
                }
                
                anim.SetBool("roulade",Input.GetKeyDown(KeyCode.F));
                
                if (player.isGrounded)
                {
                    verticalVelocity = -gravity * Time.deltaTime;
                    if (Input.GetKey(KeyCode.Space))
                    {
                        verticalVelocity = jumpForce;
                        jumpAnim = true;
                    }
                }
                else
                {
                    verticalVelocity -= gravity * Time.deltaTime;
                }

                Vector3 jump = Vector3.zero;
                jump.y = verticalVelocity * jumpForce;

                if (UnlockedRotation)
                {
                    anim.SetBool("isRunning", true);
                    transform.rotation = Quaternion.LookRotation(move);
                }
                else
                {
                    anim.SetBool("isRunning", false);
                    if (!jumpAnim)
                    {
                        FindObjectOfType<AudioManager>().Play("walk");
                    }
                }

                move += jump;
                //move += knockback;
                if (!pH.shield)
                {
                    player.Move(move * Time.deltaTime);  
                }
                if (jumpAnim)
                    anim.SetBool("isJumping", true);
                else
                    anim.SetBool("isJumping", false);
            }
        }

        [PunRPC]  
        private void SyncProfile(string name) // Afficher les nicknames
        {
            playerUsername.text = name;
        }

        [PunRPC]
        public void Knockback(Vector3 kb)
        {
            player.Move(kb);
        }
    }
}