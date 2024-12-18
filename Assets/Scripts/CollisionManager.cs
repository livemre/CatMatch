using UnityEngine;
using System.Collections; // IEnumerator kullanımı için gerekli

public class CollisionManager : MonoBehaviour
{
    [Header("Prefabler (Size 1, Size 2, vb. sırayla)")]
    public GameObject[] objectPrefabs;

    public float mergeDistance = 0.5f;

    void Update()
    {
        // FallingObject tag'li tüm objeleri bul
        GameObject[] objects = GameObject.FindGameObjectsWithTag("FallingObject");
        // Debug.Log("Sahnede FallingObject tagli obje sayisi: " + objects.Length);

        // Tüm objeleri ikili karşılaştır
        for (int i = 0; i < objects.Length; i++)
        {
            for (int j = i + 1; j < objects.Length; j++)
            {
                CheckAndMerge(objects[i], objects[j]);
            }
        }
    }

    void CheckAndMerge(GameObject objA, GameObject objB)
    {
        float dist = Vector3.Distance(objA.transform.position, objB.transform.position);

        // Eğer mesafe büyükse birleşme yok
        if (dist > mergeDistance)
            return;

        int sizeA = GetObjectSize(objA);
        int sizeB = GetObjectSize(objB);

        // Eğer boyutlar farklıysa birleşme yok
        if (sizeA != sizeB)
            return;

        // Aynı boyuttaki iki obje birleşebilir
        int newSize = sizeA + 1;
        int newIndex = newSize - 1;

        if (newIndex >= 0 && newIndex < objectPrefabs.Length)
        {
            Vector3 mergePos = (objA.transform.position + objB.transform.position) / 2f;

            // Yeni objeyi oluştur
            GameObject newObj = Instantiate(objectPrefabs[newIndex], mergePos, Quaternion.identity);

            // Yeni objenin collider’ını kısa süre devre dışı bırak
            Collider2D newCollider = newObj.GetComponent<Collider2D>();
            if (newCollider != null)
            {
                newCollider.enabled = false;
                StartCoroutine(EnableColliderAfterDelay(newCollider, 0.1f));
            }

            // Eski objeleri yok et
            Destroy(objA);
            Destroy(objB);
            
            Debug.Log($"Birlesme gerceklesti: {objA.name} + {objB.name} => {newObj.name}");
        }
        else
        {
            Debug.LogWarning("Yeni boyut için prefab bulunamadı. objectPrefabs dizisini kontrol et.");
        }
    }

    int GetObjectSize(GameObject obj)
    {
        string objName = obj.name.Replace("(Clone)", "").Trim();
        int index = objName.IndexOf("Size");
        if (index >= 0)
        {
            string sizeStr = objName.Substring(index + 4);
            if (int.TryParse(sizeStr, out int sizeVal))
            {
                return sizeVal;
            }
        }

        Debug.LogWarning($"Objenin adindan size alinamadi: {obj.name}");
        return 1; // Varsayılan
    }

    IEnumerator EnableColliderAfterDelay(Collider2D col, float delay)
    {
        yield return new WaitForSeconds(delay);
        col.enabled = true;
    }
}