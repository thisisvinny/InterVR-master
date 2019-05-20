using UnityEngine;
using System.Collections;

public class FirstPersonCam : MonoBehaviour {
    //https://gamedev.stackexchange.com/questions/104693/how-to-use-input-getaxismouse-x-y-to-rotate-the-camera
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 260.0f;
    private float pitch = 1.0f;

    void Update () {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }
}