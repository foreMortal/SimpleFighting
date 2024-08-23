using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{ 
    [SerializeField] private EnemyManager[] enemy;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] spawnPoints;

    private List<EnemyManager> activeEnemies = new List<EnemyManager>();

    private List<EnemyManager> hidedEnemiesFirst = new List<EnemyManager>();
    private List<EnemyManager> hidedEnemiesSecond = new List<EnemyManager>();
    private List<EnemyManager> hidedEnemiesThird = new List<EnemyManager>();
    private List<EnemyManager> hidedEnemiesFourth = new List<EnemyManager>();

    private List<int> enemiesCount = new List<int>() { 0, 0, 0, 0};
    private Health playersHealth; 
    private int enemyTypes;
    private float timer, addNewEnemy;

    private void Awake()
    {
        playersHealth = player.GetComponent<Health>();
    }

    private void Update()
    {
        if (Time.time >= timer)
        {
            Spawn();
            timer = Time.time + 5f;
        }
        if(Time.time >= addNewEnemy && enemyTypes < 3)
        {//  �������� ����� ��� ���������� ������ 10 ������
            enemyTypes++;
            addNewEnemy = Time.time + 10f;
        }
    }

    public void Spawn()
    {
        int enemyType = PickEnemyType();// ��� ������ ���

        switch (enemyType)
        {
            case 0:
                SpawnEnemy(hidedEnemiesFirst, 0);
                break;
            case 1:
                SpawnEnemy(hidedEnemiesSecond, 1);
                break;
            case 2:
                SpawnEnemy(hidedEnemiesThird, 2);
                break;
            case 3:
                SpawnEnemy(hidedEnemiesFourth, 3);
                break;
        }
    }

    private void SpawnEnemy(List<EnemyManager> list, int type)
    {
        if(list.Count > 0)
        {// ���� ���� ��������� � ���� ���������� ������ ���� 
            list[0].RefreshEnemy(spawnPoints[Random.Range(0, spawnPoints.Length)].position);
            activeEnemies.Add(list[0]);
            list.RemoveAt(0);
        }
        else// ���� ��� �� ������� ������
        {
            activeEnemies.Add(Instantiate(enemy[type], spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity));
            activeEnemies[^1].Setup(player, type, this);
        }
        enemiesCount[type]++;// ��������� ���������� ����������� ����� ����
    }

    public void EnemyDied(EnemyManager manager, int type)
    {
        enemiesCount[type]--;
        activeEnemies.Remove(manager);

        switch (type)
        {
            case 0:
                hidedEnemiesFirst.Add(manager); break;
            case 1:
                hidedEnemiesSecond.Add(manager); break;
            case 2:
                hidedEnemiesThird.Add(manager); break;
            case 3:
                hidedEnemiesFourth.Add(manager); break;
        }

        playersHealth.GetHited(-2f);// ���������� ������ 2 ������� ��������
    }

    private int PickEnemyType()
    {
        int enemyType = 100;//������� ����� ������ ��� ����������
        for (int i = 0; i < 4; i++)
        {
            if (enemiesCount[i] < enemyType && i <= enemyTypes)//������ �� ��� ����� ������� �������� �� ����� ������
                enemyType = enemiesCount[i];
        }
        return enemiesCount.IndexOf(enemyType);
    }
}