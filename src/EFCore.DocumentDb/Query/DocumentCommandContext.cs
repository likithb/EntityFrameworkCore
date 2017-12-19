// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;

namespace Microsoft.EntityFrameworkCore.Query
{
    public class DocumentCommandContext
    {
        private readonly DocumentClient _documentClient;
        private readonly string _collectionUri;
        private readonly Func<QuerySqlGenerator> _sqlGeneratorFunc;

        public DocumentCommandContext(
            DocumentClient documentClient, string collectionUri, Func<QuerySqlGenerator> sqlGeneratorFunc)
        {
            _documentClient = documentClient;
            _collectionUri = collectionUri;
            _sqlGeneratorFunc = sqlGeneratorFunc;
        }

        public ValueBufferFactory ValueBufferFactory { get; private set; }

        public string CollectionUri => _collectionUri;

        public QuerySqlGenerator GetSqlGenerator()
        {
            return _sqlGeneratorFunc();
        }

        public SqlQuerySpec GetSqlQuerySpec(IReadOnlyDictionary<string, object> parameterValues)
        {
            var generator = _sqlGeneratorFunc();
            ValueBufferFactory = generator.CreateValueBufferFactory();
            return new SqlQuerySpec(generator.GenerateSql(parameterValues),
                new SqlParameterCollection(generator.IncludedParameters.Values));
        }
    }
}
