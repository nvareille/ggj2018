using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WinUI : MonoBehaviour {

    public CameraCharacterController Cam;
    public EventSystem eventSys;
    public GameObject Btn;

    public void Quitt()
    {
        Application.Quit();
    }

    public void Restart()
    {
        Cam.Res();
    }

    public void Init()
    {
        eventSys.firstSelectedGameObject = Btn;
    }

}
