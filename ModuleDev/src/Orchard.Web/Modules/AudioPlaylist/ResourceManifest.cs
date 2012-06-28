using Orchard.UI.Resources;

namespace Orchard.Blogs
{
    public class ResourceManifest : IResourceManifestProvider
    {
        public void BuildManifests(ResourceManifestBuilder builder)
        {
            var manifest = builder.Add();
            manifest.DefineStyle("AudioPlaylist").SetUrl("AudioPlaylist.css");

            manifest.DefineScript("AudioPlaylist").SetUrl("AudioPlaylist.js").SetDependencies("jQuery");
            manifest.DefineScript("JPlayer").SetUrl("jquery.jplayer.js").SetDependencies("jQuery");
        }
    }
}
