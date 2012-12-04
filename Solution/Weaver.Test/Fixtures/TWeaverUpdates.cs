using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUpdates : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void AddUpdate() {
			var u = new WeaverUpdates<Person>();
			Assert.AreEqual(0, u.Count, "Incorrect pre-Count.");

			u.AddUpdate(new Person(), x => x.PersonId);
			Assert.AreEqual(1, u.Count, "Incorrect post-Count A.");

			u.AddUpdate(new Person(), x => x.Name);
			Assert.AreEqual(2, u.Count, "Incorrect post-Count B.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PairByIndex() {
			var u = new WeaverUpdates<Person>();
			var p = new Person { PersonId = 123, Name = "Zach" };

			u.AddUpdate(p, x => x.PersonId);
			u.AddUpdate(p, x => x.Name);

			Assert.NotNull(u[0], "Pair at index 0 should be filled.");
			Assert.NotNull(u[1], "Pair at index 1 should be filled.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[TestCase(-1)]
		[TestCase(1)]
		public void PairByIndexBounds(int pIndex) {
			var u = new WeaverUpdates<Person>();
			u.AddUpdate(new Person(), x => x.PersonId);

			WeaverTestUtils.CheckThrows<WeaverException>(true, () => {
				var x = u[pIndex];
			});
		}

	}


	/*================================================================================================*/
	[TestFixture]
	public class TWeaverUpdate : WeaverTestBase {


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildStrings() {
			var u = new WeaverUpdate<Person>();
			u.PropFunc = (x => x.PersonId);
			u.Node = new Person { PersonId = 123 };

			Assert.Null(u.PropName, "PropName should be null.");
			Assert.Null(u.PropVal, "PropVal should be null.");

			u.BuildStrings();

			Assert.AreEqual("PersonId", u.PropName, "Incorrect PropName.");
			Assert.NotNull(u.PropVal, "PropVal should not be null.");
			Assert.AreEqual(123, u.PropVal.Original, "Incorrect PropVal.Original.");
		}

	}

}