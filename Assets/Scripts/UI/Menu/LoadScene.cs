using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

	public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
        //update GameController
        GameController.gameState.UpdateScene(SceneManager.GetSceneByBuildIndex(sceneIndex));
    }
}
