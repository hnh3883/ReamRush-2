using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;  // ���� �ݾ� (�ν����Ϳ��� 1000������ ������)
    [SerializeField] int currentBalance;  // ���� �ݾ�

    // get �޼ҵ�� ������ ��ȯ
    // �б� ���� �޼ҵ尡 �Ǿ����  (���������� set)
    public int CurrentBalance { get { return currentBalance;  } }

    // Text Ÿ������ ���� ����
    [SerializeField] TextMeshProUGUI displayBalance;

     
    void Awake()
    {
        // ������ �� ����ݾ� = ���۱ݾ�
        currentBalance = startingBalance;

        // ������ �� �ݾ� ǥ�ø� ������Ʈ
        UpdateDisplay();
    }

    // ���ڰ��� �ְ�, ����� ������ ���� �������� ���� (Bank ��ũ��Ʈ���� ����)
    public void Deposit(int amount) // (Enemy ��ũ��Ʈ�� RewardGold �޼ҵ忡�� Ȱ��)
    {
        //Mathf.Abs�� ������ ��ȯ�ϴ� �Լ�
        // ���ڰ��� �������� �������� ����� �� �ֵ��� ��
        currentBalance += Mathf.Abs(amount);

        // �ݾ� ǥ�� ������Ʈ
        UpdateDisplay();
    }

    public void withdraw(int amount)  // (Enemy ��ũ��Ʈ�� StealGold �޼ҵ忡�� Ȱ��)
    {
        //Mathf.Abs�� ������ ��ȯ�ϴ� �Լ�
        currentBalance -= Mathf.Abs(amount);

        // �ݾ� ǥ�� ������Ʈ
        UpdateDisplay();

        // 0�����Ϸ� ��������
        if (currentBalance < 0) 
        {
            // ������ �ٽ� ��ε��Ѵ�
            ReloadScene();
        }
    }
    void UpdateDisplay() // �ݾ� ǥ�ø� ����Ͽ� ������Ʈ�ϴ� �޼ҵ�
    {
        // text Ÿ���� ������ ["Gold" + ����ݾ�] �� �־���
        displayBalance.text = "Gold: " + currentBalance;
    }

    void ReloadScene() // ���� ���ε� �ϴ� �޼ҵ�
    {
        // ���� �� ��ȣ�� ���Ŵ������� �����ͼ� ����
        Scene currentScene = SceneManager.GetActiveScene();

        // ���� �� ��ȣ�� �ٽ� ���� ���ε�
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
