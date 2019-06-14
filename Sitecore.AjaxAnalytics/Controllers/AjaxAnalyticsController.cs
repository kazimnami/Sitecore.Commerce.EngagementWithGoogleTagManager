﻿using Sitecore.AjaxAnalytics.Models;
using Sitecore.Analytics;
using Sitecore.XConnect;
using Sitecore.XConnect.Client.Configuration;
using System;
using System.Web.Mvc;

namespace Sitecore.AjaxAnalytics.Controllers
{
    public class AjaxAnalyticsController : Controller
    {
        [HttpGet]
        public ActionResult SampleLogger()
        {
            return View("/Views/AjaxAnalytics/SampleLogger.cshtml", new SampleFormModel());
        }

        [AllowAnonymous]
        [HttpPost]
        [OutputCache(Location = System.Web.UI.OutputCacheLocation.None, NoStore = true)]
        public ActionResult RegisterInteraction(string goalId, string productId)
        {
            using (var client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    var goal = Tracker.MarketingDefinitions.Goals[new Guid(goalId)];
                    var pageData = new Analytics.Data.PageEventData(goal.Alias, goal.Id)
                    {
                        Data = productId,
                        Text = "Triggered goal from front-end interaction",
                        DataKey = "productId"
                    };

                    Tracker.Current.CurrentPage.Register(pageData);

                    string result = "{success:true}";
                    return Json(result);
                }
                catch (XdbExecutionException ex)
                {
                    return new HttpStatusCodeResult(500, "Unable to log interaction.");
                }
            }
        }
    }
}