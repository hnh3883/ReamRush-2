using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 150;  // 시작 금액 (인스펙터에서 1000원으로 수정함)
    [SerializeField] int currentBalance;  // 현재 금액

    // get 메소드는 변수만 반환
    // 읽기 전용 메소드가 되어버림  (쓰기전용은 set)
    public int CurrentBalance { get { return currentBalance;  } }

    // Text 타입으로 변수 선언
    [SerializeField] TextMeshProUGUI displayBalance;

     
    void Awake()
    {
        // 시작할 때 현재금액 = 시작금액
        currentBalance = startingBalance;

        // 시작할 때 금액 표시를 업데이트
        UpdateDisplay();
    }

    // 인자값만 있고, 보상과 벌금이 얼만지 정해지지 않음 (Bank 스크립트에서 정함)
    public void Deposit(int amount) // (Enemy 스크립트의 RewardGold 메소드에서 활용)
    {
        //Mathf.Abs은 절댓값을 반환하는 함수
        // 인자값이 음수여도 절댓값으로 계산할 수 있도록 함
        currentBalance += Mathf.Abs(amount);

        // 금액 표시 업데이트
        UpdateDisplay();
    }

    public void withdraw(int amount)  // (Enemy 스크립트의 StealGold 메소드에서 활용)
    {
        //Mathf.Abs은 절댓값을 반환하는 함수
        currentBalance -= Mathf.Abs(amount);

        // 금액 표시 업데이트
        UpdateDisplay();

        // 0원이하로 떨어지면
        if (currentBalance < 0) 
        {
            // 게임을 다시 재로드한다
            ReloadScene();
        }
    }
    void UpdateDisplay() // 금액 표시를 계속하여 업데이트하는 메소드
    {
        // text 타입의 변수에 ["Gold" + 현재금액] 을 넣어줌
        displayBalance.text = "Gold: " + currentBalance;
    }

    void ReloadScene() // 씬을 리로드 하는 메소드
    {
        // 현재 씬 번호는 씬매니저에서 가져와서 대입
        Scene currentScene = SceneManager.GetActiveScene();

        // 현재 씬 번호로 다시 씬을 리로드
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}
