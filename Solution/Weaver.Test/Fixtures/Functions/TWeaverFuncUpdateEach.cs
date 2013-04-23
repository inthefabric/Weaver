using Moq;
using NUnit.Framework;
using Weaver.Exceptions;
using Weaver.Functions;
using Weaver.Interfaces;
using Weaver.Test.Common.Nodes;
using Weaver.Test.Common.Schema;
using Weaver.Test.Utils;

namespace Weaver.Test.Fixtures.Functions {

	/*================================================================================================*/
	[TestFixture]
	public class TWeaverFuncUpdateEach : WeaverTestBase {

		private Mock<IWeaverQuery> vMockQuery;
		private Mock<IWeaverPath> vMockPath;


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		protected override void SetUp() {
			base.SetUp();
			vMockQuery = new Mock<IWeaverQuery>();

			vMockPath = new Mock<IWeaverPath>();
			vMockPath.SetupGet(x => x.Query).Returns(vMockQuery.Object);
			vMockPath.SetupGet(x => x.Config).Returns(WeavInst.Config);
		}


		////////////////////////////////////////////////////////////////////////////////////////////////
		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void New() {
			var person = new Person();
			var updates = WeavInst.NewUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);

			var f = new WeaverFuncUpdateEach<Person>(updates);
			f.Path = vMockPath.Object;

			Assert.NotNull(f, "Incorrect new().");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void NewEmptyFail() {
			var updates = WeavInst.NewUpdates<Person>();

			WeaverTestUtil.CheckThrows<WeaverFuncException>(true, () => {
				var f = new WeaverFuncUpdateEach<Person>(updates);
			});
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString1() {
			var person = new Person();
			var updates = WeavInst.NewUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);

			vMockQuery.Setup(x => x.AddParam(updates[0].Value)).Returns("321");

			var f = new WeaverFuncUpdateEach<Person>(updates);
			f.Path = vMockPath.Object;

			Assert.AreEqual("sideEffect{it.setProperty('"+TestSchema.Person_PersonId+"',321)}",
				f.BuildParameterizedString(), "Incorrect result.");
		}

		/*--------------------------------------------------------------------------------------------*/
		[Test]
		public void BuildParameterizedString4() {
			var person = new Person();
			var updates = WeavInst.NewUpdates<Person>();
			updates.AddUpdate(person, p => p.PersonId);
			updates.AddUpdate(person, p => p.Name);
			updates.AddUpdate(person, p => p.IsMale);
			updates.AddUpdate(person, p => p.Age);

			vMockQuery.Setup(x => x.AddParam(updates[0].Value)).Returns("321");
			vMockQuery.Setup(x => x.AddParam(updates[1].Value)).Returns("_P0");
			vMockQuery.Setup(x => x.AddParam(updates[2].Value)).Returns("true");
			vMockQuery.Setup(x => x.AddParam(updates[3].Value)).Returns("27.3");

			var f = new WeaverFuncUpdateEach<Person>(updates);
			f.Path = vMockPath.Object;

			string s = 
				"sideEffect{"+
					"it.setProperty('"+TestSchema.Person_PersonId+"',321);"+
					"it.setProperty('Name',_P0);"+
					"it.setProperty('IsMale',true);"+
					"it.setProperty('Age',27.3)"+
				"}";

			Assert.AreEqual(s, f.BuildParameterizedString(), "Incorrect result.");
		}

	}

}