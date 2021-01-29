using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class SceneController : MonoBehaviour {
    const float _transitionCooldown = 3.0f;


    public void OnGameWon() {
        IEnumerator WaitAndRestart() {
            yield return new WaitForSecondsRealtime(_transitionCooldown);
            LoadScene(GameScene.play);
        }

        StartCoroutine(WaitAndRestart());
    }


    private void Update() {
        Scene scene = SceneManager.GetActiveScene();

        if (scene.name == MenuScene.menu) {
            if (Input.GetKeyDown(MenuKey.gameStart)) {
                LoadScene(GameScene.play);
            } else if (Input.GetKeyDown(MenuKey.gameQuit)) {
                Debug.Log("Quitting");
                Application.Quit();
            }
        } else if (scene.name == GameScene.play) {
            if (Input.GetKeyDown(GameKey.gameReset)) {
                GameInfo.Reset();

                LoadScene(GameScene.play);
            } else if (Input.GetKeyDown(GameKey.gameMenu)) {
                LoadScene(MenuScene.menu);
            }
        } else {
            Debug.LogError("Unhandled scene: " + scene.name);
        }
    }


    private void LoadScene(string scene) {
        Debug.Log("Loading scene: " + scene);
        SceneManager.LoadScene(scene);
    }
}
