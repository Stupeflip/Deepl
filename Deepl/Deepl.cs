using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net;

namespace Deepl
{
    internal class Deepl
    {
        public enum Language
        {
            BG,
            CH,
            CS,
            DA,
            NL,
            EN, //en-us
            ET,
            FI,
            FR,
            DE,
            EL,
            HU,
            IT,
            JP,
            LV,
            LT,
            PL,
            PT, //pt-br
            RO,
            RU,
            SK,
            ES,
            sv
        }

        public static HttpClient Client = new();

        public string? Resp { get; set; }

        public Deepl(Language selectedLanguage, Language targetLanguage, string text, string? proxy = null)
        {
            Client = proxy != null ? new HttpClient(new HttpClientHandler() { Proxy = new WebProxy(proxy) }) : new();

            var resp = Client.PostAsync("https://www2.deepl.com/jsonrpc?method=LMT_handle_jobs", new StringContent($"{{\"jsonrpc\":\"2.0\",\"method\": \"LMT_handle_jobs\",\"params\":{{\"jobs\":[{{\"kind\":\"default\",\"sentences\":[{{\"text\":\"{text}\",\"id\":0,\"prefix\":\"\"}}],\"preferred_num_beams\":0}}],\"lang\":{{\"preference\":{{\"weight\":{{\"DE\":0.25677,\"EN\":3.93454,\"ES\":0.17482,\"FR\":2.18168,\"IT\":0.15395,\"JA\":0.06588,\"NL\":0.12132,\"PL\":0.0839,\"PT\":0.09248,\"RU\":0.0436,\"ZH\":0.0508,\"BG\":0.03227,\"CS\":0.06775,\"DA\":0.20889,\"EL\":0.02572,\"ET\":0.09304,\"HU\":0.1203,\"LT\":0.03711,\"LV\":0.04399,\"RO\":0.06114,\"SK\":0.08773,\"SL\":0.0696,\"SV\":0.18702}},\"default\":\"default\"}},\"source_lang_user_selected\":\"{selectedLanguage}\",\"target_lang\":\"{targetLanguage}\"}},\"priority\":1,\"commonJobParams\":{{\"browserType\":1}},\"timestamp\":{DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds}}}}}", Encoding.UTF8, "application/json"))
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            if (resp.StatusCode == HttpStatusCode.OK)
            {
                string respToken = JsonSerializer.Deserialize<JsonElement>(resp.Content.ReadAsStringAsync().Result)
                    .GetProperty("result")
                    .GetProperty("translations")[0]
                    .GetProperty("beams")[0]
                    .GetProperty("sentences")[0]
                    .GetProperty("text")
                    .ToString();

                Resp = respToken;
            }
            else
            {
                Console.WriteLine("Could not translate.");
            }
        }
    }
}
