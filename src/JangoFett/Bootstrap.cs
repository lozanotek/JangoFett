namespace JangoFett {
    using System.Web.Http;

    public static class Bootstrap {
        public static void SetupJango(HttpConfiguration configuration) {
            var formatter = CreateScriptFormatter();
            configuration.Formatters.Insert(0, formatter);
        }

        public static JavaScriptFormatter CreateScriptFormatter() {
            var scriptMinifier = CloneContext.ScriptMinifier;
            var nameProvider = CloneContext.ScriptNameProvider;

            return new JavaScriptFormatter(scriptMinifier, nameProvider);
        }
    }
}
