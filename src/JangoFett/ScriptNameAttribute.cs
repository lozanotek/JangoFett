namespace JangoFett {
	using System;

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class ScriptNameAttribute : Attribute {
		public string Name { get; set; }

		public ScriptNameAttribute(string name) {
			Name = name;
		}
	}
}
