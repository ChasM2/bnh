﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bnh.Cms.Repositories;

namespace Bnh.Cms.Models
{
    internal static class InitData
    {
        public static void Init(CmsRepos db)
        {
            // just a simple check whether there is need to initilize data
            if (db.Scenes.Where(s => s.SceneId == Constants.LinkableBricksSceneId).Any()) { return; }
                
            db.Scenes.Insert(new Scene
            {
                SceneId = Constants.LinkableBricksSceneId,
                Title = "Linkable Bricks Scene",
                Walls = new [] 
                {
                    new Wall
                    {
                        Title = "Wall",
                        Width = 100.0f
                    }
                }
            });

            // add index to brick content
            db.Scenes.Collection.EnsureIndex("Walls", "Bricks", "BrickContentId");
        }
    }
}