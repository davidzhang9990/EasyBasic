using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Xml.Linq;
using Common.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Helper
{
    public static class LangHelper
    {
        public static readonly string EQ_LANGUAGE_KEY = "eq_language";

        public static readonly string READ_DB_STATS = "eq_read";

        private static readonly string LANGUAGE_CONFIG_PATH = "~/Config/language.xml";

        private static IEnumerable<XElement> _langConfig;

        private static void AddCache(string key, string value, DateTime expires = default(DateTime))
        {
            HttpCookie cookie = new HttpCookie(key, value);
            cookie.Path = "/";
            if (!expires.Equals(default(DateTime)))
            {
                cookie.Expires = expires;
            }
            HttpContext.Current.Response.AppendCookie(cookie);
        }
        private static bool GetReadStats()
        {
            var isRead = HttpContext.Current.Request.Cookies.Get(READ_DB_STATS);
            if (isRead == null || string.IsNullOrEmpty(isRead.Value))
            {
                return false;
            }
            return Convert.ToBoolean(isRead.Value);
        }
        public static void SetLanguage(string language)
        {
            AddCache(EQ_LANGUAGE_KEY, language);
        }
        public static void SetReadStats(string readStats)
        {
            AddCache(READ_DB_STATS, readStats, DateTime.Now.AddDays(1));
        }

        public static string GetLanguage()
        {
            var request = HttpContext.Current.Request;
            var cookie = request.Cookies.Get(EQ_LANGUAGE_KEY);

            string language = cookie != null && !string.IsNullOrEmpty(cookie.Value)
                    ? cookie.Value
                    : GetLangByHeadInfo();

            var userId = ClaimHelper.GetClaimValue(EasyClaimType.UserId);
            // islogin and cache exists
            if (string.IsNullOrEmpty(userId) || GetReadStats())
            {
                SetLanguage(language);
                return language;
            }
            // request api GetUserLanguage
            var url = string.Format("{0}api/account/{1}/language", request.GetDomain(), userId);
            var response = new HttpClient().GetAsync(url).Result;

            if (!response.IsSuccessStatusCode)
            {
                return language;
            }

            var userLanguage = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result)["language"].ToString();
            // if the userLanguage and cache different language reset the language cache
            if (!string.IsNullOrEmpty(userLanguage) && !userLanguage.Equals(language))
            {
                language = userLanguage;
                SetLanguage(language);
            }
            // reset the language cache
            SetReadStats(true.ToString());
            return language;

        }

        private static IEnumerable<XElement> GetLangConfig(string path)
        {
            if (!File.Exists(path))
            {
                throw new System.Exception(string.Format("{0}:{1}", path, "配置文件不存在"));
            }

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                XDocument xd = XDocument.Load(fs);
                try
                {
                    return xd.Root.Elements("Group");
                }
                catch (NullReferenceException exception)
                {
                    throw new System.Exception("没有找到跟路径,配置文件可能为空", exception);
                }

            }
        }

        public static IDictionary<string, string[]> GetLangConfig()
        {
            if (_langConfig == null)
            {
                _langConfig = GetLangConfig(HttpContext.Current.Request.MapPath(LANGUAGE_CONFIG_PATH));
            }

            return _langConfig.ToDictionary(x => x.Attribute("Name").Value,
                v => new[]
            {
                v.Attribute("ViewName").Value, 
                v.Attribute("ClientPath").Value
            });

        }

        private static string GetLangByHeadInfo()
        {
            if (_langConfig == null)
            {
                _langConfig = GetLangConfig(HttpContext.Current.Request.MapPath(LANGUAGE_CONFIG_PATH));
            }

            string defaultLang =
                _langConfig.Where(x => x.Attribute("Default") != null && Convert.ToBoolean(x.Attribute("Default").Value))
                    .SingleOrDefault()
                    .Attribute("Name").Value;

            string[] lang = HttpContext.Current.Request.UserLanguages;

            if (lang == null || !lang.Any())
            {
                return defaultLang;
            }

            foreach (var item in lang)
            {
                var xElm = _langConfig.Where(x => x.Value.Split(',').Contains(item));

                if (xElm.Any())
                {
                    return xElm.SingleOrDefault().Attribute("Name").Value;
                }
            }
            return defaultLang;
        }

    }
}
