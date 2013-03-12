namespace JangoFett {
    using Microsoft.Ajax.Utilities;

    public class AjaxMinifier : IScriptMinifier {
        public AjaxMinifier() : this(new Minifier()) { }

        public AjaxMinifier(Minifier minifier) {
            ScriptMinifier = minifier;
        }

        public Minifier ScriptMinifier { get; private set; }

        public string Minify(string originalScript) {
            if (ScriptMinifier == null) return null;

            var settings = new CodeSettings {
                CollapseToLiteral = true,
                Format = JavaScriptFormat.Normal,
                MinifyCode = true,
                PreserveFunctionNames = true,
                OutputMode = OutputMode.SingleLine,
                RemoveUnneededCode = true,
            };

            return ScriptMinifier.MinifyJavaScript(originalScript, settings);
        }
    }
}
