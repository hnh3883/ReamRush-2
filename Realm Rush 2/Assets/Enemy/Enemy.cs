using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    // 참조타입 : "Bank" 클래스 (해당 클래스에 접근하기 위함)
    Bank bank;
    void Start()
    {
        // 외부 메소드에 접근
        bank = FindObjectOfType<Bank>();
    }

    public void RewardGold()  // EnemyHealth 스크립트에서 사용 (적을 죽이면 보상)
    {
        // 메소드가 잘 돌아가기 위한 보호장치 코드
        if(bank == null){ return; }

        // Bank 클래스 안의 Deposit 메소드에 접근
        bank.Deposit(goldReward);
    }

    public void StealGold()  // EnemyMover 스크립트에서 사용 (적을 막지 못하면 벌금)
    {
        // 메소드가 잘 돌아가기 위한 보호장치 코드
        if (bank == null) { return; }

        // Bank 클래스 안의 withdraw 메소드에 접근
        bank.withdraw(goldPenalty);
    }
}
