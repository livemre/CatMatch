using UnityEngine;

public class Blink2 : MonoBehaviour
{
    private Animator animator; // Animator bileşeni referansı
    public float minBlinkTime = 3f; // Minimum bekleme süresi
    public float maxBlinkTime = 8f; // Maksimum bekleme süresi
    private float blinkTimer; // Göz kırpma için sayaç

    void Start()
    {
        animator = GetComponent<Animator>();
        ResetBlinkTimer(); // İlk zamanlayıcıyı ayarla
    }

    void Update()
    {
        // Zamanlayıcıyı azalt
        blinkTimer -= Time.deltaTime;

        if (blinkTimer <= 0f)
        {
            TriggerBlink(); // Göz kırpma animasyonunu tetikle
            ResetBlinkTimer(); // Zamanlayıcıyı sıfırla
        }
    }

    private void TriggerBlink()
    {
        animator.SetTrigger("Blink2"); // Animator'daki "Blink" tetikleyicisini çalıştır
    }

    private void ResetBlinkTimer()
    {
        // Zamanlayıcıyı rastgele bir süreye ayarla
        blinkTimer = Random.Range(minBlinkTime, maxBlinkTime);
    }
}