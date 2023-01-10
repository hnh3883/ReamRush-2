using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;  // Ÿ�� ��ġ���
    [SerializeField] float buildDelay = 1f; 

    void Start()
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)  // true �Ǵ� false�� ��ȯ�ϴ� bool Ÿ�� (Waypoint ��ũ��Ʈ���� ���)
    {
        // ����Ÿ�� : "Bank" Ŭ����
        // Bank Ŭ������ ����
        Bank bank = FindObjectOfType<Bank>();

        // bank�� �� ���̶��
        if (bank == null)
        {
            // false�� ��ȯ�϶�
            return false;
        }


        // ���� bankŬ������ ����ݾ��� Ÿ����ġ��뺸�� ���ٸ� 
        if (bank.CurrentBalance >= cost)
        {
            // �ν��Ͻ�ȭ �϶�
            Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.withdraw(cost); // ���࿡�� cost��ŭ ����
            return true; // true�� ��ȯ�϶�
        }

        return false;  // false�� ��ȯ�϶�
    }

    IEnumerator Build()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
            foreach(Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(false);
            }
        }

        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
            yield return new WaitForSeconds(buildDelay);
            foreach(Transform grandchild in child)
            {
                grandchild.gameObject.SetActive(true);
            }
        }
    }

}
