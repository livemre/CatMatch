using UnityEngine;

public class CatStretchSquish : MonoBehaviour
{
    private Animator animator; // Animator bileşeni
    private bool hasPlayedAnimation = false; // Animasyonun bir kere oynatılmasını kontrol eder

    void Start()
    {
        animator = GetComponent<Animator>(); // Animator bileşenini al
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Eğer zemin ile çarpışırsa ve animasyon daha önce oynatılmadıysa
        if (collision.gameObject.CompareTag("Ground") && !hasPlayedAnimation)
        {
            animator.SetBool("Stretch-Squish", true); // Stretch-Squish animasyonunu başlat
            Debug.Log("Zeminde");
            hasPlayedAnimation = true; // Animasyonun tekrar oynatılmasını engelle
        }
    }

    void Update()
    {
        // Eğer animasyon oynadıysa ve tamamlandıysa, Stretch-Squish parametresini kapat
        if (hasPlayedAnimation && animator.GetCurrentAnimatorStateInfo(0).IsName("Stretch-Squish"))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f) // Animasyon tamamlandı mı?
            {
                animator.SetBool("Stretch-Squish", false); // Animasyon parametresini kapat
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Zeminle temas sona erdiğinde animasyonu yeniden çalıştırabilir hale getir
        if (collision.gameObject.CompareTag("Ground"))
        {
            hasPlayedAnimation = false;
        }
    }
}