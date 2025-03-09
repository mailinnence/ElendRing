using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouse : MonoBehaviour
{

    public static mouse instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 변경되어도 유지
        }
        else
        {
            Destroy(gameObject); // 기존 인스턴스가 있으면 새 객체 삭제
        }
    }


    public void mouseOn()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void mouseOff()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
