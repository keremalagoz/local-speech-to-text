# Whisper Speech to Text

Whisper Speech to Text, OpenAI'ın güçlü Whisper modelini kullanarak ses dosyalarınızı doğrudan bilgisayarınızda metne dönüştürmenizi sağlayan kullanıcı dostu bir Windows masaüstü uygulamasıdır. Gizliliğinize önem verenler ve yerel çözümler arayanlar için idealdir.

## 🌟 Öne Çıkan Özellikler

*   **Yüksek Doğrulukta Transkripsiyon:** OpenAI Whisper modeli ile güçlendirilmiştir.
*   **Kullanıcı Dostu Arayüz:** C# Windows Forms ile geliştirilmiş, basit ve anlaşılır bir arayüz sunar.
*   **Çevrimdışı Çalışma:** İnternet bağlantısı olmadan (model dosyaları ve FFmpeg indirildikten/paketlendikten sonra) transkripsiyon yapabilme.
*   **Model Seçimi:** Farklı ihtiyaçlara ve sistem kaynaklarına göre çeşitli Whisper modelleri (tiny, base, small, medium, large, large-v3, large-v3-turbo) arasından seçim yapabilme.
*   **İşlem Birimi Seçimi:** CUDA destekli NVIDIA GPU'nuzda hızlı transkripsiyon veya CPU üzerinde çalışma imkanı.
*   **Geniş Format Desteği:** Entegre FFmpeg sayesinde MP3, WAV, M4A, FLAC ve daha birçok yaygın ses formatını destekler.
*   **Kolay Kurulum:** Windows için hazırlanan `.exe` kurulum paketi ile hızlı ve basit yükleme.
*   **Bağımsız Çalışma:** Gerekli tüm bileşenler (FFmpeg, Whisper modelleri) kurulum paketine dahildir, ek bir kuruluma ihtiyaç duymazsınız.

## 🛠️ Kurulum

Uygulamayı kullanmaya başlamak için aşağıdaki adımları izleyebilirsiniz:

1.  **İndirme:**
    *   En son kararlı sürümü GitHub **[Releases](../../releases)** sayfamızdan indirin.
    *   `whisper_setup.exe` adlı kurulum dosyasını bilgisayarınıza indirin.
2.  **Kurulum:**
    *   İndirdiğiniz `whisper_setup.exe` dosyasını çalıştırın.
    *   Kurulum sihirbazındaki adımları takip edin.
    *   Uygulama varsayılan olarak `C:\Program Files (x86)\Whisper Speech to Text` (veya 64-bit için `C:\Program Files\Whisper Speech to Text`) altına kurulacaktır.
    *   Kurulum sırasında masaüstü kısayolu oluşturma seçeneği sunulacaktır.

## 🚀 Kullanım Kılavuzu

1.  Uygulamayı Başlat Menüsü'ndeki "Whisper Speech to Text" grubundan veya (oluşturulduysa) masaüstü kısayolundan çalıştırın.
2.  **"Ses Dosyası Seç"** butonuna tıklayarak transkribe etmek istediğiniz ses dosyasını (.mp3, .wav, .m4a vb.) seçin.
3.  **"İşlem Birimi Seçin"** açılır menüsünden:
    *   Eğer uyumlu bir NVIDIA ekran kartınız varsa ve daha hızlı işlem istiyorsanız **"cuda"** seçeneğini tercih edin.
    *   Diğer durumlarda veya GPU'nuz yoksa **"cpu"** seçeneğini kullanın.
4.  **"Model Seçin"** açılır menüsünden kullanmak istediğiniz Whisper modelini seçin.
    *   **Öneri:** `base` veya `small` modelleri çoğu genel kullanım için iyi bir hız/doğruluk dengesi sunar.
    *   Daha büyük modeller (`medium`, `large`, `large-v3`) daha doğru sonuçlar verir ancak daha fazla sistem kaynağı (RAM, VRAM) tüketir ve işlem süresi daha uzundur.
5.  **"Transkribe Et"** butonuna tıklayın.
6.  İşlem tamamlandığında transkripsiyon metni aşağıdaki sonuç alanında görünecektir. Metni seçip kopyalayabilirsiniz.
7.  İşlem sırasında ilerleme ve olası bilgilendirme mesajları için "Loglar" (veya benzeri) bölümünü kontrol edebilirsiniz.

