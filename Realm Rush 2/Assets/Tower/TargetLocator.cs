using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon; // Ÿ���� ���κ� (�ν����Ϳ��� ��� �κи� ���� ��������)
    [SerializeField] ParticleSystem projectileParticles;  // ��ƼŬ �ý������� ���� (ȭ��)
    [SerializeField] float range = 15 ;  //ȭ���� ��Ÿ�
    Transform target; // ��

    void Update()
    {
        FindClosestTarget(); // ���� ����� ���� ã�� �޼ҵ�
        AimWeapon();    // Ÿ�� ������ �����ϰ�, ȭ����� �߻��ϴ� �޼ҵ�
    }

    // ���� 124
    void FindClosestTarget() // ���� ����� Ÿ���� ã�� �޼ҵ�
    {
        // ����Ÿ�� : Enemy Ŭ����
        // Enemy Ŭ������ ����
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        // ���� ����� Ÿ�� (�ϴ� �����)
        Transform closestTarget = null;

        // ���Ƿ� ���Ѵ��� ū ������ ���� 
        float maxDistance = Mathf.Infinity;


        // enemies �迭���� ó������ ������ �ݺ�
        foreach(Enemy enemy in enemies)
        {
            // targetDistance = �ڽ��� ��ġ�� ���� ��ġ�� ����
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            // ���� Ÿ�ٰŸ��� maxDistance���� �۴ٸ�
            if (targetDistance < maxDistance)
            {
                // closestTarget�� enemies �迭�� enemy ��ġ���� �����Ѵ�.
                closestTarget = enemy.transform;
                // maxDistance�� Ÿ�ٰ��� �Ÿ��� ����
                maxDistance = targetDistance;

                // �� ����� ���� ��Ÿ�������� maxDistance�� �� �۾�����
                // ����ؼ� �ݺ��ϸ� ���� ����� ���� �Ǵܰ���
            }
        }

        // Ÿ���� ���� ����� Ÿ������ ���Ѵ�
        target = closestTarget;
    }

    void AimWeapon() // Ÿ���� ������ �Ǵ��ϴ� �޼ҵ�
    {
        // Ÿ�ٰ��� �Ÿ� = �ڽ��� ��ġ�� Ÿ���� ��ġ�� ����
        float targetDistance = Vector3.Distance(transform.position, target.position);
        
        //Ÿ���� ���κ��� Ÿ���� ���Ѵ� (���� �޼ҵ忡�� target = closetTarget���� ����)
        weapon.LookAt(target);

        // Ÿ�ٰ��� �Ÿ��� �����Ÿ����� �۴ٸ�
        if(targetDistance < range)
        {
            Attack(true); // ������ �ض�
        }

        // �׷��� �ʴٸ�
        else
        {
            Attack(false); // ������ ��������
        }
    }

    void Attack(bool isActive) // ȭ��(��ƼŬ)�� ������ �޼ҵ�
    {
        // ������ٹ� (��ƼŬ ��⿡ ����)
        var emissionModule = projectileParticles.emission;

        // ���ڰ��� true�� ��ƼŬ�� ����, false�� �������� ����
        emissionModule.enabled = isActive;
    }
}
