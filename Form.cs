using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestConnection_Honda
{
    public partial class Form1 : Form
    {
        private SerialPort serialPort;
        private CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            InitializeSerialPort();
            cts = new CancellationTokenSource();
        }

        private void InitializeSerialPort()
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 10400;
            serialPort.DataBits = 8;
            serialPort.Parity = Parity.None;
            serialPort.StopBits = StopBits.One;
            serialPort.Handshake = Handshake.None;
            serialPort.ReadTimeout = 1000;
            serialPort.WriteTimeout = 1000;
        }

        private void btnTunerPro_Click(object sender, EventArgs e)
        {
            string appPath = @"C:\Program Files (x86)\TunerPro RT\TunerPro.exe";

            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    DisconnectSync();
                }

                if (File.Exists(appPath))
                {
                    Process.Start(appPath);
                }
                else
                {
                    MessageBox.Show($"File tidak ditemukan: {appPath}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Gagal membuka TunerPro: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task<bool> ConnectionAsync(CancellationToken ct)
        {
            try
            {
                byte[] wakeup = { 254, 4, 114, 140 };
                byte[] init = { 114, 5, 0, 240, 153 };

                // Pastikan port tertutup sebelum membuka
                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                    await Task.Delay(100, ct);
                }

                // Buka koneksi serial
                serialPort.Open();

                // Kirim break sequence
                serialPort.BreakState = false;
                await Task.Delay(100, ct);
                serialPort.BreakState = true;
                await Task.Delay(70, ct);
                serialPort.BreakState = false;
                await Task.Delay(150, ct);

                // Kirim wakeup sequence
                serialPort.Write(wakeup, 0, wakeup.Length);
                await Task.Delay(30, ct);

                // Kirim init sequence
                serialPort.Write(init, 0, init.Length);
                await Task.Delay(30, ct);

                // Bersihkan buffer
                serialPort.DiscardOutBuffer();
                serialPort.DiscardInBuffer();

                return true;
            }
            catch (OperationCanceledException)
            {
                // Operation was cancelled, clean up
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Koneksi gagal: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private async Task<bool> DisconnectAsync(CancellationToken ct)
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                    await Task.Delay(1000, ct);
                    return true;
                }
                return false;
            }
            catch (OperationCanceledException)
            {
                // Operation was cancelled
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Disconnect error: {ex.Message}", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }


        // Versi synchronous untuk kompatibilitas
        private bool DisconnectSync()
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                    serialPort.Close();
                    Thread.Sleep(1000);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Disconnect error: {ex.Message}", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            // Gunakan async version
            await ConnectionAsync(cts.Token);
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Batalkan semua operasi yang sedang berjalan
            cts.Cancel();

            // Tunggu sebentar untuk operasi yang mungkin sedang berjalan
            await Task.Delay(500);

            // Disconnect
            await DisconnectAsync(CancellationToken.None);

            // Pastikan SerialPort dibersihkan dengan benar
            if (serialPort != null)
            {
                serialPort.Dispose();
            }

            // Dispose CancellationTokenSource
            cts.Dispose();
        }

        // Contoh method untuk membatalkan operasi dari UI
        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();

            // Buat token baru untuk operasi berikutnya
            cts.Dispose();
            cts = new CancellationTokenSource();
        }
    }
}
