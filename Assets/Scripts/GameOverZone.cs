using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private Dictionary<FallingObject, float> objectsInZone = new Dictionary<FallingObject, float>();
    private const float timeToTriggerGameOver = 3f; // 3 saniye durma süresi

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FallingObject fallingObject = collision.GetComponent<FallingObject>();

        if (fallingObject != null && !fallingObject.isFalling)
        {
            // Eğer obje zaten listede değilse, giriş zamanını kaydet
            if (!objectsInZone.ContainsKey(fallingObject))
            {
                objectsInZone.Add(fallingObject, Time.time);
                Debug.Log($"{fallingObject.name} GameOverZone'a girdi.");
            }
        }
    }

    private void Update()
    {
        // GameOverZone'daki tüm objeleri kontrol et
        List<FallingObject> objectsToCheck = new List<FallingObject>(objectsInZone.Keys);
        foreach (FallingObject fallingObject in objectsToCheck)
        {
            // Obje hala mevcutsa ve içeride 3 saniye geçmişse
            if (fallingObject != null && Time.time - objectsInZone[fallingObject] >= timeToTriggerGameOver)
            {
                Debug.Log($"Game Over: {fallingObject.name} 3 saniye boyunca GameOverZone'da kaldı.");
                GameManager.Instance.GameOver();
                return; // Game Over olduğunda işlemi sonlandır
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FallingObject fallingObject = collision.GetComponent<FallingObject>();

        if (fallingObject != null && objectsInZone.ContainsKey(fallingObject))
        {
            // Obje Zone'dan çıktığında listeden kaldır
            objectsInZone.Remove(fallingObject);
            Debug.Log($"{fallingObject.name} GameOverZone'dan çıktı.");
        }
    }
}