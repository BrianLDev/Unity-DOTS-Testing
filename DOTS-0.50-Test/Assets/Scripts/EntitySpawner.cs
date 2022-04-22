using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

public class EntitySpawner : MonoBehaviour {

  [SerializeField] private Mesh entityMesh;
  [SerializeField] private Material entityMat;
  [SerializeField] private GameObject gameObjPrefab;
  [SerializeField] int xSize = 10;
  [SerializeField] int ySize = 10;
  [SerializeField] float spacing = 1.1f;
  private Entity entityPrefab;
  private World defaultWorld;
  private EntityManager entityManager;

  private void Start() {
    defaultWorld = World.DefaultGameObjectInjectionWorld;
    entityManager = defaultWorld.EntityManager;

    // create the settings to be used in EntityPrefab converter
    GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(defaultWorld, null);
    // convert the GameObject prefab into an Entity prefab
    entityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(gameObjPrefab, settings);
    
    // InstantiateEntity(new float3(2, -2, 2));     // Instantiate an entity using the prefab
    InstantiateEntityGrid(xSize, ySize, spacing);   // Instantiate a grid of entities

  }

  private void InstantiateEntity(float3 position) {
    // Instantiate the Entity prefab
    Entity myEntity = entityManager.Instantiate(entityPrefab);
    // move it to position
    entityManager.SetComponentData(myEntity, new Translation {
      Value = position
    });
  }

  private void InstantiateEntityGrid(int countX, int countY, float spacing) {
    for (int i=0; i<countX; i++) {
      for (int j=0; j<countY; j++) {
        InstantiateEntity(new float3(i * spacing, j * spacing, 0f));
      }
    }
  }

  private void SpawnEntitiesTest() {
    // MULTIPLE WAYS TO CREATE ENTITIES USING "PURE" ECS (VIA CODE)

    // first, create the EntityManager
    EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

    // 1) spawn an empty entity
    entityManager.CreateEntity();
    // 2) spawn an entity with specific components
    entityManager.CreateEntity(typeof(Translation), typeof(Rotation));
    // 3a) create an entity archetype (template)
    EntityArchetype archetype = entityManager.CreateArchetype(
      typeof(Translation),
      typeof(Rotation),
      typeof(RenderMesh),
      typeof(RenderBounds),
      typeof(LocalToWorld)
    );
    // 3b) spawn an entity based on the archetype
    Entity myEntity = entityManager.CreateEntity(archetype);
    // 3c) move that entity to a specific position
    entityManager.AddComponentData(myEntity, new Translation { 
      Value = new float3(2f, 2f, 2f)
    });
    // 3d) add a renderMesh to entity so that it's visible
    // NOTE: THIS DOESN'T SEEM TO WORK IN UNITY 0.50 NO MATTER WHAT I TRY. ENTITY IS NOT RENDERING.
    // MAY BE UNITY BUG OR SOMETHING, BUT EITHER WAY, USE GAMEOBJECT CONVERSION METHOD INSTEAD
    entityManager.AddSharedComponentData(myEntity, new RenderMesh {
      mesh = entityMesh,
      material = entityMat
    });
  }
}