﻿using Anotar.Log4Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Legendary_Rune_Maker.Data
{
    public class Config
    {
        public static Config Default { get; set; } = Load();

        private const string FilePath = "config.json";
        private const int LatestVersion = 12;

        public static readonly string[] AvailableLanguages = new[]
        {
            "en-US",
            "es-ES"
        };

        public int ConfigVersion { get; set; } = LatestVersion;

        public bool CheckUpdatesBeforeStartup { get; set; } = true;
        public bool LoadCacheBeforeStartup { get; set; } = true;
        public string CultureName { get; set; }
        public bool MinimizeToTaskbar { get; set; } = true;

        public bool AutoAccept { get; set; } = true;

        public bool UploadOnLock { get; set; } = true;
        public bool LoadOnLock { get; set; } = true;
        public string LockLoadProvider { get; set; }

        public bool AutoPickChampion { get; set; }
        public bool DisablePickChampion { get; set; } = true;

        public bool AutoBanChampion { get; set; }
        public bool DisableBanChampion { get; set; } = true;

        public bool AutoPickSumms { get; set; }
        public bool DisablePickSumms { get; set; } = true;

        public bool SetItemSet { get; set; } = true;
        public string ItemSetProvider { get; set; }
        public bool KeepItemSets { get; set; }
        public string LastItemSetUid { get; set; }

        public bool ShowSkillOrder { get; set; }
        public string SkillOrderProvider { get; set; }

        
        public Dictionary<Position, int> ChampionsToPick { get; set; } = new Dictionary<Position, int>
        {
            [Position.Fill] = 0,
            [Position.Top] = 0,
            [Position.Jungle] = 0,
            [Position.Mid] = 0,
            [Position.Bottom] = 0,
            [Position.Support] = 0
        };
        public Dictionary<Position, int> ChampionsToBan { get; set; } = new Dictionary<Position, int>
        {
            [Position.Fill] = 0,
            [Position.Top] = 0,
            [Position.Jungle] = 0,
            [Position.Mid] = 0,
            [Position.Bottom] = 0,
            [Position.Support] = 0
        };
        public Dictionary<Position, int[]> SpellsToPick { get; set; } = new Dictionary<Position, int[]>
        {
            [Position.Fill] = new int[2],
            [Position.Top] = new int[2],
            [Position.Jungle] = new int[2],
            [Position.Mid] = new int[2],
            [Position.Bottom] = new int[2],
            [Position.Support] = new int[2]
        };

        [JsonIgnore]
        public CultureInfo Culture => new CultureInfo(this.CultureName);

        public void Save()
        {
            LogTo.Info("Saving config");
            LogTo.Debug(() => JsonConvert.SerializeObject(this));

            File.WriteAllText(FilePath, JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static void Reload() => Default = Load();

        private static Config Load()
        {
            Config c;

            if (!File.Exists(FilePath) || MainWindow.InDesigner)
            {
                c = new Config();
            }
            else
            {
                c = JsonConvert.DeserializeObject<Config>(File.ReadAllText(FilePath));
            }

            if (c.CultureName == null)
                c.CultureName = CultureInfo.CurrentCulture.Name;

            if (c.ConfigVersion < LatestVersion)
                c.ConfigVersion = LatestVersion;

            c.Save();
            return c;
        }
    }
}
