﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bnh.Entities;
using System.Data.Objects.DataClasses;
using Bnh.Web.Models;
using Ms.Cms.Models;
using MongoDB.Bson;
using Ms.Cms.Controllers;

namespace Bnh.Controllers
{
    [Authorize]
    public class CommunityController : Controller
    {
        private BleEntities db = new BleEntities();

        //
        // GET: /Community/
        public ViewResult Index()
        {
            var city = db.Cities.First(c => c.Name == "Calgary");
            city.Communities = db.Communities.Where(c => c.CityId ==city.CityId).ToList();
            return View(city);
        }

        [HttpGet]
        public JsonResult Zones()
        {
            UrlHelper urlHelper = new UrlHelper(this.HttpContext.Request.RequestContext);
            return Json(db.Zones.OrderBy(m => m.Order).Include("Communities").ToArray().Select((item) =>
                        {
                            ZoneDto zone = new ZoneDto();
                            zone.Name = item.Name;
                            zone.Communities = item.Communities.Select((community) =>
                                                                        {
                                                                            CommunityDto communityDto = new CommunityDto();
                                                                            communityDto.DistanceToCenter = community.Remoteness;
                                                                            communityDto.Id = community.Id;
                                                                            communityDto.Name = community.Name;
                                                                            communityDto.UrlId = community.UrlId;
                                                                            communityDto.HasLake = community.HasLake;
                                                                            communityDto.HasMountainView = community.HasMountainView;
                                                                            communityDto.HasClubOfFacility = community.HasClubOrFacility;
                                                                            communityDto.HasParksAndPathways = community.HasParksAndPathways;
                                                                            communityDto.HasShoppingPlaza = community.HasParksAndPathways;
                                                                            communityDto.HasWaterFeature = community.HasWaterFeature;
                                                                            communityDto.GpsBounds = community.GpsBounds;
                                                                            communityDto.GpsLocation = community.GpsLocation;
                                                                            communityDto.DeleteUrl = urlHelper.Action("Delete", "Community", new { id = community.Id });
                                                                            communityDto.DetailsUrl = urlHelper.Action("Details", "Community", new { id = community.UrlId });
                                                                            communityDto.InfoPopup = String.Format("<a href='{0}'>{1}</a>",
                                                                                urlHelper.Action("Details", "Community", new { id = community.UrlId }), community.Name);
                                                                            return
                                                                                communityDto;
                                                                        });
                            return
                                zone;
                        }), JsonRequestBehavior.AllowGet);
        }

        public ViewResult Details(string id)
        {
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id) :
                (Func<Community, bool>)(c => c.UrlId == id);
            return View(db.Communities.Single(checker));
        }

        //
        // GET: /Community/Create
        [Authorize(Roles = "content_manager")]
        public ActionResult Create()
        {
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones);
            using(var cm = new CmsEntities())
            {
                var sceneTemplates = from s in cm.Scenes
                                     where s.IsTemplate
                                     select new { id = s.SceneId, title = s.Title };
                ViewBag.Templates = new SelectList(new[] { new { id = string.Empty, title = string.Empty } }.Union(sceneTemplates), "id", "title");

                var city = db.Cities.First(c => c.Name == "Calgary");
                ViewBag.CityZones = new SelectList(city.Zones);
                ViewBag.CityId = city.CityId;
            }
            return View();
        } 

        //
        // POST: /Community/Create

        [HttpPost]
        [Authorize(Roles = "content_manager")]
        public ActionResult Create(Community community)
        {
            if (ModelState.IsValid)
            {
                db.Communities.Insert(community);

                var templateSceneId = this.Request.Form["templateSceneId"];
                if (!string.IsNullOrEmpty(templateSceneId))
                {
                    var sceneController = new SceneController();
                    sceneController.ApplyTemplate(community.CommunityId, templateSceneId);
                }
                return RedirectToAction("Edit", new { id = community.UrlId });
            }

            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
            return View(community);
        }
        
        //
        // GET: /Community/Edit/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(string id)
        {
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id):
                (Func<Community, bool>)(c => c.UrlId == id);
            Community community = db.Communities.Single(checker);
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
            return View(community);
        }

        //
        // POST: /Community/Edit/5

        [HttpPost]
        [Authorize(Roles = "content_manager")]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                db.Communities.Save(community);
                
                return RedirectToAction("Details", new { id = community.UrlId });
            }
            ViewBag.CityZones = new SelectList(db.Cities.First(c => c.Name == "Calgary").Zones, community.Zone);
            return View(community);
        }

        [HttpGet]
        public ActionResult EditScene(string id)
        {
            ObjectId oid;
            var checker = ObjectId.TryParse(id, out oid) ?
                (Func<Community, bool>)(c => c.CommunityId == id) :
                (Func<Community, bool>)(c => c.UrlId == id);
            Community community = db.Communities.Single(checker);
            return View(community);
        }

        //
        // GET: /Community/Delete/5
        [Authorize(Roles = "content_manager")]
        public ActionResult Delete(string id)
        {
            Community community = db.Communities.Single(c => c.CommunityId == id);
            return View(community);
        }

        //
        // POST: /Community/Delete/5

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "content_manager")]
        public ActionResult DeleteConfirmed(string id)
        {            
            db.Communities.Delete(id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}