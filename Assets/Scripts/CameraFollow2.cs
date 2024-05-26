using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2 : MonoBehaviour
{
    public float mouseX, mouseY;//
    public float mouseSensitivity;

    public Transform player;

    float xRotation;

    private bool cameraActive = true;
    //----------------------------------------------------------
    private float distance;
    //public Transform character;
    void Start()
    {
        //character = GameObject.Find("Player").transform;    //传入角色物体的引用
        distance = Vector3.Distance(transform.position, player.position);  //记录相机和角色的初始距离
    }
    void MoveCamera()
    {
        if (Physics.Raycast(player.position, //从角色物体的原点发出射线(一般情况下原点是角色模型的脚部,所以需要自己根据实际调整该点)
                            transform.position - player.position,    //射线方向
                            out RaycastHit hit,
                            distance)    //最大距离只需要到相机为止
            && hit.collider.CompareTag("Wall"))     //如果射线打到了collider,则判断其是否是墙体
        {
            Vector3 newPoint = hit.point;   //若是墙体,则将打到的点设为相机的新位置
            transform.position = Vector3.Lerp(transform.position, newPoint, 0.5f);
        }
        else
        {
            //如果射线没有打到任何collider,或者打到的不是墙体,则将相机和角色之间的距离还原
            Vector3 p = (transform.position - player.position).normalized * distance;
            Vector3 newPoint = player.position + p;
            transform.position = Vector3.Lerp(transform.position, newPoint, 0.5f);
        }
    }

    //------------------------------------------------------------




    public void Update()
    {
        if (cameraActive)
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -10f, 40f);


            player.Rotate(Vector3.up * mouseX);
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            MoveCamera();
        }
    }

    public void inactiveCamRotation(){
        cameraActive = false;
    }
}
