using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 25;

    // ����Ÿ�� : "Bank" Ŭ���� (�ش� Ŭ������ �����ϱ� ����)
    Bank bank;
    void Start()
    {
        // �ܺ� �޼ҵ忡 ����
        bank = FindObjectOfType<Bank>();
    }

    public void RewardGold()  // EnemyHealth ��ũ��Ʈ���� ��� (���� ���̸� ����)
    {
        // �޼ҵ尡 �� ���ư��� ���� ��ȣ��ġ �ڵ�
        if(bank == null){ return; }

        // Bank Ŭ���� ���� Deposit �޼ҵ忡 ����
        bank.Deposit(goldReward);
    }

    public void StealGold()  // EnemyMover ��ũ��Ʈ���� ��� (���� ���� ���ϸ� ����)
    {
        // �޼ҵ尡 �� ���ư��� ���� ��ȣ��ġ �ڵ�
        if (bank == null) { return; }

        // Bank Ŭ���� ���� withdraw �޼ҵ忡 ����
        bank.withdraw(goldPenalty);
    }
}
