using Moq;
using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Test.Core.Common {

	/*================================================================================================*/
	public class TestElement<T> : WeaverElement<T> where T : class, IWeaverElement {

		public Mock<IWeaverPath> MockPath { get; private set; }

		[WeaverItemProperty]
		public int Value { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestElement() {
			MockPath = new Mock<IWeaverPath>();
			Path = MockPath.Object;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return null;
		}

	}

}