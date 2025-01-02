using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject[] objects; // 5 farklı boyuttaki prefab'ler
    public LineRenderer dropLine; // DropLine referansı

    private GameObject currentObject;
    private Vector3 spawnPosition;

    private int nextObjectIndex; // Bir sonraki nesneyi takip etmek için
    private bool isMobile; // Platform kontrolü için

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

        // İlk spawn pozisyonunu belirle
        spawnPosition = new Vector3(0f, 5.5f, 0f); // Y ekseni sabit

        // İlk rastgele bir sonraki nesneyi seç
        nextObjectIndex = Random.Range(0, objects.Length);

        // İlk nesneyi spawn et
        SpawnNewObject();
    }

    void Update()
    {
        if (isMobile)
        {
            HandleMobileInput();
        }
        else
        {
            HandleMouseInput();
            HandleKeyboardInput(); // Klavye girdilerini işle
        }

        // DropLine pozisyonunu güncelle
        if (currentObject != null)
        {
            dropLine.SetPosition(0, new Vector3(currentObject.transform.position.x, currentObject.transform.position.y, 0));
            dropLine.SetPosition(1, new Vector3(currentObject.transform.position.x, -1.75f, 0));
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Sol fare tıklaması
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float clampedX = Mathf.Clamp(worldPoint.x, screenLeftLimit, screenRightLimit);

            // Tıklanan pozisyonu başlangıç olarak ayarla
            spawnPosition.x = clampedX;

            if (currentObject != null)
            {
                currentObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
                DropObject();
            }
        }
    }

    void HandleKeyboardInput()
    {
        float move = Input.GetAxis("Horizontal") * Time.deltaTime * 10f; // Klavyeden sağ-sol hareket
        spawnPosition.x = Mathf.Clamp(spawnPosition.x + move, screenLeftLimit, screenRightLimit);

        if (currentObject != null)
        {
            currentObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
        }
    }

    void HandleMobileInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                float clampedX = Mathf.Clamp(worldPoint.x, screenLeftLimit, screenRightLimit);

                // Dokunulan pozisyonu başlangıç olarak ayarla
                spawnPosition.x = clampedX;

                if (currentObject != null)
                {
                    currentObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y, 0);
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Parmağı kaldırınca objeyi bırak
                if (currentObject != null)
                {
                    DropObject();
                }
            }
        }
    }

    void DropObject()
    {
        // Objenin fiziksel olarak düşmesini sağla
        currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;

        // DropLine pozisyonunu düşen objeye göre ayarla
        dropLine.SetPosition(0, new Vector3(currentObject.transform.position.x, currentObject.transform.position.y, 0));
        dropLine.SetPosition(1, new Vector3(currentObject.transform.position.x, -1.75f, 0));

        currentObject = null;
        dropLine.enabled = false; // DropLine'ı devre dışı bırak

        // Yeni kediyi her zaman ekranın ortasından başlatmak için pozisyonu sıfırla
        spawnPosition = new Vector3(0f, 5.5f, 0f);

        // Yeni nesne oluşturma işlemini zamanla
        Invoke(nameof(SpawnNewObject), 1f);
    }

    void SpawnNewObject()
    {
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            Debug.Log("Oyun bitti, yeni obje spawnlanmayacak.");
            return;
        }

        // Yeni nesneyi spawn et
        currentObject = Instantiate(objects[nextObjectIndex], spawnPosition, Quaternion.identity);
        currentObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        // Bir sonraki nesneyi rastgele seç
        nextObjectIndex = Random.Range(0, objects.Length);

        // DropLine'ı etkinleştir
        dropLine.enabled = true;
    }
}
