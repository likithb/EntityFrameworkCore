// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Update;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public interface IDocumentCollectionService
    {
        Task CreateAsync(IUpdateEntry entry);
        Task UpdateAsync(IUpdateEntry entry);
        Task DeleteAsync(IUpdateEntry entry);
    }
}
