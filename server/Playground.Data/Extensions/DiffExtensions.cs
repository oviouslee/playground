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
    public static class DiffExtensions
    {
        static IQueryable<Diff> Search(this IQueryable<Diff> diffs, string search) =>
            diffs.Where(x => x.Previous.ToLower().Contains(search.ToLower()));

        public static async Task<QueryResult<Diff>> QueryDiffs(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        ) {
            var container = new QueryContainer<Diff>(
                db.Diffs,
                page, pageSize, search, sort
            );

            return await container.Query((diffs, s) => diffs.Search(s));
        }

        public static async Task<Diff> GetDiff(this AppDbContext db, int id) =>
            await db.Diffs
                .FindAsync(id);

        public static async Task AddDiff(this AppDbContext db, Diff diff)
        {
            if (await diff.Validate(db))
            {
                await db.Diffs.AddAsync(diff);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateDiff(this AppDbContext db, Diff diff)
        {
            if (await diff.Validate(db))
            {
                db.Diffs.Update(diff);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveDiff(this AppDbContext db, Diff diff)
        {
            db.Diffs.Remove(diff);
            await db.SaveChangesAsync();
        }

        static async Task<bool> Validate(this Diff diff, AppDbContext db)
        {
            if (string.IsNullOrEmpty(diff.Previous))
            {
                throw new AppException("Diff must have a previous", ExceptionType.Validation);
            }

            var check = await db.Diffs
                .FirstOrDefaultAsync(x =>
                    x.Id != diff.Id &&
                    x.Previous.ToLower() == diff.Previous.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{diff.Previous} is already a Diff", ExceptionType.Validation);
            }

            return true;
        }


    }
}