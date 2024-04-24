using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectMap : MonoBehaviour
{
    [SerializeField] private string mapScene;
    
    // Start is called before the first frame update
    public void ChooseMap(string mapName)
    {
        mapScene = mapName;
    }
    
    public void ChooseDuration(int index)
    {
        Settings.SelectDuration(index);
    }

    // Update is called once per frame
    public void StartGame()
    {
        if (mapScene == string.Empty) return;
        Settings.currentScene = mapScene;
        SceneManager.LoadScene(mapScene);
    }
}
