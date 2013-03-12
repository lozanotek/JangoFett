namespace JangoFett {
    using System;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    public class JavaScriptFormatter : MediaTypeFormatter {
        public JavaScriptFormatter(IScriptMinifier scriptMinifier, IScriptNameProvider nameProvider) {
            ScriptMinifier = scriptMinifier;
            NameProvider = nameProvider;
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/javascript"));
        }

        public bool ShouldMinify { get; set; }
        public IScriptMinifier ScriptMinifier { get; set; }
        public IScriptNameProvider NameProvider { get; set; }

        public override bool CanReadType(Type type) {
            return true;
        }

        public override bool CanWriteType(Type type) {
            return true;
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, System.Net.Http.HttpRequestMessage request, MediaTypeHeaderValue mediaType) {
            var formatter = Bootstrap.CreateScriptFormatter();
            formatter.ShouldMinify = request.RequestUri.Query.Contains("min");

            return formatter;
        }

        public override Task WriteToStreamAsync(Type type, object value, System.IO.Stream writeStream, System.Net.Http.HttpContent content, System.Net.TransportContext transportContext) {
            var task = Task.Factory.StartNew(() => {
                if (value == null) {
                    writeStream.Flush();
                    return;
                }

                var settings = new JsonSerializerSettings {
                    NullValueHandling = NullValueHandling.Ignore,
                };

                var json = JsonConvert.SerializeObject(value, Formatting.None, settings);
                var variableName = NameProvider.GetScriptName(value);

                if (string.IsNullOrEmpty(variableName)) {
                    variableName = "__jango";
                }

                var script = string.Format("var {0} = {1};", variableName, json);
                if (ShouldMinify) {
                    script = ScriptMinifier.Minify(script);
                }

                var buffer = Encoding.Default.GetBytes(script);
                writeStream.Write(buffer, 0, buffer.Length);
                writeStream.Flush();
            });

            return task;
        }
    }
}