## ⚙️ Geliştirme ve Kaynak Kod

Bu proje açık kaynaklıdır ve katkılarınızı bekliyoruz!

**Proje Yapısı:**

*   **`CSharpUI/`**: C# Windows Forms kullanıcı arayüzü projesi.
*   **`PythonEngine/`**: OpenAI Whisper modelini kullanan Python transkripsiyon motoru ve ilgili dosyalar (`transcribe_audio.py`, gömülü modeller, FFmpeg).
*   **`Setup/`**: Inno Setup script dosyası (`.iss`) Windows kurulum paketini oluşturmak için.

**Gereksinimler (Python Kısmı İçin):**

*   Python 3.10.x
*   Gerekli Python kütüphaneleri için `PythonEngine/requirements.txt` dosyasına bakınız.
    ```bash
    pip install -r PythonEngine/requirements.txt
    ```
*   FFmpeg sistem PATH'inde olmalıdır.

**Gereksinimler (C# Kısmı İçin):**

*   .NET Framework 4.7.2
*   Visual Studio 2022

**Python EXE'sini Oluşturma (PyInstaller):**

`PythonEngine` klasörü içindeki `transcribe_audio.py` script'ini `.exe`'ye dönüştürmek için kullanılan örnek PyInstaller komutu (detaylar için projedeki notlara veya build script'ine bakınız):

```bash
# ÖNEMLİ: Bu komutu PythonEngine klasörünün bir üst dizininden (proje kökünden) çalıştırın
# ve --add-data içindeki whisper/assets yolunu kendi sanal ortamınıza göre güncelleyin!
pyinstaller --name WhisperTranscriber PythonEngine/transcribe_audio.py ^
    --distpath ./PythonEngine/dist ^
    --workpath ./PythonEngine/build ^
    --specpath ./PythonEngine ^
    --onedir ^
    --add-binary "PythonEngine/Binaries/ffmpeg.exe:Binaries" ^
    --add-binary "PythonEngine/Binaries/tbb12.dll:Binaries" ^
    --add-data "PythonEngine/embedded_whisper_models:embedded_whisper_models" ^
    --add-data "C:/Path/To/Your/venv/Lib/site-packages/whisper/assets:whisper/assets" ^
    --hidden-import=tiktoken_ext.openai_public ^
    --hidden-import=tiktoken_ext
```


Not: Yukarıdaki PyInstaller komutundaki yolları kendi proje yapınıza ve sanal ortam yolunuza göre düzenlemeniz gerekebilir.

## 🧱 Kullanılan Teknolojiler

**Arayüz**: C# Windows Forms, .NET Framework

**Transkripsiyon Motoru**: Python, OpenAI Whisper, PyTorch

**Ses İşleme**: FFmpeg

**Python Paketleme**: PyInstaller

**Windows Kurulum Paketi**: Inno Setup

## 📄 Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Detaylar için LICENSE dosyasına bakınız.

Kullanılan üçüncü taraf kütüphaneler ve araçlar (OpenAI Whisper, PyTorch, FFmpeg, Numba, TikToken, Inno Setup, PyInstaller vb.) kendi lisanslarına tabidir. Lütfen ilgili projelerin lisans belgelerini inceleyiniz.

## 🙌 Katkıda Bulunma

Her türlü katkıya açığız! Hata bildirimleri, özellik istekleri veya kod katkıları için lütfen GitHub Issues veya Pull Requests bölümlerini kullanın.

Projeyi Fork'layın.

Yeni bir özellik dalı (feature/AmazingFeature) veya hata düzeltme dalı (fix/BugFix) oluşturun.

Değişikliklerinizi Commit'leyin (git commit -m 'Add some AmazingFeature').

Dalınızı Push'layın (git push origin feature/AmazingFeature).

Bir Pull Request açın.

## 📞 İletişim ve Destek

Bir sorunla karşılaşırsanız veya bir sorunuz varsa, lütfen öncelikle GitHub Issues sayfasını kontrol edin veya yeni bir issue oluşturun.

Kerem Alagöz tarafından geliştirilmiştir.