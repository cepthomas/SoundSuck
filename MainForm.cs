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

        //https://ourcodeworld.com/articles/read/702/how-to-record-the-audio-from-the-sound-card-system-audio-with-c-using-naudio-in-winforms
        // Create class-level accessible variables to store the audio recorder and capturer instance
        WaveFileWriter? _audioWriter = null;

        WasapiLoopbackCapture? _loopbackCapture = null;

        string _outputDir = null;
        //string outputFile = @"C:\Dev\Apps\WinFormsApp1\temp.wav";
        #endregion

        #region Types
        ///// <summary>What are we doing.</summary>
        //public enum AppState { Stop, Play, Rewind, Complete, Dead }
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
            //KeyPreview = true; // for routing kbd strokes through OnKeyDown first.

            //// Create output.
            //_player = new(_settings.WavOutDevice, int.Parse(_settings.Latency), _waveOutSwapper);
            //_player.PlaybackStopped += Player_PlaybackStopped;

            if (_settings.OutputDir != "")
            {
                // explicit
                if (Path.Exists(_settings.OutputDir))
                {
                    _outputDir = _settings.OutputDir;
                }
                else
                {
                    Tell(true, $"Invalid dir: {_settings.OutputDir}");
                }
            }
            else
            {
                // default
                _outputDir = Path.Combine(Environment.CurrentDirectory, "temp");
                DirectoryInfo di = new(_outputDir);
                di.Create();
            }

            var ppp = MakeFilePath();

            // Other UI items.
            ToolStrip.Renderer = new ToolStripCheckBoxRenderer() { SelectedColor = _settings.DrawColor };

            sldVolume.DrawColor = _settings.DrawColor;
            sldVolume.Value = _settings.Volume;
            //sldVolume.ValueChanged += (_, __) => _player.Volume = (float)sldVolume.Value;

            Text = $"SoundSuck {MiscUtils.GetVersionString()}";
        }

        /// <summary>
        /// Form is legal now. Init things that want to log.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //if (!_player.Valid)
            //{
            //    var s = $"Something wrong with your audio output device:{_settings.WavOutDevice}";
            //    _logger.Error(s);
            //    UpdateState(AppState.Dead);
            //}

            base.OnLoad(e);
        }

        /// <summary>
        /// Bye-bye.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            LogManager.Stop();
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

            //// My stuff here.
            //_player.Run(false);
            //_player.Dispose();

            base.Dispose(disposing);
        }
        #endregion

        string MakeFilePath()
        {
            //string snow = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_sss");
            Guid guid = Guid.NewGuid();

            var bytes = guid.ToByteArray();

            return Path.Combine(_outputDir, $"booga.wav");
            //string? newfn = null;
            //if (_outputDir is not null)
            //{
            //    newfn = $"_{snow}.wav";
            //}

            //return newfn;
        }


        #region Do the work
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Record_CheckedChanged(object sender, EventArgs e)
        {
            if (btnRecord.Checked) // Start
            {
                // Redefine the capturer instance with a new instance of the LoopbackCapture class
                _loopbackCapture = new WasapiLoopbackCapture();

                // Redefine the audio writer instance with the given configuration
                _audioWriter = new WaveFileWriter("outputFile", _loopbackCapture.WaveFormat);

                // When the capturer receives audio, start writing the buffer into the mentioned file
                _loopbackCapture.DataAvailable += (s, a) =>
                {
                    _audioWriter.Write(a.Buffer, 0, a.BytesRecorded);
                };

                // When the Capturer Stops
                _loopbackCapture.RecordingStopped += (s, a) =>
                {
                    _audioWriter.Dispose();
                    _audioWriter = null;
                    _loopbackCapture.Dispose();
                };

                // Start recording.
                _loopbackCapture.StartRecording();
            }
            else // Stop
            {
                _loopbackCapture?.StopRecording();
            }
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //void OnStartRecordingClick_2()
        //{
        //    // Redefine the capturer instance with a new instance of the LoopbackCapture class
        //    _captureInstance = new WasapiLoopbackCapture();

        //    // Redefine the audio writer instance with the given configuration
        //    _recordedAudioWriter = new WaveFileWriter(outputFile, _captureInstance.WaveFormat);

        //    // When the capturer receives audio, start writing the buffer into the mentioned file
        //    _captureInstance.DataAvailable += (s, a) =>
        //    {
        //        _recordedAudioWriter.Write(a.Buffer, 0, a.BytesRecorded);
        //    };

        //    // When the Capturer Stops
        //    _captureInstance.RecordingStopped += (s, a) =>
        //    {
        //        _recordedAudioWriter.Dispose();
        //        _recordedAudioWriter = null;
        //        _captureInstance.Dispose();
        //    };

        //    // Start recording.
        //    _captureInstance.StartRecording();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        //void OnStopRecordingClick_2()
        //{
        //    _captureInstance.StopRecording();
        //}
        #endregion



        #region Settings
        /// <summary>
        /// Edit user settings.
        /// </summary>
        void EditSettings()
        {
            var changes = SettingsEditor.Edit(_settings, "User Settings", 500);

            // Detect changes of interest.
            bool restart = false;

            foreach (var (name, cat) in changes)
            {
                switch(name)
                {
                    case "WavOutDevice":
                    case "TODO1 these":
                    case "DrawColor":
                    case "WaveColor":
                    case "FileLogLevel":
                    case "NotifLogLevel":
                        restart = true;
                        break;
                    case "SplitterPosition":
                    case "SingleClickSelect":
                        break;
                }
            }

            if (restart)
            {
                MessageBox.Show("Restart required for device changes to take effect");
            }

            SaveSettings();
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

        ///// <summary>Tell me something good.</summary>
        ///// <param name="s">What</param>
        ///// <param name="file"></param>
        ///// <param name="line"></param>
        //void Tell(string s, [CallerFilePath] string file = "", [CallerLineNumber] int line = -1)
        //{
        //    var fn = Path.GetFileName(file);
        //    tvInfo.Append($"{fn}({line}) {s}");
        //}
        #endregion
    }
}
