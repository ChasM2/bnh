﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Models;
using Bnh.Cms.Repositories;
using Newtonsoft.Json;

namespace Bnh.Cms.ViewModels
{
    public class BrickViewModel
    {
        public string Title { get; set; }

        public float Width { get; set; }

        public string BrickContentId { get; set; }

        public BrickContent Content { get; set; }

        public string WidthString
        {
            get { return this.Width.ToString("F") + "%"; }
        }

        public string ToJson()
        {
            var properties = new Dictionary<string, object>();
            properties["title"] = this.Title;
            properties["width"] = this.Width;
            properties["brickcontentid"] = this.BrickContentId;
            return JsonConvert.SerializeObject(properties);
        }

        internal static BrickViewModel Create(Brick brick, CmsRepos db)
        {
            return new BrickViewModel
            {
                Title = brick.Title,
                Width = brick.Width,
                BrickContentId = brick.BrickContentId,
                Content = brick.GetContent(db)
            };
        }
    }
}