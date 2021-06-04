using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public UnityEngine.UI.Text text;
    public int i;
    public int count;
    public float delay;
    public float speed;
    public GameObject scarabPrefab;
    public GameObject targetPrefab;
    private Entity scarabEntityPrefab;
    private Entity targetEntityPrefab;
    private EntityManager manager;
    private BlobAssetStore blobAssetStore;

    // Start is called before the first frame update
    void Awake()
    {
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        blobAssetStore = new BlobAssetStore();
        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);
        scarabEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(scarabPrefab, settings);
        targetEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(targetPrefab, settings);
    }

    private void Start()
    {
        StartCoroutine(SpawnDelay());
    }


    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }

    IEnumerator SpawnDelay()
    {
        i = 0;
        var spawnPos = this.transform.position;
        Entity targetEntity = manager.Instantiate(targetEntityPrefab);
        // Unity.Mathematics.Random random = new Unity.Mathematics.Random();
        while (i < count)
        {
            Entity newEntity = manager.Instantiate(scarabEntityPrefab);
            Translation spawnPosition = new Translation
            {
                Value = new float3(spawnPos.x, spawnPos.y, spawnPos.z)
            };
            Scale scale = new Scale
            {
                Value = UnityEngine.Random.Range(0.05f, 0.2f)
            };
            SpeedData scarabData = new SpeedData
            {
                entityToFollow = targetEntity,
                speed = this.speed
            };
            manager.AddComponentData(newEntity, spawnPosition);
            // manager.AddComponentData(newEntity, scale);
            manager.AddComponentData(newEntity, scarabData);

            yield return new WaitForSeconds(delay);
            // if (i % 5 == 0)
            // yield return null;
            i++;
            text.text = i.ToString();
        }
    }
}
