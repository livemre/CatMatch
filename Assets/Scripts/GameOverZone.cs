using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        FallingObject fallingObject = collision.GetComponent<FallingObject>();

        if (fallingObject != null && !fallingObject.isFalling)
        {
            Debug.Log("Game Over: Yerleşmiş bir obje üst sınıra ulaştı!");
            GameManager.Instance.GameOver(); // UIManager üzerinden oyunu bitir
        }
    }

}