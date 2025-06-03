[🇬🇧 English](#-english) | [🇹🇷 Türkçe](#-türkçe)

<a name="-english"></a>

## 🇬🇧 English

# Whisper Speech to Text

Whisper Speech to Text is a user-friendly Windows desktop application that allows you to transcribe your audio files directly on your computer using OpenAI's powerful Whisper model. It is ideal for those who prioritize privacy and are looking for a local solution.

## 🌟 Key Features

  * **High-Accuracy Transcription:** Powered by the OpenAI Whisper model.
  * **User-Friendly Interface:** A simple and clear interface developed with C\# Windows Forms.
  * **Offline Operation:** Ability to transcribe without an internet connection (after the model files and FFmpeg have been downloaded/packaged).
  * **Model Selection:** Choose from various Whisper models (tiny, base, small, medium, large, large-v3, large-v3-turbo) according to different needs and system resources.
  * **Processing Unit Selection:** Option for fast transcription on your CUDA-supported NVIDIA GPU or running on the CPU.
  * **Wide Format Support:** Supports many common audio formats like MP3, WAV, M4A, FLAC, and more, thanks to the integrated FFmpeg.
  * **Easy Installation:** Quick and simple setup with the `.exe` installer prepared for Windows.
  * **Standalone Operation:** All necessary components (FFmpeg, Whisper models) are included in the installation package; no additional setup is required.

## 🛠️ Installation

You can follow the steps below to start using the application:

1.  **Download:**
      * Download the latest stable version from our GitHub **[şüpheli bağlantı kaldırıldı]** page.
      * Download the setup file named `whisper_setup.exe` to your computer.
2.  **Install:**
      * Run the downloaded `whisper_setup.exe` file.
      * Follow the steps in the installation wizard.
      * The application will be installed by default under `C:\Program Files (x86)\Whisper Speech to Text` (or `C:\Program Files\Whisper Speech to Text` for 64-bit).
      * An option to create a desktop shortcut will be provided during installation.

## 🚀 User Guide

1.  Launch the application from the "Whisper Speech to Text" group in the Start Menu or from the desktop shortcut (if created).
2.  Click the **"Select Audio File"** button to choose the audio file you want to transcribe (.mp3, .wav, .m4a, etc.).
3.  From the **"Select Processing Unit"** dropdown menu:
      * If you have a compatible NVIDIA graphics card and want faster processing, choose the **"cuda"** option.
      * In other cases or if you don't have a GPU, use the **"cpu"** option.
4.  From the **"Select Model"** dropdown menu, choose the Whisper model you want to use.
      * **Recommendation:** The `base` or `small` models offer a good balance of speed and accuracy for most general uses.
      * Larger models (`medium`, `large`, `large-v3`) provide more accurate results but consume more system resources (RAM, VRAM) and have longer processing times.
5.  Click the **"Transcribe"** button.
6.  Once the process is complete, the transcription text will appear in the result area below. You can select and copy the text.
7.  During the process, you can check the "Logs" (or similar) section for progress and any informational messages.

## ⚙️ Development and Source Code

This project is open source, and we welcome your contributions\!

**Project Structure:**

  * **`CSharpUI/`**: The C\# Windows Forms user interface project.
  * **`PythonEngine/`**: The Python transcription engine using the OpenAI Whisper model and related files (`transcribe_audio.py`, embedded models, FFmpeg).
  * **`Setup/`**: The Inno Setup script file (`.iss`) for creating the Windows installation package.

**Requirements (For the Python Part):**

  * Python 3.10.x
  * See the `PythonEngine/requirements.txt` file for the necessary Python libraries.
    ```bash
    pip install -r PythonEngine/requirements.txt
    ```
  * FFmpeg must be in the system's PATH.

**Requirements (For the C\# Part):**

  * .NET Framework 4.7.2
  * Visual Studio 2022

**Building the Python EXE (PyInstaller):**

Example PyInstaller command used to convert the `transcribe_audio.py` script inside the `PythonEngine` folder into an `.exe` (see the notes or build script in the project for details):

```bash
# IMPORTANT: Run this command from the parent directory of PythonEngine (the project root)
# and update the --add-data path for whisper/assets to match your virtual environment!
pyinstaller --name WhisperTranscriber PythonEngine/transcribe_audio.py ^
    --distpath ./PythonEngine/dist ^
    --onedir ^
    --add-binary "PythonEngine/Binaries/ffmpeg.exe:Binaries" ^
    --add-binary "PythonEngine/Binaries/ffprobe.exe:Binaries" ^
    --add-binary "PythonEngine/Binaries/tbb12.dll:Binaries" ^
    --add-data "PythonEngine/embedded_whisper_models:embedded_whisper_models" ^
    --add-data "C:/Path/To/Your/venv/Lib/site-packages/whisper/assets:whisper/assets" ^
    --hidden-import=tiktoken_ext.openai_public ^
    --hidden-import=tiktoken_ext
```

Note: You may need to adjust the paths in the PyInstaller command above to match your project structure and virtual environment path.

## 🧱 Technologies Used

**Interface**: C\# Windows Forms, .NET Framework

**Transcription Engine**: Python, OpenAI Whisper, PyTorch

**Audio Processing**: FFmpeg

**Python Packaging**: PyInstaller

**Windows Installer**: Inno Setup

## 📄 License

This project is licensed under the MIT License. See the LICENSE file for details.

Third-party libraries and tools used (OpenAI Whisper, PyTorch, FFmpeg, Numba, TikToken, Inno Setup, PyInstaller, etc.) are subject to their own licenses. Please review the license documents of the respective projects.

## 🙌 Contributing

All contributions are welcome\! For bug reports, feature requests, or code contributions, please use the GitHub Issues or Pull Requests sections.

1.  Fork the Project.
2.  Create a new feature branch (`feature/AmazingFeature`) or a bugfix branch (`fix/BugFix`).
3.  Commit your changes (`git commit -m 'Add some AmazingFeature'`).
4.  Push to the branch (`git push origin feature/AmazingFeature`).
5.  Open a Pull Request.

## 📞 Contact and Support

If you encounter a problem or have a question, please first check the GitHub Issues page or create a new issue.

Developed by Kerem Alagöz.

-----

## 🇹🇷 Türkçe

# Whisper Speech to Text

Whisper Speech to Text, OpenAI'ın güçlü Whisper modelini kullanarak ses dosyalarınızı doğrudan bilgisayarınızda metne dönüştürmenizi sağlayan kullanıcı dostu bir Windows masaüstü uygulamasıdır. Gizliliğinize önem verenler ve yerel çözümler arayanlar için idealdir.

## 🌟 Öne Çıkan Özellikler

  * **Yüksek Doğrulukta Transkripsiyon:** OpenAI Whisper modeli ile güçlendirilmiştir.
  * **Kullanıcı Dostu Arayüz:** C\# Windows Forms ile geliştirilmiş, basit ve anlaşılır bir arayüz sunar.
  * **Çevrimdışı Çalışma:** İnternet bağlantısı olmadan (model dosyaları ve FFmpeg indirildikten/paketlendikten sonra) transkripsiyon yapabilme.
  * **Model Seçimi:** Farklı ihtiyaçlara ve sistem kaynaklarına göre çeşitli Whisper modelleri (tiny, base, small, medium, large, large-v3, large-v3-turbo) arasından seçim yapabilme.
  * **İşlem Birimi Seçimi:** CUDA destekli NVIDIA GPU'nuzda hızlı transkripsiyon veya CPU üzerinde çalışma imkanı.
  * **Geniş Format Desteği:** Entegre FFmpeg sayesinde MP3, WAV, M4A, FLAC ve daha birçok yaygın ses formatını destekler.
  * **Kolay Kurulum:** Windows için hazırlanan `.exe` kurulum paketi ile hızlı ve basit yükleme.
  * **Bağımsız Çalışma:** Gerekli tüm bileşenler (FFmpeg, Whisper modelleri) kurulum paketine dahildir, ek bir kuruluma ihtiyaç duymazsınız.

## 🛠️ Kurulum

Uygulamayı kullanmaya başlamak için aşağıdaki adımları izleyebilirsiniz:

1.  **İndirme:**
      * En son kararlı sürümü GitHub **[şüpheli bağlantı kaldırıldı]** sayfamızdan indirin.
      * `whisper_setup.exe` adlı kurulum dosyasını bilgisayarınıza indirin.
2.  **Kurulum:**
      * İndirdiğiniz `whisper_setup.exe` dosyasını çalıştırın.
      * Kurulum sihirbazındaki adımları takip edin.
      * Uygulama varsayılan olarak `C:\Program Files (x86)\Whisper Speech to Text` (veya 64-bit için `C:\Program Files\Whisper Speech to Text`) altına kurulacaktır.
      * Kurulum sırasında masaüstü kısayolu oluşturma seçeneği sunulacaktır.

## 🚀 Kullanım Kılavuzu

1.  Uygulamayı Başlat Menüsü'ndeki "Whisper Speech to Text" grubundan veya (oluşturulduysa) masaüstü kısayolundan çalıştırın.
2.  **"Ses Dosyası Seç"** butonuna tıklayarak transkribe etmek istediğiniz ses dosyasını (.mp3, .wav, .m4a vb.) seçin.
3.  **"İşlem Birimi Seçin"** açılır menüsünden:
      * Eğer uyumlu bir NVIDIA ekran kartınız varsa ve daha hızlı işlem istiyorsanız **"cuda"** seçeneğini tercih edin.
      * Diğer durumlarda veya GPU'nuz yoksa **"cpu"** seçeneğini kullanın.
4.  **"Model Seçin"** açılır menüsünden kullanmak istediğiniz Whisper modelini seçin.
      * **Öneri:** `base` veya `small` modelleri çoğu genel kullanım için iyi bir hız/doğruluk dengesi sunar.
      * Daha büyük modeller (`medium`, `large`, `large-v3`) daha doğru sonuçlar verir ancak daha fazla sistem kaynağı (RAM, VRAM) tüketir ve işlem süresi daha uzundur.
5.  **"Transkribe Et"** butonuna tıklayın.
6.  İşlem tamamlandığında transkripsiyon metni aşağıdaki sonuç alanında görünecektir. Metni seçip kopyalayabilirsiniz.
7.  İşlem sırasında ilerleme ve olası bilgilendirme mesajları için "Loglar" (veya benzeri) bölümünü kontrol edebilirsiniz.

## ⚙️ Geliştirme ve Kaynak Kod

Bu proje açık kaynaklıdır ve katkılarınızı bekliyoruz\!

**Proje Yapısı:**

  * **`CSharpUI/`**: C\# Windows Forms kullanıcı arayüzü projesi.
  * **`PythonEngine/`**: OpenAI Whisper modelini kullanan Python transkripsiyon motoru ve ilgili dosyalar (`transcribe_audio.py`, gömülü modeller, FFmpeg).
  * **`Setup/`**: Inno Setup script dosyası (`.iss`) Windows kurulum paketini oluşturmak için.

**Gereksinimler (Python Kısmı İçin):**

  * Python 3.10.x
  * Gerekli Python kütüphaneleri için `PythonEngine/requirements.txt` dosyasına bakınız.
    ```bash
    pip install -r PythonEngine/requirements.txt
    ```
  * FFmpeg sistem PATH'inde olmalıdır.

**Gereksinimler (C\# Kısmı İçin):**

  * .NET Framework 4.7.2
  * Visual Studio 2022

**Python EXE'sini Oluşturma (PyInstaller):**

`PythonEngine` klasörü içindeki `transcribe_audio.py` script'ini `.exe`'ye dönüştürmek için kullanılan örnek PyInstaller komutu (detaylar için projedeki notlara veya build script'ine bakınız):

```bash
# ÖNEMLİ: Bu komutu PythonEngine klasörünün bir üst dizininden (proje kökünden) çalıştırın
# ve --add-data içindeki whisper/assets yolunu kendi sanal ortamınıza göre güncelleyin!
pyinstaller --name WhisperTranscriber PythonEngine/transcribe_audio.py ^
    --distpath ./PythonEngine/dist ^
    --onedir ^
    --add-binary "PythonEngine/Binaries/ffmpeg.exe:Binaries" ^
    --add-binary "PythonEngine/Binaries/ffprobe.exe:Binaries" ^
    --add-binary "PythonEngine/Binaries/tbb12.dll:Binaries" ^
    --add-data "PythonEngine/embedded_whisper_models:embedded_whisper_models" ^
    --add-data "C:/Path/To/Your/venv/Lib/site-packages/whisper/assets:whisper/assets" ^
    --hidden-import=tiktoken_ext.openai_public ^
    --hidden-import=tiktoken_ext
```

Not: Yukarıdaki PyInstaller komutundaki yolları kendi proje yapınıza ve sanal ortam yolunuza göre düzenlemeniz gerekebilir.

## 🧱 Kullanılan Teknolojiler

**Arayüz**: C\# Windows Forms, .NET Framework

**Transkripsiyon Motoru**: Python, OpenAI Whisper, PyTorch

**Ses İşleme**: FFmpeg

**Python Paketleme**: PyInstaller

**Windows Kurulum Paketi**: Inno Setup

## 📄 Lisans

Bu proje MIT Lisansı altında lisanslanmıştır. Detaylar için LICENSE dosyasına bakınız.

Kullanılan üçüncü taraf kütüphaneler ve araçlar (OpenAI Whisper, PyTorch, FFmpeg, Numba, TikToken, Inno Setup, PyInstaller vb.) kendi lisanslarına tabidir. Lütfen ilgili projelerin lisans belgelerini inceleyiniz.

## 🙌 Katkıda Bulunma

Her türlü katkıya açığız\! Hata bildirimleri, özellik istekleri veya kod katkıları için lütfen GitHub Issues veya Pull Requests bölümlerini kullanın.

1.  Projeyi Fork'layın.
2.  Yeni bir özellik dalı (`feature/AmazingFeature`) veya hata düzeltme dalı (`fix/BugFix`) oluşturun.
3.  Değişikliklerinizi Commit'leyin (`git commit -m 'Add some AmazingFeature'`).
4.  Dalınızı Push'layın (`git push origin feature/AmazingFeature`).
5.  Bir Pull Request açın.

## 📞 İletişim ve Destek

Bir sorunla karşılaşırsanız veya bir sorunuz varsa, lütfen öncelikle GitHub Issues sayfasını kontrol edin veya yeni bir issue oluşturun.

Kerem Alagöz tarafından geliştirilmiştir.
