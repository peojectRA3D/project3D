using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetYZeroInCamera : MonoBehaviour
{
    public  Camera mainCamera;
   
    // Start is called before the first frame update
    private Vector3 worldPosition;
    // Update is called once per frame
    void Update()
    {
        // ���콺 �������� ��ũ�� ��ǥ ��������
        Vector3 mousePosition = Input.mousePosition;

        // ��ũ�� ��ǥ�� ���̷� ��ȯ
        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        // ī�޶� �ü��� ���콺 ���� ���� �������� ã���ϴ�.
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0f, gameObject.transform.position.y, 0f)); // ������ ��Ÿ���� ��� (y=0)
        float rayDistance;
        Debug.Log(groundPlane);
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            // ���� ������ ���� ��ǥ�� ����մϴ�.
        worldPosition = ray.GetPoint(rayDistance);
           

            // worldPosition�� �ü��� ���콺 ��ġ�� �����ϴ� �������� y ��ǥ�� 0�� ������ ���� ��ġ�Դϴ�.
        }

     

    }

    public Vector3 getMousePosition()
    {
      
        return worldPosition;
        
    }
}
