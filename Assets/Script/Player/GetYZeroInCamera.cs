using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetYZeroInCamera : MonoBehaviour
{
    public  Camera mainCamera;
    public RaycastHit hit;
    // Start is called before the first frame update
    private Vector3 worldPosition;
    private Vector3 targetpostion;
    GameObject nearestEnemy = null;
    GameObject enemyObject = null;
    float distance = 0;
    // Update is called once per frame
    Transform tr;
    private void Start()
    {
        tr = GetComponent<Transform>();
    }
    void Update()
    {
        // 마우스 포인터의 스크린 좌표 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 스크린 좌표를 레이로 변환
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        RaycastHit[] hitInfos;

        hitInfos = Physics.SphereCastAll(ray, /*radius*/ 1.0f);
        // Ray와 충돌하는 오브젝트를 검색
        foreach (var hitInfo in hitInfos)
        {
            if (hitInfo.collider.CompareTag("Enemy"))
            {
                // 충돌한 오브젝트가 "Enemy" 태그를 가진 경우
                enemyObject = hitInfo.collider.gameObject;
                //float distance = Vector3.Distance(hitInfo.point, enemyObject.transform.position);

                // 주변에서 가장 가까운 "Enemy" 태그를 가진 게임 오브젝트를 찾았을 때의 동작 수행
                //Debug.Log("가장 처음에 부딪힌 Enemy를 찾았습니다: " + enemyObject.transform.position + "  " + hitInfo.point + "  " + distance);
                break;
            }
            else
            {
                enemyObject = null;
            }
        }
  
        
    

      

        // 카메라 시선과 마우스 레이 간의 교차점을 찾습니다.
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, tr.position.y, 0f)); // 지면을 나타내는 평면 (y=0)
        float rayDistance;
       
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // 교차 지점의 월드 좌표를 계산합니다.
        worldPosition = ray.GetPoint(rayDistance);
           

            // worldPosition은 시선과 마우스 위치를 연결하는 직선에서 y 좌표가 0인 지점의 월드 위치입니다.
        }
        


    }

    public Vector3 getMousePosition()
    {
      
        return worldPosition;
        
    }
    public Vector3 gettargetpostion()
    {
        if (enemyObject != null ) {
            return enemyObject.transform.position;
           
        }
        else
        {
            Debug.Log(worldPosition);
            return worldPosition + new Vector3(0,1.0f,0) ;
        }
    }

}
