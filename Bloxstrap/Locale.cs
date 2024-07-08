﻿using System.Windows;

using Bloxstrap.Resources;

namespace Bloxstrap
{
    internal static class Locale
    {
        public static CultureInfo CurrentCulture { get; private set; } = CultureInfo.InvariantCulture;

        public static bool RightToLeft { get; private set; } = false;

        public static readonly Dictionary<string, string> SupportedLocales = new()
        {
            { "nil", Strings.Common_SystemDefault },
            { "en", "English" },
            { "en-US", "English (United States)" },
            { "ar-SA", "العربية" },
            { "bg-BG", "Български" },
            { "bn-BD", "বাংলা" },
            { "bs", "Босански" },
            // { "cs", "Čeština" },
            { "de-DE", "Deutsch" },
            // { "dk", "Dansk" },
            { "es-ES", "Español" },
            { "fil-PH", "Filipino" },
            { "fi-FI", "Suomi" },
            { "fr-FR", "Français" },
            { "he-IL", "עברית‎" },
            { "hr-HR", "Hrvatski" },
            { "hi-IN", "Hindi (Latin)" },
            { "hu-HU", "Magyar" },
            { "id-ID", "Bahasa Indonesia" },
            { "it-IT", "Italiano" },
            { "ja-JP", "日本語" },
            { "ko-KR", "한국어" },
            { "lt-LT", "Lietuvių" },
            { "no-NO", "Bokmål" },
            // { "nl", "Nederlands" },
            { "pl-PL", "Polski" },
            { "pt-BR", "Português (Brasil)" },
            { "ro-RO", "Română" },
            { "ru-RU", "Русский" },
            { "sv-SE", "Svenska" },
            { "th-TH", "ภาษาไทย" },
            { "tr-TR", "Türkçe" },
            { "uk-UA", "Yкраїньска" },
            { "vi-VN", "Tiếng Việt" },
            { "zh-CN", "中文 (简体)" },
            { "zh-HK", "中文 (廣東話)" },
            { "zh-TW", "中文 (繁體)" }
        };

        public static string GetIdentifierFromName(string language) => SupportedLocales.FirstOrDefault(x => x.Value == language).Key ?? "nil";

        public static List<string> GetLanguages()
        {
            var languages = new List<string>();
            
            languages.AddRange(SupportedLocales.Values.Take(3));
            languages.AddRange(SupportedLocales.Values.Where(x => !languages.Contains(x)).OrderBy(x => x));
            languages[0] = Strings.Common_SystemDefault; // set again for any locale changes

            return languages;
        }

        public static void Set(string identifier)
        {
            if (!SupportedLocales.ContainsKey(identifier))
                identifier = "nil";

            if (identifier == "nil")
            {
                CurrentCulture = Thread.CurrentThread.CurrentUICulture;
            }
            else
            {
                CurrentCulture = new CultureInfo(identifier);

                CultureInfo.DefaultThreadCurrentUICulture = CurrentCulture;
                Thread.CurrentThread.CurrentUICulture = CurrentCulture;
            }

            RightToLeft = CurrentCulture.Name.StartsWith("ar") || CurrentCulture.Name.StartsWith("he");
        }

        public static void Initialize()
        {
            Set("nil");

            // https://supportcenter.devexpress.com/ticket/details/t905790/is-there-a-way-to-set-right-to-left-mode-in-wpf-for-the-whole-application
            EventManager.RegisterClassHandler(typeof(Window), FrameworkElement.LoadedEvent, new RoutedEventHandler((sender, _) =>
            {
                var window = (Window)sender;

                if (RightToLeft)
                {
                    window.FlowDirection = FlowDirection.RightToLeft;

                    if (window.ContextMenu is not null)
                        window.ContextMenu.FlowDirection = FlowDirection.RightToLeft;
                }
                else if (CurrentCulture.Name.StartsWith("th"))
                {
                    window.FontFamily = new System.Windows.Media.FontFamily(new Uri("pack://application:,,,/Resources/Fonts/"), "./#Noto Sans Thai");
                }
            }));
        }
    }
}
