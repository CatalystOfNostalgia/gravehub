using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager gameManager;

    // Store all of the prefabs here
    public GameObject firePrefab;
    public GameObject waterPrefab;
    public GameObject airPrefab;
    public GameObject earthPrefab;

    /**
    public Building fireTreePrefab;
    public Building windmillPrefab;
    public Building cavePrefab;
    public Building pondPrefab;
    public Building headquartersPrefab;
    **/

    // Store private list of Game Objects
    private List<GameObject> fireActive;
    private List<GameObject> waterActive;
    private List<GameObject> earthActive;
    private List<GameObject> airActive;

    void Start()
    {
        if (gameManager == null)
        {
            gameManager = this;
        }
        else if (gameManager != this)
        {
            Destroy(gameObject);
        }
        fireActive = new List<GameObject>();
        waterActive = new List<GameObject>();
        earthActive = new List<GameObject>();
        airActive = new List<GameObject>();
    }

    public void SpawnBuildings()
    {
        // Generates buildings from resource list
        foreach (KeyValuePair<Tuple, Building> pair in SaveState.state.resourceBuildings)
        {
            Vector3 target = GetSpawnVector(pair.Key);
            target.z = target.z - 1;
            Instantiate(pair.Value, target, Quaternion.identity);
        }
    }

    /**
     * Spawns all of the poofs at one location
     * on game start
     */
    public void SpawnPoofs()
    {
        int fireTotal = SaveState.state.fireEle;
        int waterTotal = SaveState.state.waterEle;
        int earthTotal = SaveState.state.earthEle;
        int airTotal = SaveState.state.airEle;

        int fireLeft = fireTotal;
        int waterLeft = waterTotal;
        int earthLeft = earthTotal;
        int airLeft = airTotal;

        // Fire loop
        if (fireActive.Count < fireTotal && fireLeft > 0)
        {
            for (int i = 0; i < fireTotal; i++)
            {
                SpawnPoof(firePrefab, GetRandomSpawnPoint(), fireActive);
                fireLeft--;
            }
        }

        // Water loop
        if (waterActive.Count < waterTotal && waterLeft > 0)
        {
            for (int i = 0; i < waterTotal; i++)
            {
                SpawnPoof(waterPrefab, GetRandomSpawnPoint(), waterActive);
                waterLeft--;
            }
        }

        // Earth loop
        if (earthActive.Count < earthTotal && earthLeft > 0)
        {
            for (int i = 0; i < earthTotal; i++)
            {
                SpawnPoof(earthPrefab, GetRandomSpawnPoint(), earthActive);
                earthLeft--;
            }
        }

        // Air loop
        if (airActive.Count < airTotal && airLeft > 0)
        {
            for (int i = 0; i < airTotal; i++)
            {
                SpawnPoof(airPrefab, GetRandomSpawnPoint(), airActive);
                airLeft--;
            }
        }
    }

    /**
     * Allows the caller to spawn a poof
     */
    public void SpawnPoof(GameObject prefab, Tuple spawnPoint, List<GameObject> active)
    {
        Vector3 position = GetSpawnVector(spawnPoint);
        GameObject go = Instantiate(prefab, position, Quaternion.identity) as GameObject;
        active.Add(go);
        CharacterScript cs = go.GetComponent<CharacterScript>();
        cs.onTile = TileScript.grid.GetTile(spawnPoint);
    }

    /**
     * Generates a random tuple for spawning poofs
     */
    public Tuple GetRandomSpawnPoint()
    {
        return TileScript.grid.tiles[(int)Random.Range(0, TileScript.grid.tiles.Length - 1)].index;
    }

    /**
     * Returns a Vector3 associated with this grid position
     */
    public Vector3 GetSpawnVector(Tuple spawnPoint)
    {
        return TileScript.grid.GetTile(spawnPoint).transform.position;
    }
}
