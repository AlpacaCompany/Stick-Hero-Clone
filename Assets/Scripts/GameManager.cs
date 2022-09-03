using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public GameObject Gameover;
    public TextMeshProUGUI CounText;
    [HideInInspector]
    public static int Count;
    public CinemachineVirtualCamera cine;
    
    // Start is called before the first frame update
    void Start()
    {
        Count = 0; //카운트 초기화
        Time.timeScale = 1; //시간 초기화
    }

    // Update is called once per frame
    void Update()
    {
        CounText.text = Count.ToString(); //카운트 텍스트 업데이트
    }

    public void OnGameover() {
        Gameover.SetActive(true); //게임오버 UI켜기
    }

    public void GameOverBtn() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //재시작
        
    }
}
