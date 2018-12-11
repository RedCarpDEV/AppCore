using System.Linq;
using System.Windows;

namespace RCd.AppCore.Ui.Desktop.Theming
{

    public class SkinDictionary : ResourceDictionary
    {

        #region Constructors

        public SkinDictionary()
        {
            SkinManager.Current.Initialize(Application.Current);

            UpdateSkin(SkinManager.DEFAULT_SKIN);
        }

        #endregion

        #region Properties

        private Skin _skin;
        public Skin Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                SkinManager.Current.AddSkinDictionary(_skin.Source, null, null, null);
                UpdateSkin(_skin.Name);
            }
        }

        #endregion

        #region Methods

        private void UpdateSkin(string skinName)
        {
            SkinManager.Current.SetSkin(skinName);

            if (SkinManager.Current.FindSkinSourceByName(skinName, out var source))
            {
                Source = Application.Current.Resources.MergedDictionaries.FirstOrDefault(s => s.Source == source)?.Source;
            }
        }

        #endregion

    }

}