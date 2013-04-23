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
			var u = WeavInst.NewUpdates<Person>();
			Assert.AreEqual(0, u.Count, "Incorrect pre-Count.");

			u.AddUpdate(new Person(), x => x.PersonId);
			Assert.AreEqual(1, u.Count, "Incorrect post-Count A.");

			u.AddUpdate(new Person(), x => x.Name);
			Assert.AreEqual(2, u.Count, "Incorrect post-Count B.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void PairByIndex() {
			var u = WeavInst.NewUpdates<Person>();
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
			var u = WeavInst.NewUpdates<Person>();
			u.AddUpdate(new Person(), x => x.PersonId);

			WeaverTestUtil.CheckThrows<WeaverException>(true, () => {
				var x = u[pIndex];
			});
		}

	}

}