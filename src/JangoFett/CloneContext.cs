namespace JangoFett {
    using System;

    public static class CloneContext {
        private static readonly object _lock = new object();
        private static IScriptNameProvider scriptNameProvider;
        private static IScriptMinifier scriptMinifier;
        private static Func<IScriptNameProvider> nameProvider;
        private static Func<IScriptMinifier> scriptProvider; 

        static CloneContext() {
            SetMinifierProvider(() => new AjaxMinifier());
            SetScriptNameProvider(() => new ScriptNameProvider());
        }
        
        public static void SetScriptNameProvider(Func<IScriptNameProvider> provider) {
            nameProvider = provider;
        }

        public static void SetMinifierProvider(Func<IScriptMinifier> provider) {
            scriptProvider = provider;
        }

        public static IScriptMinifier ScriptMinifier {
            get {
                if (scriptMinifier == null) {
                    lock (_lock) {
                        scriptMinifier = scriptProvider();
                    }
                }

                return scriptMinifier;
            }
        }

        public static IScriptNameProvider ScriptNameProvider {
            get {
                if (scriptNameProvider == null) {
                    lock (_lock) {
                        scriptNameProvider = nameProvider();
                    }
                }

                return scriptNameProvider;
            }
        }
    }
}