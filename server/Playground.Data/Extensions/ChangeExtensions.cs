using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using Playground.Core;
using Playground.Core.ApiQuery;
using Playground.Data.Entities;

namespace Playground.Data.Extensions
{
    public static class ChangeExtensions
    {
        static IQueryable<Change> Search(this IQueryable<Change> changes, string search) =>
            changes.Where(x => x.Id.ToString().ToLower().Contains(search.ToLower()));

        public static async Task<QueryResult<Change>> QueryChanges(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        ) {
            var container = new QueryContainer<Change>(
                db.Changes,
                page, pageSize, search, sort
            );

            return await container.Query((changes, s) => changes.Search(s));
        }

        public static async Task<Change> GetChange(this AppDbContext db, int id) =>
            await db.Changes
                .FindAsync(id);

        public static async Task AddChange(this AppDbContext db, Change change)
        {
            if (await change.Validate(db))
            {
                await db.Changes.AddAsync(change);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateChange(this AppDbContext db, Change change)
        {
            if (await change.Validate(db))
            {
                db.Changes.Update(change);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveChange(this AppDbContext db, Change change)
        {
            db.Changes.Remove(change);
            await db.SaveChangesAsync();
        }

        static async Task<bool> Validate(this Change change, AppDbContext db)
        {
            // if (string.IsNullOrEmpty(change.Value))
            // {
            //     throw new AppException("Change must have a Id", ExceptionType.Validation);
            // }

            var check = await db.Changes
                .FirstOrDefaultAsync(x =>
                    x.Id != change.Id
                    // x.Value.ToLower() == change.Value.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{change.Id} is already a Change", ExceptionType.Validation);
            }

            return true;
        }


    }
}