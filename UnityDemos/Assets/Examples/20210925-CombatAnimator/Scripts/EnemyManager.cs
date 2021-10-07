using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace example.y20210925
{
    public class EnemyManager : MonoBehaviour
    {
        private EnemyScript[] enemies;
        public EnemyStruct[] allEnemies;
        private List<int> enemyIndexes;

        [Header("Main AI Loop - Settings")]
        private Coroutine AI_Loop_Coroutine;

        public int aliveEnemyCount;

        private void Start()
        {
            enemies = GetComponentsInChildren<EnemyScript>();

            allEnemies = new EnemyStruct[enemies.Length];

            for (int i = 0; i < allEnemies.Length; i++)
            {
                allEnemies[i].enemyScript = enemies[i];
                allEnemies[i].enemyAvailability = true;
            }

            // 启动AI 协程
            StartAI();
        }

        public void StartAI()
        {
            AI_Loop_Coroutine = StartCoroutine(AI_Loop(null));
        }

        // AI 协程循环
        IEnumerator AI_Loop(EnemyScript enemy)
        {
            // 如果活动的敌人数量为0，停止AI协程
            if (AliveEnemyCount() == 0)
            {
                StopCoroutine(AI_Loop(null));
                yield break;
            }

            // 在游戏中，无论是动作，还是关卡增加随机，有助于游戏性的增加
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            // 随机选择一个除了enemy 以外的敌人
            EnemyScript attackingEnemy = RandomEnemyExcludingOne(enemy);

            if (attackingEnemy == null)
            {
                attackingEnemy = RandomEnemy();
            }

            if (attackingEnemy == null)
            {
                yield break;
            }

            yield return new WaitUntil(() => attackingEnemy.IsRetreating() == false);
            yield return new WaitUntil(() => attackingEnemy.IsLockedTarget() == false);
            yield return new WaitUntil(() => attackingEnemy.IsStunned() == false);

            attackingEnemy.SetAttack();

            yield return new WaitUntil(() => attackingEnemy.IsPreparingAttack() == false);

            attackingEnemy.SetRetreat();    // 撤退

            yield return new WaitForSeconds(Random.Range(0, 0.5f));

            if (AliveEnemyCount() > 0)
            {
                AI_Loop_Coroutine = StartCoroutine(AI_Loop(attackingEnemy));
            }    
        }

        public EnemyScript RandomEnemy()
        {
            enemyIndexes = new List<int>();

            for (int i =0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].enemyAvailability)
                {
                    enemyIndexes.Add(i);
                }
            }

            if (enemyIndexes.Count == 0)
            {
                return null;
            }

            EnemyScript randomEnemy;
            int randomIndex = Random.Range(0, enemyIndexes.Count);
            randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

            return randomEnemy;
        }

        public EnemyScript RandomEnemyExcludingOne(EnemyScript exclude)
        {
            enemyIndexes = new List<int>();

            for (int i=0; i< allEnemies.Length; i++)
            {
                if (allEnemies[i].enemyAvailability && allEnemies[i].enemyScript != exclude)
                {
                    enemyIndexes.Add(i);
                }
            }

            EnemyScript randomEnemy;
            int randomIndex = Random.Range(0, enemyIndexes.Count);
            randomEnemy = allEnemies[enemyIndexes[randomIndex]].enemyScript;

            return randomEnemy;
        }

        public int AvailableEnemyCount()
        {
            int count = 0;
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].enemyAvailability)
                {
                    count++;
                }
            }
            return count;
        }

        public bool AnEnemyIsPreparingAttack()
        {
            foreach (EnemyStruct enemyStruct in allEnemies)
            {
                if (enemyStruct.enemyScript.IsPreparingAttack())
                {
                    return true;
                }
            }
            return false;
        }

        public int AliveEnemyCount()
        {
            int count = 0;
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].enemyScript.isActiveAndEnabled)
                {
                    count++;
                }
            }
            aliveEnemyCount = count;
            return count;
        }

        public void SetEnemyAvailiablity(EnemyScript enemy, bool state)
        {
            for (int i = 0; i < allEnemies.Length; i++)
            {
                if (allEnemies[i].enemyScript == enemy)
                {
                    allEnemies[i].enemyAvailability = state;
                }
            }

            if (FindObjectOfType<EnemyDetection>().CurrentTarget() == enemy)
            {
                FindObjectOfType<EnemyDetection>().SetCurrentTarget(null);
            }
        }
    }


    [System.Serializable]
    public struct EnemyStruct
    {
        public EnemyScript enemyScript;
        public bool enemyAvailability;
    }
}