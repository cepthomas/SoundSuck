using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text.Json.Serialization;
using System.Drawing.Design;
using Ephemera.NBagOfTricks;
using Ephemera.NBagOfUis;


namespace SoundSuck
{
    [Serializable]
    public sealed class UserSettings : SettingsCore
    {
        #region Persisted Editable Properties
        [DisplayName("Control Color")]
        [Description("The color used for control surfaces.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonColorConverter))]
        public Color DrawColor { get; set; } = Color.MediumOrchid;

        [DisplayName("Output Directory")]
        [Description("Where to put captured audio files. Can be empty.")]
        [Browsable(true)]
        public string OutputDir { get; set; } = "";

        [DisplayName("Save As MP3")]
        [Description("Save as mp3 otherwise wav (default).")]
        [Browsable(true)]
        public bool SaveAsMp3 { get; set; } = false;
        #endregion

        #region Persisted Non-editable Persisted Properties
        [Browsable(false)]
        public double Gain { get; set; } = 2.0;
        #endregion
    }
}
