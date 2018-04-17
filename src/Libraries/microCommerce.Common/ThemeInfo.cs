namespace microCommerce.Common
{
    public class ThemeInfo
    {
        /// <summary>
        /// Gets or sets the theme system name
        /// </summary>
        public string ThemeName { get; set; }

        /// <summary>
        /// Gets or sets the theme friendly name
        /// </summary>
        public string FriendlyName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the theme supports RTL (right-to-left)
        /// </summary>
        public bool SupportRtl { get; set; }

        /// <summary>
        /// Gets or sets the path to the preview image of the theme
        /// </summary>
        public string PreviewPicturePath { get; set; }

        /// <summary>
        /// Gets or sets the preview text of the theme
        /// </summary>
        public string PreviewDescription { get; set; }
    }
}