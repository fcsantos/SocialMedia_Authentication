using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facebook;
using Newtonsoft.Json;
using System.Web.Security;
using SocialMediaLoginMVC.Models;

namespace SocialMediaLoginMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.IsUsuarioLogado = false;
            if (TempData["NomeUsuario"] != null)
            {
                ViewBag.Usuario = TempData["NomeUsuario"].ToString();
                ViewBag.IsUsuarioLogado = true;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private Uri RedirectedUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        [AllowAnonymous]
        public ActionResult Facebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "118812552167300",
                client_secret = "6022616e1b176fe221dd20d591173444",
                redirect_uri = RedirectedUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });

            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "118812552167300",
                client_secret = "6022616e1b176fe221dd20d591173444",
                redirect_uri = RedirectedUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;
            Session["AccessToken"] = accessToken;
            fb.AccessToken = accessToken;
            dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");
            string email = me.email;
            string lastname = me.last_name;
            string picture = me.picture.data.url;
            FormsAuthentication.SetAuthCookie(email, false);
            var json = JsonConvert.SerializeObject(me);
            Usuario jsonDes = JsonConvert.DeserializeObject<Usuario>(json);
            TempData["NomeUsuario"] = jsonDes.first_name + " " + jsonDes.last_name;
            return RedirectToAction("Index", "Home");
        }
    }
}