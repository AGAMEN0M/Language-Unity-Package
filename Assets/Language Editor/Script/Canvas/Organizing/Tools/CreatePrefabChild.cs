using UnityEngine;

public class CreatePrefabChild : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject prefab;
    [Space(5)]
    [SerializeField] private GameObject objeto;

    public void CreatePrefab()
    {
        GameObject objetoFilho = objeto;
        GameObject prefabInstanciado = Instantiate(prefab, objetoFilho.transform.position, objetoFilho.transform.rotation);
        prefabInstanciado.transform.SetParent(objetoFilho.transform, false);
    }
}