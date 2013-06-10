using Moq;
using Weaver.Core.Elements;
using Weaver.Core.Path;

namespace Weaver.Test.Core.Common {

	/*================================================================================================*/
	public class TestItem : WeaverElement {

		public Mock<IWeaverPath> MockPath { get; private set; }

		[WeaverItemProperty]
		public int Value { get; set; }


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		public TestItem() {
			MockPath = new Mock<IWeaverPath>();
			Path = MockPath.Object;
		}

		/*--------------------------------------------------------------------------------------------*/
		public override string BuildParameterizedString() {
			return null;
		}

	}

}