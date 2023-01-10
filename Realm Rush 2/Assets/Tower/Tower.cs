using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;  // 타워 설치비용
    [SerializeField] float buildDelay = 1f; 

    void Start()
    {
        StartCoroutine(Build());
    }

    public bool CreateTower(Tower tower, Vector3 position)  // true 또는 false를 반환하는 bool 타입 (Waypoint 스크립트에서 사용)
    {
        // 참조타입 : "Bank" 클래스
        // Bank 클래스에 접근
        Bank bank = FindObjectOfType<Bank>();

        // bank가 빈 값이라면
        if (bank == null)
        {
            // false를 반환하라
            return false;
        }


        // 만약 bank클래스의 현재금액이 타워설치비용보다 많다면 
        if (bank.CurrentBalance >= cost)
        {
            // 인스턴스화 하라
            Instantiate(tower.gameObject, position, Quaternion.identity);
            bank.withdraw(cost); // 은행에서 cost만큼 차감
            return true; // true를 반환하라
        }

        return false;  // false를 반환하라
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
