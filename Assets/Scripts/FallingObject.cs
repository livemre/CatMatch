using UnityEngine;

public class FallingObject : MonoBehaviour
{
    public int size; 
    public GameObject nextObjectPrefab; // Bir üst seviyedeki obje
    public GameObject explosionPrefab; // Patlama efekti

    private bool isCollidingWithSameSize = false;
    private float collisionStartTime = 0f;
    private FallingObject otherObject;
    public bool isFalling = true;
    private float fallTimer = 10f;  // Düşme süresi (2 saniye)
    public AudioClip mergeSound; // Birleşme sırasında çalınacak ses
    private bool hasPlayedGroundSound = false;
   

    private void Start()
    {
    
        // Objeler ilk başta düşüyor olarak ayarlanır
        isFalling = true;
        
      
    }
    
    private void Update()
    {
        
        
        // Zamanlayıcıyı azalt
        if (isFalling)
        {
            fallTimer -= Time.deltaTime;

            // 2 saniye geçtiyse düşmeyi bitmiş kabul et
            if (fallTimer <= 0f)
            {
                isFalling = false;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        otherObject = collision.gameObject.GetComponent<FallingObject>();
        if (otherObject != null && otherObject.size == size)
        {
            // Aynı boyutta bir obje ile çarpışma başladı
            isCollidingWithSameSize = true;
            collisionStartTime = Time.time; // Çarpışma başlangıç zamanı
        }
        
      
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Çarpışma devam ederken kontrol et
        if (isCollidingWithSameSize && otherObject != null && otherObject.size == size)
        {
            float elapsed = Time.time - collisionStartTime;
            if (elapsed > 0.01f) 
            {
                // 0.01 saniyeyi geçti ve hala çarpışıyor, birleşme gerçekleşsin
                MergeObjects(otherObject);
            }
        }
        
        
        
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Çarpışma bittiğinde resetle
        FallingObject exitedObject = collision.gameObject.GetComponent<FallingObject>();
        if (exitedObject == otherObject)
        {
            isCollidingWithSameSize = false;
            otherObject = null;
        }
    }

    private void MergeObjects(FallingObject other)
    {
        
        // Eğer bir üst sınıf obje varsa birleşme gerçekleşir
        if (nextObjectPrefab != null)
        {
            // Birleşme noktası
            Vector3 mergePos = (transform.position + other.transform.position) / 2f;

            // Patlama efektini oluştur
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, mergePos, Quaternion.identity);
            }


            AudioManager.Instance.PlaySound(mergeSound);
            
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif


            // Yeni objeyi oluştur
            Instantiate(nextObjectPrefab, mergePos, Quaternion.identity);

            // Puanı ekle (bir üst objenin seviyesi baz alınarak)
            int points = CalculatePoints(size);
            FindObjectOfType<UIManager>().AddScore(points);

            // Eski objeleri yok et
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
        else
        {
            // Eğer bir üst sınıf obje yoksa, hiçbir şey yapma
            Debug.Log($"{gameObject.name} ve {other.gameObject.name} birleşemedi çünkü üst sınıf yok.");
        }
    }

    /// <summary>
    /// Puanı objenin seviyesine göre hesaplar.
    /// </summary>
    /// <param name="currentSize">Objenin boyutu (seviyesi).</param>
    /// <returns>Hesaplanan puan.</returns>
    private int CalculatePoints(int currentSize)
    {
        return (int)Mathf.Pow(2, currentSize + 2); // Örneğin size=1 -> 5, size=2 -> 10, size=3 -> 20
    }
    
    

   
}