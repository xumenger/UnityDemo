using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestQuaternion : MonoBehaviour
{
    float rotationSpeed = 45;
    Vector3 currentEulerAngles;
    Quaternion currentRotation;
    float x;
    float y;
    float z;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) x = 1 - x;
        if (Input.GetKeyDown(KeyCode.Y)) y = 1 - y;
        if (Input.GetKeyDown(KeyCode.Z)) z = 1 - z;

        //modifying the Vector3, based on input multiplied by speed and time
        currentEulerAngles += new Vector3(x, y, z) * Time.deltaTime * rotationSpeed;

        //moving the value of the Vector3 into Quanternion.eulerAngle format
        currentRotation.eulerAngles = currentEulerAngles;

        //apply the Quaternion.eulerAngles change to the gameObject
        transform.rotation = currentRotation;
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 24;
        // Use eulerAngles to show the euler angles of the quaternion stored in Transform.Rotation
        GUI.Label(new Rect(10, 0, 0, 0), "Rotating on X:" + x + " Y:" + y + " Z:" + z, style);

        //outputs the Quanternion.eulerAngles value
        GUI.Label(new Rect(10, 25, 0, 0), "CurrentEulerAngles: " + currentEulerAngles, style);

        //outputs the transform.eulerAngles of the GameObject
        GUI.Label(new Rect(10, 50, 0, 0), "GameObject World Euler Angles: " + transform.eulerAngles, style);
    }
}
