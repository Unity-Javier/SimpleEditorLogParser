namespace SimpleEditorLogParser.Parser
{
    public class AssetTimings
    {
        public string assetPath;
        public float seconds;
        public string extension;
        public string categorizedExtension;

        public AssetTimings(string pathToAsset, string importSeconds)
        {
            assetPath = pathToAsset;
            float.TryParse(importSeconds, out seconds);
            extension = ParserUtils.GetExtension(assetPath, ParserUtils.kDefaultSupportedExtensions);
            categorizedExtension = ParserUtils.CategorizeExtension(extension, ParserUtils.kDefaultSupportedExtensions);
        }

        public override string ToString()
        {
            return $"{assetPath},{extension},{categorizedExtension},{seconds}";
        }
    }
}
