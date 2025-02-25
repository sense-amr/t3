﻿using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using T3.Core.Audio;
using T3.Core.Resource;
using T3.Core.Utils;
using T3.Serialization;

namespace T3.Core.Operator
{
    /// <summary>
    /// Defines playback related settings for a symbol like its primary soundtrack or audio input device,
    /// BPM rate and other settings.
    /// </summary>
    public class PlaybackSettings
    {
        public bool Enabled { get; set; }
        public float Bpm  = 120;
        public List<AudioClip> AudioClips { get; private set; } = new();
        public AudioSources AudioSource;
        public SyncModes Syncing;
        
        public string AudioInputDeviceName = string.Empty;
        public float AudioGainFactor = 1;
        public float AudioDecayFactor = 0.9f;

        public bool GetMainSoundtrack(out AudioClip soundtrack)
        {
            foreach (var clip in AudioClips)
            {
                if (!clip.IsSoundtrack)
                    continue;

                soundtrack = clip;
                return true;
            }

            soundtrack = null;
            return false;
        }

        public enum AudioSources
        {
            ProjectSoundTrack,
            ExternalDevice,
        }
        
        public enum SyncModes
        {
            Timeline,
            Tapping,
        }


        public void WriteToJson(JsonTextWriter writer)
        {
            var hasSettingsForClips = Enabled || AudioClips.Count > 0;
            if (!hasSettingsForClips)
                return;

            writer.WritePropertyName(nameof(PlaybackSettings));

            writer.WriteStartObject();
            {
                //writer.WriteEndArray();

                JsonExtensions.WriteValue(writer, nameof(Enabled), Enabled);
                JsonExtensions.WriteValue(writer, nameof(Bpm), Bpm);
                JsonExtensions.WriteValue(writer, nameof(AudioSource), AudioSource);
                JsonExtensions.WriteValue(writer, nameof(Syncing), Syncing);
                JsonExtensions.WriteValue(writer, nameof(AudioDecayFactor), AudioDecayFactor);
                JsonExtensions.WriteValue(writer, nameof(AudioGainFactor), AudioGainFactor);
                JsonExtensions.WriteObject(writer, nameof(AudioInputDeviceName), AudioInputDeviceName);

                // Write audio clips
                var audioClips = AudioClips;
                if (audioClips != null && audioClips.Count != 0)
                {
                    writer.WritePropertyName("AudioClips");
                    writer.WriteStartArray();
                    foreach (var audioClip in audioClips)
                    {
                        audioClip.ToJson(writer);
                    }
                    writer.WriteEndArray();
                }
            }

            writer.WriteEndObject();
        }

        public static PlaybackSettings ReadFromJson(JToken o)
        {
            var clips = GetClips(o).ToList(); // Support legacy json format

            var settingsToken = (JObject)o[nameof(Symbol.PlaybackSettings)];
            if (settingsToken == null && clips.Count == 0)
                return null;

            var newSettings = new PlaybackSettings
                                  {
                                      AudioClips = clips
                                  };
            
            if (settingsToken != null)
            {
                newSettings.Enabled = JsonUtils.ReadToken(settingsToken, nameof(Enabled),false);
                newSettings.Bpm = JsonUtils.ReadToken(settingsToken, nameof(Bpm), 120f);
                newSettings.AudioSource = JsonUtils.ReadEnum<AudioSources>(settingsToken, nameof(AudioSource));
                newSettings.Syncing = JsonUtils.ReadEnum<SyncModes>(settingsToken, nameof(Syncing));
                newSettings.AudioDecayFactor = JsonUtils.ReadToken(settingsToken, nameof(AudioDecayFactor),0.5f);
                newSettings.AudioGainFactor = JsonUtils.ReadToken(settingsToken, nameof(AudioGainFactor), 1f);
                newSettings.AudioInputDeviceName = JsonUtils.ReadToken<string>(settingsToken, nameof(AudioInputDeviceName), null);
                
                newSettings.AudioClips.AddRange(GetClips(settingsToken)); // Support correct format
            }
            
            if (newSettings.Bpm == 0 && newSettings.GetMainSoundtrack(out var soundtrack))
            {
                newSettings.Bpm = soundtrack.Bpm;
                newSettings.Enabled = true;
            }

            return newSettings;
        }

        private static IEnumerable<AudioClip> GetClips(JToken o)
        {
            var jAudioClipArray = (JArray)o[nameof(Symbol.PlaybackSettings.AudioClips)];
            if (jAudioClipArray != null)
            {
                foreach (var c in jAudioClipArray)
                {
                    yield return AudioClip.FromJson(c);
                }
            }
        }
    }
}