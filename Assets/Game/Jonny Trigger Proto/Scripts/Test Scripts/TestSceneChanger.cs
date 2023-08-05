using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestSceneChanger : MonoBehaviour
{
    [SerializeField, SceneDetails] private SerializedScene name;
    void Start()
    {
        SceneManager.LoadScene(1);
    }
}
