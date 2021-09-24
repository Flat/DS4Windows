﻿using System;
using System.Xml.Serialization;
using DS4Windows;
using DS4WinWPF.DS4Control.Profiles.Legacy.Converters;

namespace DS4WinWPF.DS4Control.Profiles.Legacy
{
    [XmlRoot(ElementName = "Profile")]
    public partial class DS4WindowsAppSettings
    {
        [XmlElement(ElementName = "useExclusiveMode")]
        public bool UseExclusiveMode { get; set; }

        [XmlElement(ElementName = "startMinimized")]
        public bool StartMinimized { get; set; }

        [XmlElement(ElementName = "minimizeToTaskbar")]
        public bool MinimizeToTaskbar { get; set; }

        [XmlElement(ElementName = "formWidth")]
        public int FormWidth { get; set; }

        [XmlElement(ElementName = "formHeight")]
        public int FormHeight { get; set; }

        [XmlElement(ElementName = "formLocationX")]
        public int FormLocationX { get; set; }

        [XmlElement(ElementName = "formLocationY")]
        public int FormLocationY { get; set; }

        [XmlElement(ElementName = "LastChecked")]
        public DateTime LastChecked { get; set; }

        [XmlElement(ElementName = "CheckWhen")]
        public int CheckWhen { get; set; }

        [XmlElement(ElementName = "LastVersionChecked")]
        public string LastVersionChecked { get; set; }

        [XmlElement(ElementName = "Notifications")]
        public int Notifications { get; set; }

        [XmlElement(ElementName = "DisconnectBTAtStop")]
        public bool DisconnectBTAtStop { get; set; }

        [XmlElement(ElementName = "SwipeProfiles")]
        public bool SwipeProfiles { get; set; }

        [XmlElement(ElementName = "QuickCharge")]
        public bool QuickCharge { get; set; }

        [XmlElement(ElementName = "CloseMinimizes")]
        public bool CloseMinimizes { get; set; }

        [XmlElement(ElementName = "UseLang")]
        public string UseLang { get; set; }

        [XmlElement(ElementName = "DownloadLang")]
        public bool DownloadLang { get; set; }

        [XmlElement(ElementName = "FlashWhenLate")]
        public bool FlashWhenLate { get; set; }

        [XmlElement(ElementName = "FlashWhenLateAt")]
        public int FlashWhenLateAt { get; set; }

        [XmlElement(ElementName = "AppIcon")] 
        public TrayIconChoice AppIcon { get; set; } = TrayIconChoice.Default;

        [XmlElement(ElementName = "AppTheme")] 
        public AppThemeChoice AppTheme { get; set; } = AppThemeChoice.Default;

        [XmlElement(ElementName = "UseUDPServer")]
        public bool UseUDPServer { get; set; }

        [XmlElement(ElementName = "UDPServerPort")]
        public int UDPServerPort { get; set; }

        [XmlElement(ElementName = "UDPServerListenAddress")]
        public string UDPServerListenAddress { get; set; }

        [XmlElement(ElementName = "UDPServerSmoothingOptions")]
        public UDPServerSmoothingOptions UDPServerSmoothingOptions { get; set; } = new();

        [XmlElement(ElementName = "UseCustomSteamFolder")]
        public bool UseCustomSteamFolder { get; set; }

        [XmlElement(ElementName = "CustomSteamFolder")]
        public string CustomSteamFolder { get; set; }

        [XmlElement(ElementName = "AutoProfileRevertDefaultProfile")]
        public bool AutoProfileRevertDefaultProfile { get; set; }

        [XmlElement(ElementName = "DeviceOptions")]
        public DeviceOptions DeviceOptions { get; set; } = new();

        [XmlElement(ElementName = "CustomLed1")]
        public CustomLedProxyType CustomLed1 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed2")]
        public CustomLedProxyType CustomLed2 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed3")]
        public CustomLedProxyType CustomLed3 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed4")]
        public CustomLedProxyType CustomLed4 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed5")]
        public CustomLedProxyType CustomLed5 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed6")]
        public CustomLedProxyType CustomLed6 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed7")]
        public CustomLedProxyType CustomLed7 { get; set; } = new();

        [XmlElement(ElementName = "CustomLed8")]
        public CustomLedProxyType CustomLed8 { get; set; } = new();

        [XmlAttribute(AttributeName = "app_version")]
        public string AppVersion { get; set; }

        [XmlAttribute(AttributeName = "config_version")]
        public int ConfigVersion { get; set; }

        public DS4WindowsAppSettings()
        {
        }

        public DS4WindowsAppSettings(IBackingStore store, string appVersion, int configVersion)
        {
            AppVersion = appVersion;
            ConfigVersion = configVersion;

            CopyFrom(store);
        }
    }
}
