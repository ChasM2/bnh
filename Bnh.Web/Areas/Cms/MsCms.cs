﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Models;
using Bnh.Cms.Models.Utilities;
using System.Web.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Bnh.Cms
{
    public static class MsCms
    {
        public static HttpApplication Application { get; private set; }

        public static string LayoutView { get; private set; }

        public static string DesignerRole { get; private set; }

        public static string TinyMceScript { get; private set; }

        public static IEnumerable<BrickRegistration> RegisteredBrickTypes { get; private set; }

        public static string GoogleMapsApiScript { get; set; }

        public static void Setup(HttpApplication app, string layout = null, string designer = null, string tinymce = null, string googleMaps = null, IEnumerable<BrickRegistration> bricks = null)
        {
            Application = app;
            // TODO: provide default layout
            LayoutView = layout ?? "";
            DesignerRole = designer ?? "";
            TinyMceScript = tinymce ?? "";
            GoogleMapsApiScript = googleMaps ?? "";
            RegisteredBrickTypes = GetRegisteredBrickTypes(bricks ?? Enumerable.Empty<BrickRegistration>()).ToList();

            // Register known types map to make possible to deserialize inherited types by descriminator
            foreach (var br in RegisteredBrickTypes)
            {
                BsonClassMap.LookupClassMap(br.Type);
            }
        }

        private static IEnumerable<BrickRegistration> GetRegisteredBrickTypes(IEnumerable<BrickRegistration> externalBricks)
        {
            yield return new BrickRegistration { Type = typeof(HtmlContent), Title = "Rich Text", View = "" };
            yield return new BrickRegistration { Type = typeof(RazorContent), Title = "Razor Template", View = "" };
            yield return new BrickRegistration { Type = typeof(MapContent), Title = "Map", View = "" };
            yield return new BrickRegistration { Type = typeof(GalleryContent), Title = "Gallery", View = "" };
            yield return new BrickRegistration { Type = typeof(ReviewsContent), Title = "Reviews", View = "" };
            yield return new BrickRegistration { Type = typeof(EmptyContent), Title = "Empty", View = "" };
            yield return new BrickRegistration { Type = typeof(LinkableContent), Title = "Linkable", View = "" };
            yield return new BrickRegistration { Type = typeof(TocContent), Title = "Table of Content", View = "" };

            foreach (var externalBrick in externalBricks)
            {
                yield return externalBrick;
            }
        }
    }
}