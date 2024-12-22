using UnityEngine;

public class CameraResizer : MonoBehaviour
{
    public float targetWidth = 1920f; // Referans genişlik
    public float targetHeight = 1080f; // Referans yükseklik
    public float pixelsPerUnit = 100f; // Piksel başına birim
    public float aspectRatio = 1f; // Varsayılan en-boy oranı

    private float currentTargetWidth;
    private float currentTargetHeight;
    private float currentPixelsPerUnit;
    private float currentAspectRatio;

    void Start()
    {
        // Başlangıçta kamerayı ayarla
        AdjustCamera();
    }

    void Update()
    {
        // Değerlerde değişiklik olup olmadığını kontrol et
        if (currentTargetWidth != targetWidth ||
            currentTargetHeight != targetHeight ||
            currentPixelsPerUnit != pixelsPerUnit ||
            currentAspectRatio != aspectRatio)
        {
            AdjustCamera(); // Kamerayı güncelle
        }
    }

    void AdjustCamera()
    {
        float targetAspect = targetWidth / targetHeight;
        float screenAspect = (float)Screen.width / (float)Screen.height;

        if (screenAspect >= targetAspect)
        {
            // Geniş ekran: Kamera yüksekliği sabit, genişliği ayarlanır
            Camera.main.orthographicSize = targetHeight / pixelsPerUnit / aspectRatio / 2f;
        }
        else
        {
            // Dar ekran: Kamera genişliği sabit, yüksekliği ayarlanır
            float differenceInSize = targetAspect / screenAspect;
            Camera.main.orthographicSize = (targetHeight / pixelsPerUnit / aspectRatio / 2f) * differenceInSize;
        }

        // Mevcut değerleri güncelle
        currentTargetWidth = targetWidth;
        currentTargetHeight = targetHeight;
        currentPixelsPerUnit = pixelsPerUnit;
        currentAspectRatio = aspectRatio;
    }
}