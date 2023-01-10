using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    
    [SerializeField] int maxHitPoints = 5; // ������ full ü��

    [Tooltip("���� ������ maxHitPoints�� �߰��϶�")]
    [SerializeField] int difficultyRamp = 1;  // ���� ���� ������
    int currentHitPoints = 0;   // ������ ���� ü��

    // ����Ÿ�� : "Enemy" Ŭ���� (�ش� Ŭ������ �����ϱ� ����)
    Enemy enemy;

    // OnEnable�� ���̾��Ű���� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ �� �� ȣ��
    // �Ʒ��� ProcessHit �޼ҵ忡�� ���� ������ ��Ȱ��ȭ�� �ǵ��� �ڵ� �ۼ���
    void OnEnable()
    {
        // ������ ���� ü���� �ٽ� full ü������ ��������.
        currentHitPoints = maxHitPoints;
    }

    void Start()
    {
        // �ܺ� �޼ҵ忡 ����
        enemy = GetComponent<Enemy>();
    }

    
    void OnParticleCollision() // ��ƼŬ�� �¾����� �ߵ�
    {
        // ��ƼŬ(ȭ��)�� ���� �¾Ҵٸ� �޼ҵ� ����
        ProcessHit();  
    }

    void ProcessHit()
    {
        // ������ ���� ü���� 1�� ���ҽ�Ŵ
        currentHitPoints--;

        // ���� ������ ü���� 0���� �۾����ٸ�
        if(currentHitPoints <= 0)
        {
            gameObject.SetActive(false);  // �� ��ũ��Ʈ�� ������ ������Ʈ�� ��Ȱ��ȭ ��Ŵ 
            maxHitPoints += difficultyRamp;  // ������ full ü���� ��������ŭ ������Ŵ (�ð��� �带���� ���̵� ��� ����)
            enemy.RewardGold(); // Enemy ��ũ��Ʈ�� RewardGold �޼ҵ� ����
        }
    }
}
