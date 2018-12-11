using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace RCd.AppCore.Ui.Desktop.Theming
{

    public class SkinManager
    {

        #region Constructors

        private SkinManager()
        {
            _skins = new Dictionary<string, Uri>();
        }

        private static SkinManager _current;
        public static SkinManager Current => _current ?? (_current = new SkinManager());

        #endregion

        #region Member

        public const string DEFAULT_SKIN = "DefaultSkin";

        //public const string DEFAULT_SKIN_SOURCE = "/RCd.AppCore.Ui.Desktop.Controls;component/Skins/DefaultSkin.xaml";

        public const string DEFAULT_SKIN_SOURCE = "/RCd.AppCore.Ui.Desktop.Controls;component/Resources/Brushes.xaml";

        private readonly Dictionary<string, Uri> _skins;

        private Application _app;

        #endregion

        #region Properties

        public string CurrentSkin { get; private set; }

        #endregion

        #region Methods

        public void Initialize(Application app)
        {
            _app = app;

            AddSkin(DEFAULT_SKIN, new Uri(DEFAULT_SKIN_SOURCE, UriKind.RelativeOrAbsolute));

            AddSkinDictionary(_skins[DEFAULT_SKIN], null, null, null);
        }

        public void AddSkin(string skinName, Uri skinSource)
        {
            _skins[skinName] = skinSource;
        }

        public bool SetSkin(string skinName)
        {
            return SetSkin(skinName, null, null);
        }

        public bool SetResource(object resourceKey, object resource)
        {
            if (string.IsNullOrEmpty(CurrentSkin)) return false;

            return SetSkin(CurrentSkin, resourceKey, resource);
        }

        private bool SetSkin(string skinName, object resourceKey, object resource)
        {
            if (!_skins.ContainsKey(skinName)) return false;
            if (!FindSkinSourceByName(skinName, out var skinSource)) return false;

            RemoveSkinDictionaries(out var currentSkinResources);
            AddSkinDictionary(skinSource, currentSkinResources, resourceKey, resource);

            CurrentSkin = skinName;
            return true;
        }

        internal bool FindSkinSourceByName(string skinName, out Uri skinSource)
        {
            if (!_skins.ContainsKey(skinName))
            {
                skinSource = null;
                return false;
            }
            skinSource = _skins[skinName];
            return true;
        }

        private void RemoveSkinDictionaries(out ResourceDictionary skinResources)
        {
            var resources = _app.Resources.MergedDictionaries
                .Where(d => _skins.Values.Contains(d.Source))
                .ToList();

            skinResources = resources.LastOrDefault();

            resources.ForEach(r => _app.Resources.MergedDictionaries.Remove(r));
        }

        public void AddSkinDictionary(Uri skinSource, ResourceDictionary currentSkinResources, object resourceKey, object resource)
        {
            var dictionary = new ResourceDictionary { Source = skinSource };

            if (currentSkinResources != null)
            {
                foreach (var key in currentSkinResources.Keys)
                {
                    dictionary[key] = currentSkinResources[key];
                }
            }

            if (resourceKey != null)
            {
                dictionary[resourceKey] = resource;
            }

            _app.Resources.MergedDictionaries.Add(dictionary);
        }

        #endregion

    }

}