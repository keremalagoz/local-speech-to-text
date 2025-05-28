using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace v1
{
    public class WhisperService
    {
        private const string WHISPER_EXE_PATH = "dist/WhisperTranscriber/WhisperTranscriber.exe";

        public class TranscribeResult
        {
            public bool Success { get; set; }
            public string ErrorMessage { get; set; }
        }

        public async Task<TranscribeResult> TranscribeAudioAsync(
            string audioFilePath,
            string device,
            string model,
            Action<string> logCallback,
            Action<string> transcriptionCallback)
        {
            try
            {
                // WhisperTranscriber.exe'nin varlığını kontrol et
                if (!File.Exists(WHISPER_EXE_PATH))
                {
                    return new TranscribeResult
                    {
                        Success = false,
                        ErrorMessage = $"WhisperTranscriber.exe bulunamadı: {Path.GetFullPath(WHISPER_EXE_PATH)}"
                    };
                }

                // Process başlatma ayarları
                var startInfo = new ProcessStartInfo
                {
                    FileName = WHISPER_EXE_PATH,
                    Arguments = $"--audio_file \"{audioFilePath}\" --device {device} --model {model}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                    WorkingDirectory = Path.GetDirectoryName(Path.GetFullPath(WHISPER_EXE_PATH))
                };

                using (var process = new Process { StartInfo = startInfo })
                {
                    // Çıktı yakalama
                    var outputBuilder = new System.Text.StringBuilder();
                    var errorBuilder = new System.Text.StringBuilder();

                    process.OutputDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            outputBuilder.AppendLine(e.Data);
                            transcriptionCallback?.Invoke(outputBuilder.ToString());
                        }
                    };

                    process.ErrorDataReceived += (sender, e) =>
                    {
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            errorBuilder.AppendLine(e.Data);
                            logCallback?.Invoke(e.Data);
                        }
                    };

                    // Process'i başlat
                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();

                    // Process'in tamamlanmasını bekle
                    await Task.Run(() => process.WaitForExit());

                    // Sonucu kontrol et
                    if (process.ExitCode == 0)
                    {
                        return new TranscribeResult { Success = true };
                    }
                    else
                    {
                        return new TranscribeResult
                        {
                            Success = false,
                            ErrorMessage = $"İşlem başarısız oldu. Çıkış kodu: {process.ExitCode}\nHata: {errorBuilder}"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new TranscribeResult
                {
                    Success = false,
                    ErrorMessage = $"Beklenmeyen bir hata oluştu: {ex.Message}"
                };
            }
        }
    }
} 