// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class DocumentDbClientService : IDocumentDbClientService
    {
        private static readonly string _remoteEndpoint = "https://localhost:8081";
        private static readonly string _remoteKey
            = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";
        private readonly DocumentClient _documentClient;
        private readonly string _databaseId;
        private readonly IDiagnosticsLogger<DbLoggerCategory.Database.Command> _logger;
        private readonly IDocumentCollectionServiceFactory _documentCollectionServiceFactory;
        private Dictionary<IEntityType, IDocumentCollectionService> _collections
            = new Dictionary<IEntityType, IDocumentCollectionService>();

        public DocumentDbClientService(IDiagnosticsLogger<DbLoggerCategory.Database.Command> logger,
            IDbContextOptions dbContextOptions,
            IDocumentCollectionServiceFactory documentCollectionServiceFactory)
        {
            _databaseId = dbContextOptions.Extensions.OfType<DocumentDbOptionsExtension>().Single().StoreName;
            _documentClient = new DocumentClient(new Uri(_remoteEndpoint), _remoteKey);
            _logger = logger;
            _documentCollectionServiceFactory = documentCollectionServiceFactory;
        }

        public DocumentClient Client => _documentClient;

        public string DatabaseId => _databaseId;

        public IEnumerator<Document> ExecuteQuery(
            string collectionId,
            SqlQuerySpec sqlQuerySpec)
        {
            _logger.CommandExecuting(sqlQuerySpec);

            return _documentClient.CreateDocumentQuery<Document>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, collectionId),
                sqlQuerySpec,
                new FeedOptions()
                {
                    EnableCrossPartitionQuery = true,
                    EnableScanInQuery = true
                })
                .GetEnumerator();
        }

        public async Task<int> SaveChangesAsync(IReadOnlyList<IUpdateEntry> entries, CancellationToken cancellationToken)
        {
            var rowsAffected = 0;

            foreach (var entry in entries)
            {
                var entityType = entry.EntityType;

                if (!_collections.TryGetValue(entityType, out var documentCollectionService))
                {
                    _collections.Add(
                        entityType,
                        documentCollectionService = _documentCollectionServiceFactory.Create(this, entityType));
                }

                switch (entry.EntityState)
                {
                    case EntityState.Added:
                        await documentCollectionService.CreateAsync(entry);
                        break;

                    case EntityState.Modified:
                        await documentCollectionService.UpdateAsync(entry);
                        break;

                    case EntityState.Deleted:
                        await documentCollectionService.DeleteAsync(entry);
                        break;
                }

                rowsAffected++;
            }

            return rowsAffected;
        }
    }
}
