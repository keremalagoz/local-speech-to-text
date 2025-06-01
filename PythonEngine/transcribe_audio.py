import whisper
import torch
import os
import sys
import argparse
import platform
import io

# Windows'ta stdout ve stderr için UTF-8 kodlamasını ayarla
if platform.system() == 'Windows':
    # Python 3.7+ için reconfigure kullan
    if hasattr(sys.stdout, 'reconfigure'):
        sys.stdout.reconfigure(encoding='utf-8')
        sys.stderr.reconfigure(encoding='utf-8')
    else:
        # Eski Python sürümleri için
        sys.stdout = io.TextIOWrapper(sys.stdout.buffer, encoding='utf-8')
        sys.stderr = io.TextIOWrapper(sys.stderr.buffer, encoding='utf-8')

# --- FFMPEG PATH AYARI (öncekiyle aynı) ---
def set_ffmpeg_path_if_bundled():
    if getattr(sys, 'frozen', False) and hasattr(sys, '_MEIPASS'):
        ffmpeg_exe_dir = sys._MEIPASS
        if ffmpeg_exe_dir not in os.environ['PATH'].split(os.pathsep):
            os.environ['PATH'] = ffmpeg_exe_dir + os.pathsep + os.environ['PATH']
# --- FFMPEG PATH AYARI SONU ---

# Kullanıcı dostu model adlarını gerçek dosya adlarına eşleyen bir sözlük
MODEL_FILENAME_MAP = {
    "tiny": "tiny.pt",
    "base": "base.pt",
    "small": "small.pt", # Eğer small.pt gömülecekse, buraya ekleyin ve dosyayı kopyalayın
    "medium": "medium.pt",
    "large": "large-v3.pt",        # Kullanıcı 'large' seçerse bu dosya kullanılacak
    "large-turbo": "large-v3-turbo.pt" # Kullanıcı 'large-turbo' seçerse bu dosya kullanılacak
}

def get_bundled_model_path(user_selected_model_name):
    """
    Kullanıcının seçtiği model adına göre (örn: "large") gömülü model dosyasının tam yolunu döndürür.
    """
    actual_filename = MODEL_FILENAME_MAP.get(user_selected_model_name)
    if not actual_filename:
        print(f"UYARI: '{user_selected_model_name}' için tanımlı bir model dosyası bulunamadı. Model adı doğrudan kullanılacak.", file=sys.stderr)
        return user_selected_model_name # Fallback, Whisper kendi bulmayı denesin

    if getattr(sys, 'frozen', False) and hasattr(sys, '_MEIPASS'):
        base_path = sys._MEIPASS
        potential_path = os.path.join(base_path, "embedded_whisper_models", actual_filename)
        if os.path.exists(potential_path):
            return potential_path
        else:
            print(f"UYARI: Gömülü model dosyası {potential_path} (beklenen ad: {actual_filename}) bulunamadı. '{user_selected_model_name}' adına göre yükleme denenecek.", file=sys.stderr)
            return user_selected_model_name
    else:
        # Geliştirme modu
        script_dir = os.path.dirname(os.path.abspath(__file__))
        local_dev_model_path = os.path.join(script_dir, "embedded_whisper_models", actual_filename)
        if os.path.exists(local_dev_model_path):
            return local_dev_model_path
        return user_selected_model_name


def transcribe_audio(audio_file_path, device_preference, user_selected_model_name="base"):
    set_ffmpeg_path_if_bundled()

    actual_device = device_preference
    if device_preference == "cuda":
        if not torch.cuda.is_available():
            print("UYARI: CUDA istendi ancak bulunamadı. CPU'ya geçiliyor.", file=sys.stderr)
            actual_device = "cpu"
        else:
            print("CUDA kullanılacak.", file=sys.stderr)
    else:
        print("CPU kullanılacak.", file=sys.stderr)

    # Kullanıcının seçtiği model adına göre .pt dosyasının yolunu al
    model_path_or_name = get_bundled_model_path(user_selected_model_name)
    
    # Yüklenecek modelin adını (veya yolunu) loglayalım
    print(f"'{user_selected_model_name}' (dosya: {os.path.basename(model_path_or_name) if os.path.exists(model_path_or_name) else model_path_or_name}) modeli yükleniyor ({actual_device} üzerinde)...", file=sys.stderr)

    try:
        model = whisper.load_model(model_path_or_name, device=actual_device)
    except Exception as e:
        print(f"HATA: Model yüklenirken bir hata oluştu: {e}", file=sys.stderr)
        # Eğer bir dosya yolu deneniyorsa ve bu yol user_selected_model_name'den farklıysa (yani map'ten geldiyse)
        # ve o dosya yolu gerçekten var olmayan bir dosyayı işaret ediyorsa (get_bundled_model_path'tan model adı döndüyse)
        expected_filename = MODEL_FILENAME_MAP.get(user_selected_model_name)
        if expected_filename and model_path_or_name == user_selected_model_name: # Yani dosya bulunamadı ve fallback çalıştı
             print(f"Lütfen '{expected_filename}' dosyasının 'embedded_whisper_models' klasöründe olduğundan ve PyInstaller ile doğru paketlendiğinden emin olun.", file=sys.stderr)
        elif expected_filename and not os.path.exists(model_path_or_name) : # Dosya yolu döndü ama var değil
             print(f"Beklenen model dosyası '{model_path_or_name}' bulunamadı.", file=sys.stderr)

        return None

    if not os.path.exists(audio_file_path):
        print(f"HATA: Ses dosyası bulunamadı: '{audio_file_path}'", file=sys.stderr)
        return None
    if not os.path.isfile(audio_file_path):
        print(f"HATA: '{audio_file_path}' bir dosya değil.", file=sys.stderr)
        return None

    print(f"'{audio_file_path}' dosyası transkribe ediliyor...", file=sys.stderr)
    try:
        use_fp16 = (actual_device == "cuda")
        result = model.transcribe(audio_file_path, fp16=use_fp16)
        print(result["text"])
        return result["text"]
    except Exception as e:
        print(f"HATA: Transkripsiyon sırasında bir hata oluştu: {e}", file=sys.stderr)
        return None

def main():
    parser = argparse.ArgumentParser(description="OpenAI Whisper ile Ses Transkripsiyonu.")
    parser.add_argument("--audio_file", type=str, required=True, help="Transkribe edilecek ses dosyası.")
    parser.add_argument("--device", type=str, choices=["cuda", "cpu"], required=True, help="İşlem birimi.")
    
    # Kullanıcının seçeceği model adları (C# tarafında gösterilecek)
    # Bunlar MODEL_FILENAME_MAP sözlüğündeki anahtarlar olmalı
    available_models = list(MODEL_FILENAME_MAP.keys())
    
    parser.add_argument("--model", type=str, default="base",
                        choices=available_models,
                        help=f"Kullanılacak Whisper modeli. Seçenekler: {', '.join(available_models)}. Varsayılan: base")
    args = parser.parse_args()
    
    # args.model doğrudan transcribe_audio fonksiyonuna gönderilir.
    # Fonksiyon içindeki get_bundled_model_path, bu adı doğru .pt dosyasına çevirecektir.
    transcribe_audio(args.audio_file, args.device, args.model)

if __name__ == "__main__":
    main()