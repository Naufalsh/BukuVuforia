using UnityEngine;
using UnityEngine.UI; // Penting untuk mengakses komponen Image
using System.Collections; // Penting untuk Coroutine
using UnityEngine.SceneManagement; // Penting untuk beralih Scene

public class SplashScreenManager : MonoBehaviour
{
    public Image[] splashImages; // Array untuk menampung ketiga gambar splash screen
    public float fadeDuration = 1.5f; // Durasi fade in/out
    public float displayDuration = 2.0f; // Durasi gambar diam setelah fade in
    public string nextSceneName = "MainMenu"; // Nama Scene berikutnya setelah splash screens

    private int currentSplashIndex = 0;

    void Start()
    {
        // Pastikan semua splash screen tersembunyi di awal
        foreach (Image img in splashImages)
        {
            Color c = img.color;
            c.a = 0f; // Set alpha menjadi 0 (transparan)
            img.color = c;
        }

        // Mulai coroutine untuk mengelola urutan splash screen
        StartCoroutine(PlaySplashScreens());
    }

    IEnumerator PlaySplashScreens()
    {
        yield return null; // Tunggu satu frame agar semua Start() selesai

        for (int i = 0; i < splashImages.Length; i++)
        {
            currentSplashIndex = i;

            // Fade In
            yield return StartCoroutine(FadeImage(splashImages[currentSplashIndex], 0f, 1f, fadeDuration));

            // Tahan gambar selama displayDuration
            yield return new WaitForSeconds(displayDuration);

            // Jika ini bukan splash screen terakhir, lakukan fade out
            if (currentSplashIndex < splashImages.Length - 1)
            {
                yield return StartCoroutine(FadeImage(splashImages[currentSplashIndex], 1f, 0f, fadeDuration));
            }
            // Jika ini adalah splash screen terakhir, tunggu sebentar lalu pindah scene tanpa fade out
            else
            {
                 // Opsional: Anda bisa menambahkan fade out untuk splash screen terakhir juga jika mau
                 // yield return StartCoroutine(FadeImage(splashImages[currentSplashIndex], 1f, 0f, fadeDuration));
            }
        }

        // Setelah semua splash screen selesai, pindah ke Scene berikutnya
        SceneManager.LoadScene(nextSceneName);
    }

    IEnumerator FadeImage(Image imageToFade, float startAlpha, float endAlpha, float duration)
    {
        float timer = 0f;
        Color currentColor = imageToFade.color;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, timer / duration);
            currentColor.a = newAlpha;
            imageToFade.color = currentColor;
            yield return null; // Tunggu frame berikutnya
        }

        // Pastikan alpha diatur ke nilai akhir yang tepat
        currentColor.a = endAlpha;
        imageToFade.color = currentColor;
    }
}