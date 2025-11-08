using UnityEngine;



public class ObjectVisibility : MonoBehaviour
{
    public GameObject cube;

    public void ChangeCubeVisibility()
    {
        cube.SetActive(!cube.activeSelf);
    }
}
