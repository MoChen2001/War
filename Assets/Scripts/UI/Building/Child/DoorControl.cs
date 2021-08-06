using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControl : MonoBehaviour 
{
    private Transform m_Transform;
    private Quaternion closeRotation;
    private Quaternion openRotation;


    private void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        closeRotation = m_Transform.rotation;
        m_Transform.Rotate(m_Transform.up, -90);
        openRotation =  m_Transform.rotation;

        CloseDoor();
    }



    public void OpenDoor()
    {
        m_Transform.rotation = openRotation;
    }

    public void CloseDoor()
    {
        m_Transform.rotation = closeRotation;
    }

}
