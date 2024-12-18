using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Çarpan obje FallingObject sınıfına sahip mi kontrol et
        FallingObject fallingObject = collision.GetComponent<FallingObject>();

        if (fallingObject != null)
        {
            // Eğer obje düşmesini bitirdiyse (isFalling == false)
            if (!fallingObject.isFalling)
            {
                Debug.Log("Game Over: Yerleşmiş bir obje üst sınıra ulaştı!");
                UIManager.Instance.GameOver(); // UIManager üzerinden oyunu bitir
            }
        }
    }
}