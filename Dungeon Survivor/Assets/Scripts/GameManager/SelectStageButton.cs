using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectStageButton : MonoBehaviour
{
    public StageData stageData;
    public void StartGamePlay(string stageToPlay)
    {
        SceneManager.LoadScene("Essential", LoadSceneMode.Single);
        SceneManager.LoadScene(stageToPlay, LoadSceneMode.Additive);
    }

}
