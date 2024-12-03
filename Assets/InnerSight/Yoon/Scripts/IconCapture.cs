using System.IO;
using UnityEngine;

public class IconCapture : MonoBehaviour
{
    public Camera captureCamera; // 캡처용 카메라
    public RenderTexture renderTexture; // 연결된 Render Texture
    public string savePath = "Assets/Icons/"; // 저장 경로

    void Update()
    {
        // 스페이스바를 눌러 캡처
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CaptureIcon("CapturedIcon");
        }
    }
    public void CaptureIcon(string fileName)
    {
        // Render Texture를 활성화
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // 텍스처를 읽어오기
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        captureCamera.Render();
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();

        // PNG로 저장
        byte[] bytes = texture.EncodeToPNG();
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);
        File.WriteAllBytes(savePath + fileName + ".png", bytes);

        // 정리
        RenderTexture.active = currentRT;
        Destroy(texture);
        Debug.Log("아이콘 저장 완료: " + savePath + fileName + ".png");
    }
}
