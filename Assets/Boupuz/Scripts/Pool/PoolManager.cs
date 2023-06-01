using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;
    [SerializeField] private List<PrefabPool> _listPrefab;
    private List<ObjectPool> _listObject = new List<ObjectPool>();

    public void Init()
    {
        PoolManager.Instance = this;
        if (_listPrefab != null)
        {
            for (int i = 0; i < _listPrefab.Count; i++)
            {
                if (_listObject != null)
                {
                    GameObject gameObject = new GameObject();
                    gameObject.name = _listPrefab[i].Name;
                    gameObject.transform.SetParent(this.transform);

                    ObjectPool objectPool = new ObjectPool();
                    objectPool.Name = _listPrefab[i].Name;
                    objectPool.Parent = gameObject;
                    objectPool.ListObject = CreateObjects(_listPrefab[i].Prefab, gameObject.transform, _listPrefab[i].Total);

                    _listObject.Add(objectPool);
                }
            }
        }
        DontDestroyOnLoad(this);
    }

    public List<GameObject> CreateObjects(GameObject prefab, Transform parent, int amount)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.SetParent(parent);
            gameObject.SetActive(false);
            gameObjects.Add(gameObject);
        }
        return gameObjects;
    }

    public List<GameObject> GetObjects(string name, int amount, Transform parent)
    {
        List<GameObject> listObject = new List<GameObject>();
        List<GameObject> listObjectCanGet = new List<GameObject>();
        if(_listObject.Where(obj=>obj.Name == name).Count() > 0)
        {
            if(_listObject.Where(obj => obj.Name == name).ToArray()[0].ListObject.Count() - amount >=0)
            {
                listObject = _listObject.Where(obj => obj.Name == name).ToArray()[0].ListObject;
                for (int i = 0; i < amount; i++)
                {
                    listObject[0].SetActive(true);
                    listObject[0].transform.SetParent(parent);
                    listObjectCanGet.Add(listObject[0]);
                    listObject.RemoveAt(0);
                }
            }
        }
        return listObjectCanGet;
    }
}
