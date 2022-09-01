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

    // Start is called before the first frame update
    void Start()
    {
       rigid = GetComponent<Rigidbody2D>(); //rigid를 이 스크립트가 있는 오브젝트에게서 가져온다.
    }

    // Update is called once per frame
    void Update()
    {
        MouseStay(0); //마우스를 누르고있을때 발생하는 이벤트

        MouseUp(0, 10f); //마우스를 뗄때 발생하는 이벤트

        Ray("Land", Pos); // 레이를 쏘고, 맞았을때의 이벤트
    }

    public void MouseStay(int i) {
        if(Input.GetMouseButton(i)) { //마우스 i버튼을 누르고 있을때 (누르는 중일때)
            transform.localScale +=  offset * Time.deltaTime; //오브젝트의 크기를 원하는 속도만큼 늘리고
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
            if(hit.collider.tag == Tag) { //만약 레이가 닿은 오브젝트의 태그가 Tag라면 , 성공했을때 다음발판으로 넘어가는 처리.
                Player.transform.position = new Vector3(hit.collider.gameObject.transform.position.x ,0,0); //플레이어의 위치를 레이에 맞은 오브젝트의 x값 위로 옮김.
            } else { //실패했을때. 게임오버 처리

            }
        }
    }
}
