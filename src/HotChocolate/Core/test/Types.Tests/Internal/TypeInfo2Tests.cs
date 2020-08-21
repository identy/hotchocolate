﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HotChocolate.Types;
using Xunit;

namespace HotChocolate.Internal
{
    public class TypeInfoTests
    {
        [InlineData(typeof(string), "String")]
        [InlineData(typeof(Task<string>), "String")]
        [InlineData(typeof(List<string>), "[String]")]
        [InlineData(typeof(Task<List<string>>), "[String]")]
        [InlineData(typeof(Task<List<List<string>>>), "[[String]]")]
        [InlineData(typeof(string[]), "[String]")]
        [InlineData(typeof(Task<string[]>), "[String]")]
        [InlineData(typeof(NativeType<string>), "String")]
        [InlineData(typeof(NativeType<Task<string>>), "String")]
        [InlineData(typeof(NativeType<List<string>>), "[String]")]
        [InlineData(typeof(NativeType<Task<List<string>>>), "[String]")]
        [InlineData(typeof(NativeType<string[]>), "[String]")]
        [InlineData(typeof(NativeType<Task<string[]>>), "[String]")]
        [InlineData(typeof(int), "String!")]
        [InlineData(typeof(Task<int>), "String!")]
        [InlineData(typeof(List<int>), "[String!]")]
        [InlineData(typeof(Task<List<int>>), "[String!]")]
        [InlineData(typeof(Task<List<List<int>>>), "[[String!]]")]
        [InlineData(typeof(int[]), "[String!]")]
        [InlineData(typeof(Task<int[]>), "[String!]")]
        [InlineData(typeof(NativeType<int>), "String!")]
        [InlineData(typeof(NativeType<Task<int>>), "String!")]
        [InlineData(typeof(NativeType<List<int>>), "[String!]")]
        [InlineData(typeof(NativeType<Task<List<int>>>), "[String!]")]
        [InlineData(typeof(NativeType<int[]>), "[String!]")]
        [InlineData(typeof(NativeType<Task<int[]>>), "[String!]")]
        [InlineData(typeof(int?), "String")]
        [InlineData(typeof(Task<int?>), "String")]
        [InlineData(typeof(List<int?>), "[String]")]
        [InlineData(typeof(Task<List<int?>>), "[String]")]
        [InlineData(typeof(Task<List<List<int?>>>), "[[String]]")]
        [InlineData(typeof(int?[]), "[String]")]
        [InlineData(typeof(Task<int?[]>), "[String]")]
        [InlineData(typeof(NativeType<int?>), "String")]
        [InlineData(typeof(NativeType<Task<int?>>), "String")]
        [InlineData(typeof(NativeType<List<int?>>), "[String]")]
        [InlineData(typeof(NativeType<Task<List<int?>>>), "[String]")]
        [InlineData(typeof(NativeType<int?[]>), "[String]")]
        [InlineData(typeof(NativeType<Task<int?[]>>), "[String]")]
        [InlineData(typeof(Optional<string[]>), "[String]")]
        [InlineData(typeof(ValueTask<string[]>), "[String]")]
        [InlineData(typeof(IAsyncEnumerable<string>), "[String]")]
        [InlineData(typeof(IEnumerable<string>), "[String]")]
        [InlineData(typeof(IQueryable<string>), "[String]")]
        [Theory]
        public void CreateTypeInfoFromRuntimeType(
            Type clrType,
            string expectedTypeName)
        {
            // arrange
            // act
            var typeInfo = TypeInfo2.Create(clrType);

            // assert
            Assert.Equal(expectedTypeName, typeInfo.CreateType(new StringType()).Print());
        }

        [InlineData(typeof(CustomStringList), "[String]")]
        [InlineData(typeof(List<string>), "[String]")]
        [InlineData(typeof(Collection<string>), "[String]")]
        [InlineData(typeof(ReadOnlyCollection<string>), "[String]")]
        [InlineData(typeof(ImmutableList<string>), "[String]")]
        [InlineData(typeof(ImmutableArray<string>), "[String]!")]
        [InlineData(typeof(IList<string>), "[String]")]
        [InlineData(typeof(ICollection<string>), "[String]")]
        [InlineData(typeof(IEnumerable<string>), "[String]")]
        [InlineData(typeof(IReadOnlyCollection<string>), "[String]")]
        [InlineData(typeof(IReadOnlyList<string>), "[String]")]
        [InlineData(typeof(string[]), "[String]")]
        [Theory]
        public void SupportedListTypes(Type clrType, string expectedTypeName)
        {
            // arrange
            // act
            var typeInfo = TypeInfo2.Create(clrType);

            // assert
            Assert.Equal(expectedTypeName, typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullListNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullListNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullListNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullListNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableListNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableListNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableListNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableListNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullCollectionNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullCollectionNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullCollectionNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullCollectionNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableCollectionNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableCollectionNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableCollectionNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableCollectionNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullArrayNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullArrayNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NonNullArrayNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NonNullArrayNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]!" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableArrayNullableElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableArrayNullableElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String]" , typeInfo.CreateType(new StringType()).Print());
        }

        [Fact]
        public void NullableArrayNonNullElement()
        {
            // arrange
            MethodInfo methodInfo =
                typeof(Nullability).GetMethod(nameof(Nullability.NullableArrayNonNullElement));

            // act
            var typeInfo = TypeInfo2.Create(methodInfo!);

            // assert
            Assert.Equal("[String!]" , typeInfo.CreateType(new StringType()).Print());
        }

