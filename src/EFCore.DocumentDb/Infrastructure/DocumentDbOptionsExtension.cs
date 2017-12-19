// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Text;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class DocumentDbOptionsExtension : IDbContextOptionsExtension
    {
        private string _database;
        private string _logFragment;

        public DocumentDbOptionsExtension()
        {
        }

        protected DocumentDbOptionsExtension(DocumentDbOptionsExtension copyFrom)
        {
            _database = copyFrom._database;
        }

        protected virtual DocumentDbOptionsExtension Clone() => new DocumentDbOptionsExtension(this);

        public virtual string StoreName => _database;

        public virtual DocumentDbOptionsExtension WithStoreName(string database)
        {
            var clone = Clone();

            clone._database = database;

            return clone;
        }

        public bool ApplyServices(IServiceCollection services)
        {
            services.AddEntityFrameworkDocumentDb();

            return true;
        }

        public long GetServiceProviderHashCode()
        {
            return 0;
        }

        public void Validate(IDbContextOptions options)
        {
        }

        public string LogFragment
        {
            get
            {
                if (_logFragment == null)
                {
                    var builder = new StringBuilder();

                    builder.Append("Database=").Append(_database).Append(' ');

                    _logFragment = builder.ToString();
                }

                return _logFragment;
            }
        }
    }
}
