## Weaver

Weaver provides a fluent, strongly-typed interface for generating [Gremlin](https://github.com/tinkerpop/gremlin) scripts (for .NET/C#).

This requires that your graph domain classes use Weaver's custom attributes for [vertices](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverVertexAttribute.cs), [edges](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverEdgeAttribute.cs), and [properties](https://github.com/inthefabric/Weaver/blob/master/Solution/Weaver.Core/Elements/WeaverPropertyAttribute.cs). The `Weaver.Titan` package includes additional [Titan](https://github.com/thinkaurelius/titan)-specific functionality for creating graph data-types, groups, and indices.

#### Install with NuGet

- [Weaver.Core](https://www.nuget.org/packages/Weaver)
```
PM> Install-Package Weaver
```

- [Weaver.Titan](https://www.nuget.org/packages/Weaver.Titan)
```
PM> Install-Package Weaver.Titan 
```

#### Basic Usage

Weaver converts C# code:
```cs
myWeaverObj.Graph.V.ExactIndex<User>(x => x.Name, "Zach")
```

...into Gremlin script:
```
g.V('U_Na','Zach')
```

The Gremlin script can also be parameterized (enabled by default) to achieve more efficient query compilation on the database side.

#### Fabric

Weaver was built to support the [Fabric](https://github.com/inthefabric/Fabric) project, which provides several [useful examples](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs) of Weaver configuration, setup, and usage. 

A slightly-modified example from Fabric [code](https://github.com/inthefabric/Fabric/blob/master/Solution/Fabric.Api.Modify/Tasks/ModifyTasks.cs#L50):
```cs
IWeaverFuncAs<Member> memAlias;

IWeaverQuery q = myWeaverObj.Graph
	.V.ExactIndex<User>(x => x.ArtifactId, 123)
	.DefinesMemberList.ToMember
		.As(out memAlias)
	.InAppDefines.FromApp
		.Has(x => x.ArtifactId, WeaverStepHasOp.EqualTo, pApiCtx.AppId)
	.Back(memAlias)
	.HasMemberTypeAssign.ToMemberTypeAssign
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.None)
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.Invite)
		.Has(x => x.MemberTypeId, WeaverStepHasOp.NotEqualTo, (byte)MemberTypeId.Request)
	.Back(memAlias)
	.ToQuery();

SendGremlinRequest(q.Script, q.Params);
```


[![githalytics.com alpha](https://cruel-carlota.pagodabox.com/9caca4070a7a2601105b67a6840644c2 "githalytics.com")](http://githalytics.com/inthefabric/Weaver)
