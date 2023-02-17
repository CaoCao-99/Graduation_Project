using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
public class Flash_Move : MonoBehaviour
{
    //외부 컴포넌트 정보
    Light flash_light;
    Transform tr;
    public Image BloodScreen;

    Vector3[] pos_arr = { new Vector3 { x = -3, y = 0, z = 3 }, new Vector3 { x = 3, y = 0, z = 3}, new Vector3 { x = -3, y = 0, z = 3 }, new Vector3 { x = 3, y = 0, z = 3 } };

    //내부 설정 변수
    public bool start = false;
    bool pos_1 = true;
    bool pos_2 = false;
    bool pos_3 = false;
    bool pos_4 = false;
    bool end = false;
    bool first = true;
    float currTime;
    static float ori_x, ori_y, ori_z, r_x, r_y, r_z;
    //public float speed = 10.0f;          //빛이 움직이는 속도
    public float duration = 3.0f;        //빛이 지속되는 시간
    public float time_accuracy = 0.1f;  //감지 시간
    public float speed = 0.001f;
    // Start is called before the first frame update
    void Awake()
    {
        flash_light = GetComponent<Light>();
        flash_light.enabled = false; //처음엔 꺼져있게 설정
        //GetComponent<Collider>().enabled = false;
        tr = this.transform;
        ori_x = tr.position.x;
        ori_y = tr.position.y;
        ori_z = tr.position.z;
        r_x = tr.rotation.x;
        r_y = tr.rotation.y;
        r_z = tr.rotation.z;
        
    }

    void Update()
    {
        currTime += Time.deltaTime; // 시간 증가
        if (start)
        {
            flash_light.enabled = true; //처음엔 꺼져있게 설정
            if (pos_1)
            {
                
                tr.position = Vector3.MoveTowards(tr.position, tr.position + pos_arr[0], speed * 0.1f);
                if (tr.position.z >= pos_arr[0].z)
                {
                    currTime = 0;
                    pos_1 = false;
                    pos_2 = true;
                }
            }
            else if (pos_2)
            {
                if (currTime >= 1)  // 1초가 넘었을 경우 실행(즉, 1초 정지 후 진행)
                {
                    tr.position = Vector3.MoveTowards(tr.position, tr.position + pos_arr[1], speed * 0.3f);
                    if (tr.position.z >= pos_arr[0].z + pos_arr[1].z)
                    {
                        currTime = 0;
                        pos_2 = false;
                        pos_3 = true;
                    }

                }
            }
            else if (pos_3)
            {
                if (currTime >= 0.5)  // 0.5초가 넘었을 경우 실행(즉, 1초 정지 후 진행)
                {
                    tr.position = Vector3.MoveTowards(tr.position, tr.position + pos_arr[2], speed * 0.5f);
                    if (tr.position.z >= pos_arr[0].z + pos_arr[1].z + pos_arr[2].z)
                    {
                        currTime = 0;
                        pos_3 = false;
                        pos_4 = true;
                    }
                }
            }
            else if (pos_4)
            {
                if (currTime >= 0.8)  // 0.8초가 넘었을 경우 실행(즉, 1초 정지 후 진행)
                {

                    tr.position = Vector3.MoveTowards(tr.position, tr.position + pos_arr[3], speed * 0.2f);
                }
            }
            if (tr.position.z >= 12)
            {
                pos_4 = false;
                flash_light.enabled = false;
                end = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Red_In");
            first = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && first && !end)
        {
            Debug.Log("Red_Out");
            StartCoroutine(ShowBLoodScreen());
        }
    }

    IEnumerator ShowBLoodScreen()
    {
        BloodScreen.color = new Color(1, 0, 0, UnityEngine.Random.Range(0.3f, 0.5f));
        yield return new WaitForSeconds(0.2f);
        BloodScreen.color = Color.clear;
    }
}
