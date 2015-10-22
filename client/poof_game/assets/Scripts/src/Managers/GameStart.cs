using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour {

    public TileScript grid;
    public GameManager gManager;
    public SaveState saveState;
	public BuildingManager bManager;

	// Adds all essential game objects to scene
	void Awake () {
        Instantiate(grid, new Vector3(0, 0, 15), Quaternion.identity);
        Instantiate(gManager, new Vector3(0, 0, 0), Quaternion.identity);
		Instantiate (bManager, new Vector3 (0, 1, 0), Quaternion.identity);
        Instantiate(saveState, new Vector3(0, 2, 0), Quaternion.identity);
        StartCoroutine("RenderScene");
        // Be careful! Anything after this coroutine will run 
        // before coroutine finishes
    }

    /**
     * Use this function to build the scene
     * The coroutine allows us to let Unity 
     * run other tasks before running another
     * iteration of the loop
     */
    private IEnumerator RenderScene()
    {
        while (!SceneIsReady())
        {
            yield return null;
        }
        Debug.Log("Scene is ready to load all data!");
        SaveState.state.PullFromServer();
        GameManager.gameManager.SpawnBuildings();
        GameManager.gameManager.SpawnPoofs();
    }

    /**
     * Returns true only if all essential game objects
     * are built
     */
    private bool SceneIsReady()
    {
        if (GameManager.gameManager == null || SaveState.state == null || TileScript.grid == null || BuildingManager.manager == null)
        {
            return false;
        }
        return true;
    }

    public void TestJSON()
    {
		SaveState.state.PullFromServer ();
    }
}
