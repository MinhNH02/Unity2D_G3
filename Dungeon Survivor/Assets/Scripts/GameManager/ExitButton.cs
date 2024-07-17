using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(QuitApplication);
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
