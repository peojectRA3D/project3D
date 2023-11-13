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
    // Update is called once per frame
    void Update()
    {
        // 마우스 포인터의 스크린 좌표 가져오기
        Vector3 mousePosition = Input.mousePosition;

        // 스크린 좌표를 레이로 변환
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // 카메라 시선과 마우스 레이 간의 교차점을 찾습니다.
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, gameObject.transform.position.y, 0f)); // 지면을 나타내는 평면 (y=0)
        float rayDistance;
       
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // 교차 지점의 월드 좌표를 계산합니다.
        worldPosition = ray.GetPoint(rayDistance);
           

            // worldPosition은 시선과 마우스 위치를 연결하는 직선에서 y 좌표가 0인 지점의 월드 위치입니다.
        }
        if (Physics.Raycast(ray, out hit))
        {
            string hitTag = hit.collider.tag;
            
            if (hitTag == "Enemy")
            {
                // 교차 지점의 월드 좌표를 계산합니다.
                targetpostion = hit.point;

                //Debug.Log("Hit Position: " + worldPosition);
            }
            else
            {
                targetpostion = worldPosition;
            }
        }


    }

    public Vector3 getMousePosition()
    {
      
        return worldPosition;
        
    }
    public Vector3 gettargetpostion()
    {
        
        return targetpostion;

    }
}
