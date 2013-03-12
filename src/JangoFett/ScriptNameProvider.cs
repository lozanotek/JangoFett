namespace JangoFett {
    using System;

    public class ScriptNameProvider : IScriptNameProvider {
        public string GetScriptName(object value) {
            if (value == null) return string.Empty;
            var type = value.GetType();
            var attributes = type.GetCustomAttributes(typeof(ScriptNameAttribute), true) as ScriptNameAttribute[];

            if (attributes == null || attributes.Length == 0) {
                var typeName = type.Name;
                return Char.ToLowerInvariant(typeName[0]) + typeName.Substring(1);
            }

            return attributes[0].Name;
        }
    }
}
