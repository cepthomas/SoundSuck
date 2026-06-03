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


namespace SoundSuck
{
    public partial class MainForm : Form
    {
        #region Fields
        /// <summary>The settings.</summary>
        readonly UserSettings _settings;

        /// <summary>
        /// https://ourcodeworld.com/articles/read/702/how-to-record-the-audio-from-the-sound-card-system-audio-with-c-using-naudio-in-winforms
        /// TODO new NAudio3 changes this interface.
        /// </summary>
        WaveFileWriter? _audioWriter = null;
        WasapiLoopbackCapture? _loopbackCapture = null;

        /// <summary>Where you going?</summary>
        readonly string _outputDir = "";
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

            if (_settings.OutputDir != "")
            {
                // explicit
                if (Path.Exists(_settings.OutputDir))
                {
                    _outputDir = _settings.OutputDir;
                }
                else
                {
                    throw new ArgumentException($"Invalid dir: {_settings.OutputDir}");
                }
            }
            else
            {
                // default
                _outputDir = Path.Combine(Environment.CurrentDirectory, "temp");
                DirectoryInfo di = new(_outputDir);
                di.Create();
            }

            btnRecord.CheckedChanged += Record_CheckedChanged;
            //btnAbout.Click += (sender, e) => { };
            btnSettings.Click += (sender, e) => EditSettings();

            // Other UI items.
            ToolStrip.Renderer = new ToolStripCheckBoxRenderer() { SelectedColor = _settings.DrawColor };

            sldVolume.DrawColor = _settings.DrawColor;
            sldVolume.Minimum = 0.0;
            sldVolume.Maximum = 2.0;
            sldVolume.Value = 1.0;
            sldVolume.Visible = false; //TODO use volume?

            meterLevel.Minimum = 0.0;
            meterLevel.Maximum = 1.0;
            meterLevel.MeterType = MeterType.Linear;// ContinuousDots;
            meterLevel.DrawColor = _settings.DrawColor;

            Text = $"SoundSuck {MiscUtils.GetVersionString()}";
            Tell(false, Text);
        }

        /// <summary>
        /// Form is legal now.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
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
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Record_CheckedChanged(object? sender, EventArgs e)
        {
            if (btnRecord.Checked) // Start
            {
                _loopbackCapture = new WasapiLoopbackCapture();
                //_loopbackCapture.WaveFormat = new WaveFormat(44100, 16, 2);

                string snow = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_sss");
                string fn = $"{snow}.wav";
                _audioWriter = new WaveFileWriter(Path.Combine(_outputDir, fn), _loopbackCapture.WaveFormat);

                Tell(false, $"Start recording to {fn}");
                _loopbackCapture.DataAvailable += (s, a) =>
                {
                    _audioWriter.Write(a.Buffer, 0, a.BytesRecorded);

                    // Metering.
                    if (a.BytesRecorded > 0)
                    {
                        float min = float.MaxValue;
                        float max = float.MinValue;

                        float sum = 0;
                        float[] f1 = new float[a.BytesRecorded / 4];
                        for (int i = 0; i < a.BytesRecorded; i += 4)
                        {
                            var val = BitConverter.ToSingle(a.Buffer, i);
                            f1[i / 4] = val;
                            sum += val;
                            min = Math.Min(min, val);
                            max = Math.Max(max, val);
                        }
                        double avg = sum / a.BytesRecorded / 4;
                        meterLevel.AddValue(Math.Max(max, -min));
                    }
                };

                _loopbackCapture.RecordingStopped += (s, a) =>
                {
                    Tell(false, $"Finished recording to {fn}");
                    _audioWriter.Dispose();
                    _audioWriter = null;
                    _loopbackCapture.Dispose();
                    meterLevel.AddValue(0);
                };

                _loopbackCapture.StartRecording();
            }
            else // Stop
            {
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
            var changes = SettingsEditor.Edit(_settings, "User Settings", 500);
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
        /// <param name="err">What</param>
        /// <param name="s">What</param>
        void Tell(bool err, string s)
        {
            if (err) tvInfo.Append($"ERR {s}");
            else tvInfo.Append($"{s}");
        }
        #endregion
    }
}
