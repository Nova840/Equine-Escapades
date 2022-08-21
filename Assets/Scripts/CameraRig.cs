using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRig : MonoBehaviour {

    public Rigidbody Follow { get; set; }
    public float SpawnAngleY { get; set; }//Y angle of spawnpoint. used to set initial camera rotation

    [SerializeField]
    private float moveLerpRate = 10, rotateLerpRate = 10, sensitivityX = 60, sensitivityY = 120, initialAngleX = 30;

    private float angleX = 0, angleY = 0;

    [SerializeField]
    private float minX = 0, maxX = 90;

    public string Device { get; set; }

    [SerializeField]
    private LayerMask[] cullingMasks = new LayerMask[4];

    private void Start() {
        int playerNumber = PlayerInput.FindIndex(Device) + 1;
        Camera camera = GetComponentInChildren<Camera>();
        if (playerNumber == 1) {
            camera.gameObject.tag = "MainCamera";
            camera.gameObject.AddComponent<AudioListener>();
        }
        camera.cullingMask = cullingMasks[playerNumber - 1];
        transform.position = TargetPosition();
        angleY = SpawnAngleY + 180;
        angleX = initialAngleX;
        transform.localEulerAngles = new Vector3(angleX, angleY, 0);
    }

    private void Update() {
        transform.position = Vector3.Lerp(transform.position, TargetPosition(), moveLerpRate * Time.deltaTime);

        if (Device[0] != 'K' || !Pause.Paused) {
            float scaledInputX = Input.GetAxisRaw(InputMapper.Map(Device + "A5")) * sensitivityX;
            if (Device[0] != 'K')//mouse input is already frame rate independent
                scaledInputX *= Time.deltaTime;
            angleX += scaledInputX;
            angleX = Mathf.Clamp(angleX, minX, maxX);
            angleX = Angle360(angleX);

            float scaledInputY = Input.GetAxisRaw(InputMapper.Map(Device + "A4")) * sensitivityY;
            if (Device[0] != 'K')//mouse input is already frame rate independent
                scaledInputY *= Time.deltaTime;
            angleY += scaledInputY;
            angleY = Angle360(angleY);
        }

        transform.localEulerAngles = new Vector3(
            Mathf.LerpAngle(transform.localEulerAngles.x, angleX, rotateLerpRate * Time.deltaTime),
            Mathf.LerpAngle(transform.localEulerAngles.y, angleY, rotateLerpRate * Time.deltaTime),
            0
        );
    }

    private Vector3 TargetPosition() {
        return Follow.transform.TransformPoint(Follow.centerOfMass);
    }

    private float Angle360(float angle) {//keep angle between 0 and 360
        angle %= 360;
        if (angle < 0)
            angle += 360;
        return angle;
    }

}