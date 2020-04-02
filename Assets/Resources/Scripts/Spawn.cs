using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private Vector3 position;
    private bool isAvailable;

    private void Awake()
    {
        position = transform.position;
        isAvailable = true;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public void SetIsAvailable(bool available)
    {
        isAvailable = available;
    }

    public bool GetIsAvailable()
    {
        return isAvailable;
    }
}
