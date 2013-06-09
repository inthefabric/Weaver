namespace Weaver.Core.Elements {

	/*================================================================================================*/
	public class WeaverProperty : IWeaverProperty {

		public string Name { get; protected set; }
		public string DbName { get; protected set; }
		public object Value { get; protected set; }

	}

	/*================================================================================================*/
	public class WeaverProperty<T> : WeaverProperty, IWeaverProperty<T> {

		public T TypedValue { get; protected set; }

	}

}