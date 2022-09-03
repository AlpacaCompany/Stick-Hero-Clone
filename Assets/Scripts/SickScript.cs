using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SickScript : MonoBehaviour
{
    public Vector3 offset; //오프셋, 사실상 스틱을 늘리는 속도.
    public GameObject Player; //플레이어 오브젝트
    Rigidbody2D rigid; //스틱의 물리 컴포넌트
    RaycastHit2D hit; //레이를 쓰기 위한 변수
    public Transform Pos; //레이를 쏘기위한 위치, 각도를 위한 오브젝트
    public GameObject StickPrefab; //막대기 프리팹
    public GameObject LandPrefab; //지형 프리팹

    GameObject Lands; //땅을 묶어놓을 부모 오브젝트
    GameManager mana; //게임매니져 가져오기

    // Start is called before the first frame update
    void Start()
    {
       rigid = GetComponent<Rigidbody2D>(); //rigid를 이 스크립트가 있는 오브젝트에게서 가져온다.
       Lands = GameObject.Find("Lands"); //부모 Lands가져오기
       mana = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>(); //mana 가져오기
    }

    // Update is called once per frame
    void Update()
    {
        MouseStay(0); //마우스를 누르고있을때 발생하는 이벤트

        MouseUp(0, 10f); //마우스를 뗄때 발생하는 이벤트

        Ray("Land", Pos); // 레이를 쏘고, 맞았을때의 이벤트

        OnFalling(-10); //떨어졌을때 이벤트
    }

    public void MouseStay(int i) {
        if(Input.GetMouseButton(i)) { //마우스 i버튼을 누르고 있을때 (누르는 중일때)
            transform.localScale +=  offset * Time.deltaTime; //오브젝트의 크기를 원하는 속도만큼 늘리고
            transform.rotation = Quaternion.Euler(0,0,0); //회전 안되게 고정
            transform.position = new Vector3(transform.position.x, (transform.localScale.y - 2) * 0.5f, transform.position.z); //오브젝트의 위치를 땅에 맞춰 알맞게 변환
        }
    }

    public void MouseUp(int i, float speed) {
        if(Input.GetMouseButtonUp(i)) { //마우스 i버튼을 올릴때 (뗐을때)
            transform.Rotate(Vector3.back * speed); //z값으로 speed 만큼 회전 -> 막대 오브젝트가 기울어져서 쓰러짐 (도미노처럼)
        }
    }

    public void Ray(string Tag , Transform Posi) {
        if(rigid.velocity == Vector2.zero) { //물리.가속도값이 0이라면 -> 움직이지 않는 경우
            hit = Physics2D.Raycast(Posi.position + new Vector3(0.1f , 0,0) , Posi.right, 10); //레이를 Pos의 오른쪽 방향으로 쏨
            if(Physics2D.Raycast(Posi.position + new Vector3(0.1f , 0,0) , Posi.right, 10)) { //레이가 맞았을때
                if(hit.collider.CompareTag(Tag)) { //만약 레이가 닿은 오브젝트의 태그가 Tag라면 , 성공했을때 다음발판으로 넘어가는 처리.
                Player.transform.position = new Vector3(hit.collider.gameObject.transform.position.x ,0,0); //플레이어의 위치를 레이에 맞은 오브젝트의 x값 위로 옮김.
                GameObject Stick = Instantiate(StickPrefab , transform.position, Quaternion.identity); //새 스틱 생성
                Stick.transform.localScale = new Vector3(0.2f,2,1); //스틱의 크기 조절.
                Stick.GetComponent<SickScript>().Player = Player; //스틱 프리팹의 플레이어를 현재의 플레이어로 설정
                Stick.transform.position = Player.transform.position + new Vector3(1,0.1f,0); //스틱 위치를 플레이어 앞으로 이동
                GameObject Land = Instantiate(LandPrefab); //땅 복제 , Land 변수에 저장
                Land.transform.parent = Lands.transform; //땅의 부모를 Lands로 변경.
                Land.transform.localScale = new Vector3(Random.Range(2.2f,4.5f), 4,1); //땅의 x를 1~4중 랜덤, 기본값
                Land.transform.position = new Vector3(Player.transform.position.x, -3,0) + new Vector3(Random.Range(5,8), 0,0);   //땅의 위치(x)를 무작위로 변경            
                Destroy(this.gameObject); //전 다리를 건넌 막대기 삭제
                GameManager.Count++; //카운트 1추가
                this.enabled = false; //다음 스틱을 만들었을때, 두 스틱 모두 동작하는걸 막기 위한 스크립트 비활성화처리.
            }
            } else { //레이가 맞지 않았을때
                if(this.gameObject.transform.rotation.eulerAngles.z < 271 && this.gameObject.transform.rotation.eulerAngles.z > 269) { //땅보다 길게 늘렸을때
                    mana.OnGameover(); //게임오버
                }
            }
        }
    }

    public void OnFalling(int speed) { //떨어지고 있을때
        if(rigid.velocity.y <= speed) { //떨어지는 속도가 10이상이면 (떨어졌다면)
            mana.OnGameover(); //게임오버
            Time.timeScale = 0; //계속 떨어지면 이 if문이 계속 실행되기에 시간을 0으로
        }
    }
}
