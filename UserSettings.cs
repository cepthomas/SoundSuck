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
        [Description("The color used for active control surfaces.")]
        [Browsable(true)]
        [JsonConverter(typeof(JsonColorConverter))]
        public Color DrawColor { get; set; } = Color.MediumOrchid;

        [DisplayName("Output Directory")]
        [Description("Where to put captured audio files.")]
        [Browsable(true)]
        public string OutputDir { get; set; } = "";
        #endregion

        #region Persisted Non-editable Persisted Properties
        [Browsable(false)]
        public double Volume { get; set; } = 0.8; // AudioLibDefs.MAX_VOLUME / 2;
        #endregion
    }
}
