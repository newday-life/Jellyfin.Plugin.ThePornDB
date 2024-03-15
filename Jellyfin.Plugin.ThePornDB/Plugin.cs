using System;
using System.Collections.Generic;
using MediaBrowser.Common.Plugins;
using ThePornDB.Configuration;

#if __EMBY__
using System.IO;
using MediaBrowser.Common;
using MediaBrowser.Common.Net;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Drawing;
using MediaBrowser.Model.Logging;
#else
using MediaBrowser.Common.Configuration;
using MediaBrowser.Model.Serialization;
using MediaBrowser.Model.Plugins;
using System.Net.Http;
using Microsoft.Extensions.Logging;
#endif

[assembly: CLSCompliant(false)]

namespace ThePornDB
{
#if __EMBY__
    public class Plugin : BasePlugin<PluginConfiguration>, IHasThumbImage
    {
        public Plugin(IApplicationPaths applicationPaths, IHttpClient http, ILogManager logger)
#else
    public class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
    {
        public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer, IHttpClientFactory http, ILogger<Plugin> logger)
#endif
#if __EMBY__
            : base(applicationHost)
#else
            : base(applicationPaths, xmlSerializer)
#endif
        {
            Instance = this;
            Http = http;

#if __EMBY__
            if (logger != null)
            {
                Log = logger.GetLogger(this.Name);
            }
#else
            Log = logger;
#endif
        }

#if __EMBY__
        public static IHttpClient Http { get; set; }
#else
        public static IHttpClientFactory Http { get; set; }
#endif

        public static ILogger Log { get; set; }

        public static Plugin Instance { get; private set; }

        public override string Name => "ThePornDB";

        public override Guid Id => Guid.Parse("fb7580cf-576d-4991-8e56-0b4520c111d3");

#if __EMBY__
        public ImageFormat ThumbImageFormat => ImageFormat.Png;

        public Stream GetThumbImage() => this.GetType().Assembly.GetManifestResourceStream($"{this.GetType().Namespace}.Resources.logo.png");
        public PluginConfiguration Configuration => GetOptions();
#else
        public IEnumerable<PluginPageInfo> GetPages()
            => new[]
            {
                new PluginPageInfo
                {
                    Name = this.Name,
                    EmbeddedResourcePath = $"{this.GetType().Namespace}.Configuration.configPage.html",
                },
            };
#endif      
    }
}
