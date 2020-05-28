using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	[DllImport("__Internal")]
    private static extern void Hello();

	// Use this for initialization
	float[] parseData(string dataString) {
		string[] values = dataString.Split(',');
		float[] data = new float[12];
		for (int i = 0; i < 12; i++) {
			data[i] = float.Parse(values[i]);
		}
		return data;
	}

	void Calibrate (string dataString) {
		Camera cam = (gameObject.GetComponent(typeof(Camera)) as Camera);
		Matrix4x4 matrix = new Matrix4x4();
		float pi = (float)System.Math.PI;
		float[] data = parseData(dataString);
		// rotation is the rotation matrix of the camera in relation to itself ((0, 0), (2, 2) square of modelview matrix)
		float[,] rotation = new float[3, 3];
		float[] translation = new float[3];
		for (int i = 0; i < 3; i++) {
			for (int j = 0; j < 3; j++) {
				rotation[i, j] = data[3 * i + j];
			}
		}
		for (int i = 0; i < 3; i++) {
			translation[i] = data[9 + i];
		}
		// matrix is the projection matrix of the camera
		matrix.SetColumn(0, new Vector4(2.6454312801361084f, 0.0f, 0.0f, 0.0f));
		matrix.SetColumn(1, new Vector4(0.0f, 3.5272417068481445f, 0.0f, 0.0f));
		matrix.SetColumn(2, new Vector4(-0.034412480890750885f, -0.049199994653463364f, -1.000166654586792f, -1.0f));
		matrix.SetColumn(3, new Vector4(0.0f, 0.0f, -2.000166654586792f, 0.0f));
		cam.projectionMatrix = matrix;

		float angleX = (float)System.Math.Atan2(rotation[2, 1], rotation[2, 2]);
		float angleY = (float)System.Math.Atan2(-rotation[2, 0], System.Math.Sqrt(rotation[2, 1] * rotation[2, 1] + rotation[2, 2] * rotation[2, 2]));
		float angleZ = (float)System.Math.Atan2(rotation[1, 0], rotation[0, 0]);
		// roll, yaw, pitch
		transform.Rotate(new Vector3(0f, 0f, angleZ / pi * 180.0f), Space.Self);
		transform.Rotate(new Vector3(0f, -angleY / pi * 180.0f, 0f), Space.Self);
		transform.Rotate(new Vector3(-angleX / pi * 180.0f, 0f, 0f), Space.Self);
		transform.Translate(new Vector3(translation[0],
			translation[1],
			translation[2]));
	}

	void Start() {
        //Calibrate("1,0,0,0,1,0,0,0,1,1.9394323348999023,3.918166732788086,-62.5104248046875");
        //Hello();
		// gameObject.GetComponent<Camera>().pixelRect = new Rect(0, 0, Screen.width*2/3, Screen.height);
	}
}