        [InlineData(typeof(NonNullType<NativeType<string>>), "String!")]
        [InlineData(typeof(NonNullType<NativeType<int?>>), "String!")]
        [InlineData(typeof(NonNullType<NativeType<List<NonNullType<NativeType<string>>>>>), "[String!]!")]
        [InlineData(typeof(NonNullType<NativeType<NonNullType<NativeType<string>>[]>>), "[String!]!")]
        [InlineData(typeof(NonNullType<NativeType<List<NonNullType<NativeType<int?>>>>>), "[String!]!")]
        [InlineData(typeof(NonNullType<NativeType<NonNullType<NativeType<int?>>[]>>), "[String!]!")]
        [Theory]
        public void MixedTypes(Type clrType, string expectedTypeName)
        {
            // arrange
            // act
            var typeInfo = TypeInfo2.Create(clrType);

            // assert
            Assert.Equal(expectedTypeName, typeInfo.CreateType(new StringType()).Print());
        }

        [InlineData(typeof(NativeType<Task<string>>), typeof(string))]
        [InlineData(typeof(NativeType<string>), typeof(string))]
        [InlineData(typeof(Task<string>), typeof(string))]
        [InlineData(typeof(ValueTask<string[]>), typeof(string[]))]
        [InlineData(typeof(ValueTask<List<string>>), typeof(List<string>))]
        [Theory]
        public void Unwrap(Type type, Type expectedReducedType)
        {
            // arrange
            // act
            IExtendedType reducedType = TypeInfo2.RuntimeType.Unwrap(type);

            // assert
            Assert.Equal(expectedReducedType, reducedType.Type);
        }

        [Fact]
        public void Create_TypeInfo_From_RewrittenType()
        {
            // arrange
            Type type = typeof(ListType<NonNullType<NativeType<string>>>);

            // act
            bool success = TypeInfo2.TryCreate(type, out TypeInfo2 typeInfo);

            // assert
            Assert.True(success);

            Assert.Collection(typeInfo.Components,
                t => Assert.Equal(TypeComponentKind.List, t),
                t => Assert.Equal(TypeComponentKind.NonNull, t),
                t => Assert.Equal(TypeComponentKind.Named, t));

            IType schemaType = typeInfo.CreateType(new StringType());

            Assert.IsType<StringType>(
                Assert.IsType<NonNullType>(
                    Assert.IsType<ListType>(schemaType).ElementType).Type);
        }

        [Fact]
        public void Case5()
        {
            // arrange
            Type nativeType =
                typeof(NonNullType<ListType<NonNullType<ListType<NonNullType<StringType>>>>>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.Equal("[[String!]!]!", type.Print());
        }

        [Fact]
        public void Case4()
        {
            // arrange
            Type nativeType = typeof(NonNullType<ListType<NonNullType<StringType>>>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<NonNullType>(type);
            type = ((NonNullType)type).Type as IOutputType;
            Assert.IsType<ListType>(type);
            type = ((ListType)type).ElementType as IOutputType;
            Assert.IsType<NonNullType>(type);
            type = ((NonNullType)type).Type as IOutputType;
            Assert.IsType<StringType>(type);
        }

        [Fact]
        public void Case3_1()
        {
            // arrange
            Type nativeType = typeof(ListType<NonNullType<StringType>>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<ListType>(type);
            type = ((ListType)type).ElementType as IOutputType;
            Assert.IsType<NonNullType>(type);
            type = ((NonNullType)type).Type as IOutputType;
            Assert.IsType<StringType>(type);
        }

        [Fact]
        public void Case3_2()
        {
            // arrange
            Type nativeType = typeof(NonNullType<ListType<StringType>>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<NonNullType>(type);
            type = ((NonNullType)type).Type as IOutputType;
            Assert.IsType<ListType>(type);
            type = ((ListType)type).ElementType as IOutputType;
            Assert.IsType<StringType>(type);
        }

        [Fact]
        public void Case2_1()
        {
            // arrange
            Type nativeType = typeof(NonNullType<StringType>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<NonNullType>(type);
            type = ((NonNullType)type).Type as IOutputType;
            Assert.IsType<StringType>(type);
        }

        [Fact]
        public void Case2_2()
        {
            // arrange
            Type nativeType = typeof(ListType<StringType>);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<ListType>(type);
            type = ((ListType)type).ElementType as IOutputType;
            Assert.IsType<StringType>(type);
        }

        [Fact]
        public void Case1()
        {
            // arrange
            Type nativeType = typeof(StringType);

            // act
            var success = TypeInfo2.TryCreate(nativeType, out TypeInfo2 typeInfo);
            IType type = typeInfo!.CreateType(new StringType());

            // assert
            Assert.True(success);
            Assert.IsType<StringType>(type);
        }

        private class CustomStringList
            : CustomStringListBase
        {
        }

        private class CustomStringListBase
            : List<string>
        {
        }

#nullable enable

        public class Nullability
        {
            public List<string> NonNullListNonNullElement() => default!;

            public List<string?> NonNullListNullableElement() => default!;

            public List<string?>? NullableListNullableElement() => default;

            public List<string>? NullableListNonNullElement() => default;

            public ICollection<string> NonNullCollectionNonNullElement() => default!;

            public ICollection<string?> NonNullCollectionNullableElement() => default!;

            public ICollection<string?>? NullableCollectionNullableElement() => default;

            public ICollection<string>? NullableCollectionNonNullElement() => default;

            public string[] NonNullArrayNonNullElement() => default!;

            public string?[] NonNullArrayNullableElement() => default!;

            public string?[]? NullableArrayNullableElement() => default;

            public string[]? NullableArrayNonNullElement() => default;
        }

#nullable disable
    }
}