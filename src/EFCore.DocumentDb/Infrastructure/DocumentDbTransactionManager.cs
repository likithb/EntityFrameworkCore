// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class DocumentDbTransactionManager : IDbContextTransactionManager
    {
        private static readonly DocumentDbTransaction _stubTransaction = new DocumentDbTransaction();
        public IDbContextTransaction CurrentTransaction => null;

        public Transaction EnlistedTransaction => null;

        public IDbContextTransaction BeginTransaction()
        {
            return _stubTransaction;
        }

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IDbContextTransaction>(_stubTransaction);
        }

        public void CommitTransaction()
        {
        }

        public void EnlistTransaction(Transaction transaction)
        {
        }

        public void ResetState()
        {
        }

        public void RollbackTransaction()
        {
        }
    }



}
