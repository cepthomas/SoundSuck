using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfUis;
using NAudio.Lame;


namespace SoundSuck
{
    public partial class MainForm : Form
    {
        #region Fields
        /// <summary>The settings.</summary>
        readonly UserSettings _settings;

        /// <summary>Stream writer.</summary>
        Stream? _audioWriter = null;

        /// <summary>Audio input.</summary>
        WasapiLoopbackCapture? _loopbackCapture = null;

        /// <summary>Where you going?</summary>
        readonly string _tempDir = "";
        #endregion

        #region Lifecycle
        public MainForm()
        {
            // Must do this first before initializing.
            string appDir = MiscUtils.GetAppDataDir("SoundSuck", "Ephemera");
            _settings = (UserSettings)SettingsCore.Load(appDir, typeof(UserSettings));

            InitializeComponent();
            Icon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);

            // Info display.
            tvInfo.Font = Font;
            tvInfo.Matchers =
            [
                new("ERR", Color.Red),
                new("WRN", Color.Green),
            ];

            // Init main form from settings.
            WindowState = FormWindowState.Normal;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(_settings.FormGeometry.X, _settings.FormGeometry.Y);

            // Ensure temp exists.
            _tempDir = Path.Combine(Environment.CurrentDirectory, "temp");
            DirectoryInfo di = new(_tempDir);
            di.Create();
            di.GetFiles().ForEach(fi => { File.Delete(fi.FullName); });

            btnRecord.CheckedChanged += Record_CheckedChanged;
            btnSettings.Click += (sender, e) => EditSettings();

            // Other UI items.
            ToolStrip.Renderer = new ToolStripCheckBoxRenderer() { SelectedColor = _settings.DrawColor };

            meterLevel.Minimum = 0.0;
            meterLevel.Maximum = 1.0;
            meterLevel.MeterType = MeterType.Linear;// ContinuousDots;
            meterLevel.DrawColor = _settings.DrawColor;

            //TODO use volume?
            sldVolume.Visible = false;
            sldVolume.DrawColor = _settings.DrawColor;
            sldVolume.Minimum = 0.0;
            sldVolume.Maximum = 2.0;
            sldVolume.Value = 1.0;

            Text = $"SoundSuck {MiscUtils.GetVersionString()}";
            Tell(Text);

            DevStuff();
        }

        /// <summary>
        /// Bye-bye.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            SaveSettings();
            base.OnFormClosing(e);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            // My stuff here.
            _loopbackCapture?.StopRecording();
            _loopbackCapture?.Dispose();

            base.Dispose(disposing);
        }
        #endregion

        #region Do the work
        /// <summary>
        /// https://ourcodeworld.com/articles/read/702/how-to-record-the-audio-from-the-sound-card-system-audio-with-c-using-naudio-in-winforms
        /// TODO new NAudio3 changes this interface!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Record_CheckedChanged(object? sender, EventArgs e)
        {
            if (btnRecord.Checked) // Start
            {
                var ext = _settings.SaveAsMp3 ? "mp3" : "wav";
                string snow = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_sss");
                var fname = $"{snow}.{ext}";
                var fpath = Path.Combine(_tempDir, fname);

                _loopbackCapture = new WasapiLoopbackCapture();
                //_loopbackCapture.WaveFormat = new WaveFormat(44100, 16, 2);
                _audioWriter = _settings.SaveAsMp3 ?
                    new WaveFileWriter(fpath, _loopbackCapture.WaveFormat) :
                    new LameMP3FileWriter(fpath, _loopbackCapture.WaveFormat, 128);

                Tell($"Start recording to {fname}");
                _loopbackCapture.DataAvailable += (sender, args) =>
                {
                    int num = args.BytesRecorded;
                    _audioWriter.Write(args.Buffer, 0, num);

                    // Metering - abs max.
                    float max = float.MinValue;
                    for (int i = 0; i < num; i += 4)
                    {
                        var val = BitConverter.ToSingle(args.Buffer, i);
                        max = Math.Max(max, Math.Abs(val));
                    }
                    meterLevel.AddValue(max);
                };

                _loopbackCapture.RecordingStopped += (sender, args) =>
                {
                    Tell($"Finished recording to {fname}");

                    // Clean up resources.
                    _audioWriter.Dispose();
                    _audioWriter = null;
                    _loopbackCapture.Dispose();
                    _loopbackCapture = null;
                    meterLevel.AddValue(0);

                    // Get user filename and rename temp.
                    using SaveFileDialog saveDlg = new()
                    {
                        Title = "Save as file",
                        Filter = _settings.SaveAsMp3 ? "(*.mp3)|*.mp3" : "(*.wav)|*.wav)",
                        FileName = fname,
                        InitialDirectory = _settings.OutputDir
                    };

                    if (saveDlg.ShowDialog() == DialogResult.OK)
                    {
                        File.Move(fpath, saveDlg.FileName);
                        Tell($"File saved to {saveDlg.FileName}");
                    }
                };

                _loopbackCapture.StartRecording();
            }
            else // Stop
            {
                // Signal RecordingStopped callback.
                _loopbackCapture?.StopRecording();
            }
        }
        #endregion

        #region Settings
        /// <summary>
        /// Edit user settings.
        /// </summary>
        void EditSettings()
        {
            var changes = SettingsEditor.Edit(_settings, "User Settings", 200);
            if (changes.Count > 0)
            {
                MessageBox.Show("Restart required for device changes to take effect");
                SaveSettings();
            }
        }

        /// <summary>
        /// Collect and save user settings.
        /// </summary>
        void SaveSettings()
        {
            _settings.FormGeometry = new Rectangle(Location.X, Location.Y, Width, Height);
            _settings.Save();
        }
        #endregion

        #region Internals
        /// <summary>Tell me something good.</summary>
        /// <param name="s">What</param>
        void Tell(string s)
        {
            tvInfo.Append($"{s}");
        }

        /// <summary>
        /// Debugging.
        /// </summary>
        void DevStuff()
        {
            List<WaveInCapabilities> recin = [];
            // –1 indicates the default input device.
            for (int id = -1; id < WaveIn.DeviceCount; id++)
            {
                var cap = WaveIn.GetCapabilities(id);
                recin.Add(cap);

                Tell($"IN: {id} {cap.ProductName}");
            }

            List<WaveOutCapabilities> recout = [];
            // –1 indicates the default output device.
            for (int id = -1; id < WaveOut.DeviceCount; id++)
            {
                var cap = WaveOut.GetCapabilities(id);
                recout.Add(cap);

                Tell($"OUT: {id} {cap.ProductName}");
            }

            //IN: -1 Microsoft Sound Mapper
            //IN: 0 Microphone(Realtek(R) Audio) <<
            //IN: 1 Microphone Array(Intel® Smart
            //OUT: -1 Microsoft Sound Mapper
            //OUT: 0 Headphones(Realtek(R) Audio) <<
            //OUT: 1 Speakers(Realtek(R) Audio)
        }
        #endregion
    }
}
