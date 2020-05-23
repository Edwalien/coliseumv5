using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Coliseum
{

    public class Weapon : MonoBehaviour
    {
        public float attackDamage;
        public float attackRange;
        public float cooldown;
        public string weapon;
        public float knockback;

        // Start is called before the first frame update
        void Start()
        {
            AssignStat();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void AssignStat()
        {
            switch (weapon)
            {
                case "marteau":
                    attackDamage = 16;
                    attackRange = 3;
                    cooldown = 1.2f;
                    knockback = 12;
                    break;

                case "lance":
                    attackDamage = 6;
                    attackRange = 6;
                    cooldown = 1f;
                    knockback = 3;
                    break;

                case "lame":
                    attackDamage = 3;
                    attackRange = 1;
                    cooldown = 0.5f;
                    knockback = 2;
                    break;

                case "claymore":
                    attackDamage = 10;
                    attackRange = 4;
                    cooldown = 1;
                    knockback = 5;
                    break;

                case "":
                    attackDamage = 1;
                    attackRange = 1;
                    cooldown = 1;
                    knockback = 3;
                    break;
            }
        }
    }
}