using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // ������ �����յ��� ����� ���ӿ�����Ʈ Ÿ�� ���� ����
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;  // Ǯ�� ũ�� ���� (������ ������Ʈ ��)    ���� : 0~50
    [SerializeField] [Range(0.1f,30f)] float spawnTimer = 1f;  // Ǯ�� ���ݼ��� (1�ʿ� �ϳ��� Ǯ��)  ���� :0.1~ 30

    // �迭 ����
    GameObject[] pool;

    void Awake()
    {
        // start �������� ���� ����
        PopulatePool();  // Ǯ���ϴ� �޼ҵ� (ó�� ������ �� �ν��Ͻ�ȭ�� ����)
    }

    void Start()
    {
        // ��Ȱ��ȭ�� ���̾��Ű���� 1�ʰ������� Ȱ��ȭ ��Ű�� �ڷ�ƾ �Լ�
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool() // Ǯ���ϴ� �޼ҵ�
    {
        // Ǯ�� ũ��� poolSize�� ����
        pool = new GameObject[poolSize];

        // Ǯ �迭�� ���̸�ŭ �ݺ�
        for (int i = 0; i < pool.Length; i++)
        {
            // i��° pool�� �ν��Ͻ�ȭ �ض�
            pool[i] = Instantiate(enemyPrefab, transform);

            // �ν��Ͻ��� ���ѳ��� ��Ȱ��ȭ ���·� ��
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        // Ǯ �迭�� ���̸�ŭ �ݺ�
        for(int i = 0; i < pool.Length; i++)
        {
            // ���� i��° pool�� ��Ȱ��ȭ �Ǿ��ִٸ�
            if(pool[i].activeInHierarchy == false)
            {
                // i��° pool�� Ȱ��ȭ ���Ѷ�
                // ������ �����ϸ� EnemyMover ��ũ��Ʈ���� �ٽ� ��Ȱ��ȭ ��Ŵ
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        // �ڷ�ƾ�� �ݺ��ϱ� ����
        while (true)
        {
            // ��Ȱ��ȭ�� ���̾��Ű���� 1�ʰ������� Ȱ��ȭ
            EnableObjectInPool();

            // yield return�� ��� �������� �Ѱ��ִ� ��
            // WaitForSeconds()�� ���ڰ���ŭ �ð��� ������ �ٽ� �ڷ�ƾ�� �����ϴ� �ڵ� 
            yield return new WaitForSeconds(spawnTimer);
        }
    }

}
