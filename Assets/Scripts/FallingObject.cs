using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int size;
    public GameObject nextObjectPrefab; // Bir üst seviyedeki obje
    public GameObject explosionPrefab; // Patlama efekti
    public Animator animator; // Animator bileşeni
    public bool isFalling = true;

    public AudioClip mergeSound; // Birleşme sırasında çalınacak ses

    private bool isCollidingWithSameSize = false;
    private float collisionStartTime = 0f; // Çarpışma başlangıç zamanı
    private FallingObject otherObject; // Çarpıştığı diğer obje referansı
    private const float mergeWaitTime = 0.03f; // Birleşme için bekleme süresi

    private void Start()
    {
        // Animator bileşenini al
        animator = GetComponent<Animator>();

        // Objeler ilk başta düşüyor olarak ayarlanır
        isFalling = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Çarpışan obje ile animasyonu tetikle
        TriggerStretchAnimation(animator);

        // Çarpıştığı tüm objelerin animasyonlarını tetikle
        foreach (ContactPoint2D contact in collision.contacts)
        {
            Animator otherAnimator = contact.collider.GetComponent<Animator>();
            if (otherAnimator != null)
            {
                TriggerStretchAnimation(otherAnimator);
            }
        }

        // Eğer zemin ya da "Cat" tag'li bir objeye çarptıysa düşmeyi durdur
        if (isFalling && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Cat")))
        {
            Debug.Log($"{gameObject.name} yere ya da bir kediye çarptı, 'Stretch' animasyonu tetiklendi.");
            isFalling = false;
        }

        // Aynı boyuttaki FallingObject ile çarpışma kontrolü
        otherObject = collision.gameObject.GetComponent<FallingObject>();
        if (otherObject != null && otherObject.size == size)
        {
            isCollidingWithSameSize = true;
            collisionStartTime = Time.time; // Çarpışma başlangıç zamanı
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Eğer aynı boyutta bir obje ile çarpışıyorsa birleşme kontrolü
        if (isCollidingWithSameSize && otherObject != null && otherObject.size == size)
        {
            float elapsed = Time.time - collisionStartTime;
            if (elapsed > mergeWaitTime)
            {
                MergeObjects(otherObject);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        FallingObject exitedObject = collision.gameObject.GetComponent<FallingObject>();
        if (exitedObject == otherObject)
        {
            isCollidingWithSameSize = false;
            otherObject = null;
        }
    }

    private void MergeObjects(FallingObject other)
    {
        if (nextObjectPrefab != null)
        {
            Vector3 mergePos = (transform.position + other.transform.position) / 2f;

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, mergePos, Quaternion.identity);
            }

            AudioManager.Instance.PlaySound(mergeSound);

#if UNITY_ANDROID || UNITY_IOS
            Taptic.Success();
#endif

            Instantiate(nextObjectPrefab, mergePos, Quaternion.identity);

            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log($"{gameObject.name} ve {other.gameObject.name} birleşemedi çünkü üst sınıf obje yok.");
        }
    }

    private void TriggerStretchAnimation(Animator animator)
    {
        if (animator != null)
        {
            animator.SetTrigger("Stretch");
        }
    }
}
