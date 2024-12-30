using UnityEngine;
using System.Linq; // Sahnedeki objeleri filtrelemek için

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objects; // 5 farklı boyuttaki prefab'ler
    public LineRenderer dropLine;

    private GameObject currentObject;
    private Vector3 spawnPosition;

    private int nextObjectIndex; // Bir sonraki nesneyi takip etmek için
    private bool isMobile; // Platform kontrolü için
    private bool isDragging = false; // Parmağın hareketi kontrolü için

    private float screenLeftLimit; // Ekranın sol sınırı
    private float screenRightLimit; // Ekranın sağ sınırı

    void Start()
    {
        // Platformun mobil olup olmadığını belirle
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        // Ekran sınırlarını hesapla
        Camera cam = Camera.main;
        screenLeftLimit = cam.ScreenToWorldPoint(new Vector3(0, 0, 0)).x + 0.5f; // Biraz içeriden başla
        screenRightLimit = cam.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x - 0.5f;

        // Spawn konumunu biraz aşağıya ayarla
        spawnPosition = new Vector3(0f, 30.5f, 0f); // Y değeri ayarlandı

        // İlk rastgele bir sonraki nesneyi seç
        nextObjectIndex = Random.Range(0, objects.Length);

        // İlk nesneyi spawn et
        SpawnNewObject();
    }

    void Update()
    {
        if (currentObject != null)
        {
            if (isMobile)
            {
                HandleMobileInput();
            }
            else
            {
                HandleKeyboardInput();
            }

            // DropLine pozisyonunu güncelle
            dropLine.SetPosition(0, spawnPosition);
            dropLine.SetPosition(1, new Vector3(spawnPosition.x, -1.75f, 1));
        }
        else
        {
            // Eğer bir nesne bırakılmışsa, DropLine’ı gizle
            dropLine.enabled = false;
        }
    }

    void HandleMobileInput()
    {
        float sensitivity = 0.005f; // Parmağın hareketine hassasiyet. Daha küçük bir değer hareketi yavaşlatır.

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Parmağın dokunması başladığında
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                // Parmağın hareketi sırasında objeyi sağa sola taşı
                float moveAmount = touch.deltaPosition.x * sensitivity; // Hareket miktarını hassasiyetle kontrol et
                spawnPosition.x = Mathf.Clamp(spawnPosition.x + moveAmount, screenLeftLimit, screenRightLimit);
                currentObject.transform.position = spawnPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Parmağı ekrandan kaldırınca topu bırak
                isDragging = false;
                DropObject();
            }
        }
    }


    void HandleKeyboardInput()
    {
        // Sağ-Sol Hareket
        float move = Input.GetAxis("Horizontal") * Time.deltaTime * 10f;
        spawnPosition.x = Mathf.Clamp(spawnPosition.x + move, screenLeftLimit, screenRightLimit);
        currentObject.transform.position = spawnPosition;

        if (Input.GetMouseButtonDown(0))
        {
            // Fare tıklamasıyla objeyi serbest bırak
            DropObject();
        }
    }

    void DropObject()
    {
        currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        currentObject = null;
        Invoke(nameof(SpawnNewObject), 1f); // Yeni nesneyi çağır
    }

    
   

    void SpawnNewObject()
    {
        
        // Eğer oyun bitti ise obje spawnlanmasın
        if (GameManager.Instance.isGameOver)
        {
            Debug.Log("Oyun bitti, yeni obje spawnlanmayacak.");
            return;
        }
        
        
        // Sahnedeki kedilerin en yüksek seviyesini kontrol et
        int maxLevel = GameObject.FindGameObjectsWithTag("Cat")
            .Select(cat => cat.GetComponent<FallingObject>().size)
            .DefaultIfEmpty(0) // Eğer sahnede kedi yoksa 0 döner
            .Max();

        // Spawn edilecek nesneyi belirle
        if (maxLevel >= 9)
        {
            nextObjectIndex = Random.Range(0, 5); // Element1, Element2, Element3, Element4 veya Element5
        }
        else if (maxLevel >= 7)
        {
            nextObjectIndex = Random.Range(0, 4); // Element1, Element2, Element3 veya Element4
        }
        else
        {
            nextObjectIndex = Random.Range(0, 3); // Element1, Element2 veya Element3
        }

        spawnPosition.y = 5.5f; // Nesne biraz aşağıdan başlar

        // Yeni nesneyi spawn et
        currentObject = Instantiate(objects[nextObjectIndex], spawnPosition, Quaternion.identity);
        currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // DropLine'ı yeniden etkinleştir ve yeni pozisyonu ayarla
        dropLine.enabled = true;

        // UI'yi güncelle
        FindObjectOfType<UIManager>().UpdateNextCatUI(objects[nextObjectIndex]);
    }
}
