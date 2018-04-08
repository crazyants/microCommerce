namespace microCommerce.Setting
{
    public class SeoSettings : ISettings
    {
        /// <summary>
        /// A value indicating whether JS file bundling and minification is enabled
        /// </summary>
        public bool EnableJsBundling { get; set; }

        /// <summary>
        /// A value indicating whether CSS file bundling and minification is enabled
        /// </summary>
        public bool EnableCssBundling { get; set; }

        /// <summary>
        /// A value indicating whether Twitter META tags should be generated
        /// </summary>
        public bool TwitterMetaTags { get; set; }

        /// <summary>
        /// A value indicating whether Open Graph META tags should be generated
        /// </summary>
        public bool OpenGraphMetaTags { get; set; }

        /// <summary>
        /// Custom tags in the <![CDATA[<head></head>]]> section
        /// </summary>
        public string CustomHeadTags { get; set; }
        
        /// <summary>
        /// A value indicating whether canonical URL tags should be used
        /// </summary>
        public bool CanonicalUrlsEnabled { get; set; }

        /// <summary>
        /// A value indicating whether SEO friendly URLs with multiple languages are enabled
        /// </summary>
        public bool SeoFriendlyUrlsForLanguagesEnabled { get; set; }
    }
}