using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using v1; // WhisperService sınıfının namespace'ini ekle

namespace v1
{
    public partial class Form1 : Form
    {
        private Button btnSelectAudio;
        private TextBox txtAudioPath;
        private ComboBox cmbDevice;
        private ComboBox cmbModel;
        private Button btnTranscribe;
        private TextBox txtTranscription;
        private TextBox txtLogs;
        private Label lblStatus;
        private ProgressBar progressBar;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Form ayarları
            this.Text = "Whisper Transkripsiyon Uygulaması";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Ses Dosyası Seç butonu
            btnSelectAudio = new Button
            {
                Text = "Ses Dosyası Seç",
                Location = new Point(20, 20),
                Size = new Size(120, 30)
            };
            btnSelectAudio.Click += BtnSelectAudio_Click;

            // Ses dosyası yolu TextBox
            txtAudioPath = new TextBox
            {
                Location = new Point(150, 20),
                Size = new Size(500, 30),
                ReadOnly = true
            };

            // İşlem Birimi ComboBox
            var lblDevice = new Label
            {
                Text = "İşlem Birimi Seçin:",
                Location = new Point(20, 70),
                Size = new Size(120, 20)
            };

            cmbDevice = new ComboBox
            {
                Location = new Point(150, 70),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbDevice.Items.AddRange(new string[] { "cpu", "cuda" });
            cmbDevice.SelectedIndex = 0;

            // Model ComboBox
            var lblModel = new Label
            {
                Text = "Model Seçin:",
                Location = new Point(20, 110),
                Size = new Size(120, 20)
            };

            cmbModel = new ComboBox
            {
                Location = new Point(150, 110),
                Size = new Size(200, 30),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbModel.Items.AddRange(new string[] { "tiny", "base", "small", "medium", "large", "large-turbo" });
            cmbModel.SelectedIndex = 1;

            // Transkribe Et butonu
            btnTranscribe = new Button
            {
                Text = "Transkribe Et",
                Location = new Point(20, 150),
                Size = new Size(120, 30)
            };
            btnTranscribe.Click += BtnTranscribe_Click;

            // Transkripsiyon sonucu TextBox
            var lblTranscription = new Label
            {
                Text = "Transkripsiyon Sonucu:",
                Location = new Point(20, 190),
                Size = new Size(150, 20)
            };

            txtTranscription = new TextBox
            {
                Location = new Point(20, 220),
                Size = new Size(740, 150),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            // Log TextBox
            var lblLogs = new Label
            {
                Text = "İşlem Logları:",
                Location = new Point(20, 380),
                Size = new Size(150, 20)
            };

            txtLogs = new TextBox
            {
                Location = new Point(20, 410),
                Size = new Size(740, 100),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true
            };

            // Durum etiketi
            lblStatus = new Label
            {
                Text = "Hazır",
                Location = new Point(150, 155),
                Size = new Size(200, 20)
            };

            // Progress Bar
            progressBar = new ProgressBar
            {
                Location = new Point(20, 520),
                Size = new Size(740, 20),
                Style = ProgressBarStyle.Marquee,
                Visible = false
            };

            // Kontrolleri forma ekle
            this.Controls.AddRange(new Control[] {
                btnSelectAudio, txtAudioPath,
                lblDevice, cmbDevice,
                lblModel, cmbModel,
                btnTranscribe, lblStatus,
                lblTranscription, txtTranscription,
                lblLogs, txtLogs,
                progressBar
            });
        }

        private void BtnSelectAudio_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Ses Dosyaları|*.mp3;*.wav;*.m4a;*.ogg|Tüm Dosyalar|*.*";
                openFileDialog.Title = "Ses Dosyası Seç";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtAudioPath.Text = openFileDialog.FileName;
                }
            }
        }

        private async void BtnTranscribe_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAudioPath.Text))
            {
                MessageBox.Show("Lütfen bir ses dosyası seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnTranscribe.Enabled = false;
            progressBar.Visible = true;
            lblStatus.Text = "İşleniyor...";
            txtTranscription.Clear();
            txtLogs.Clear();

            try
            {
                var whisperService = new WhisperService();
                var result = await whisperService.TranscribeAudioAsync(
                    txtAudioPath.Text,
                    cmbDevice.SelectedItem.ToString(),
                    cmbModel.SelectedItem.ToString(),
                    (log) => AppendLog(log),
                    (transcription) => UpdateTranscription(transcription)
                );

                if (result.Success)
                {
                    lblStatus.Text = "Tamamlandı";
                }
                else
                {
                    lblStatus.Text = "Hata oluştu";
                    MessageBox.Show(result.ErrorMessage, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Hata oluştu";
                MessageBox.Show($"Beklenmeyen bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnTranscribe.Enabled = true;
                progressBar.Visible = false;
            }
        }

        private void AppendLog(string log)
        {
            if (txtLogs.InvokeRequired)
            {
                txtLogs.Invoke(new Action<string>(AppendLog), log);
                return;
            }

            txtLogs.AppendText(log + Environment.NewLine);
            txtLogs.ScrollToCaret();
        }

        private void UpdateTranscription(string transcription)
        {
            if (txtTranscription.InvokeRequired)
            {
                txtTranscription.Invoke(new Action<string>(UpdateTranscription), transcription);
                return;
            }

            txtTranscription.Text = transcription;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
