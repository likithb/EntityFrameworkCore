// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class DocumentCollectionService<TKey> : IDocumentCollectionService
    {
        private readonly DocumentClient _documentClient;
        private readonly string _databaseId;
        private readonly IEntityType _entityType;
        private IPrincipalKeyValueFactory<TKey> _principalKeyValueFactory;

        public DocumentCollectionService(
            DocumentClient documentClient,
            string databaseId,
            IEntityType entityType,
            IPrincipalKeyValueFactory<TKey> principalKeyValueFactory)
        {
            _documentClient = documentClient;
            _databaseId = databaseId;
            _entityType = entityType;
            _principalKeyValueFactory = principalKeyValueFactory;
        }

        public async Task CreateAsync(IUpdateEntry entry)
        {
            var document = new Document
            {
                Id = GetId(entry)
            };
            foreach (var property in _entityType.GetProperties())
            {
                document.SetPropertyValue(property.Name, entry.GetCurrentValue(property));
            }

            try
            {
                await _documentClient.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(
                        _databaseId, _entityType.DocumentDb().CollectionName), document);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteAsync(IUpdateEntry entry)
        {
            try
            {
                await _documentClient.DeleteDocumentAsync(
                    UriFactory.CreateDocumentUri(
                        _databaseId, _entityType.DocumentDb().CollectionName, GetId(entry)));
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private string GetId(IUpdateEntry entry)
        {
            var key = _principalKeyValueFactory.CreateFromCurrentValues((InternalEntityEntry)entry);

            // TODO: Escape | Bar|string.Empty
            return key is object[] array ? string.Join("|", array) : key.ToString();
        }

        public async Task UpdateAsync(IUpdateEntry entry)
        {
            var document = new Document
            {
                Id = GetId(entry)
            };
            foreach (var property in _entityType.GetProperties())
            {
                document.SetPropertyValue(property.Name, entry.GetCurrentValue(property));
            }

            try
            {
                await _documentClient.ReplaceDocumentAsync(
                    UriFactory.CreateDocumentUri(
                        _databaseId, _entityType.DocumentDb().CollectionName, document.Id), document);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }



}
