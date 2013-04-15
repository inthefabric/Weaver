namespace Weaver.Interfaces {

	/*================================================================================================*/
	public interface IWeaverProperty {

		string Name { get; }
		string DbName { get; }
		object Value { get; }

	}

	/*================================================================================================*/
	public interface IWeaverProperty<T> : IWeaverProperty {

		T TypedValue { get; }

	}

}