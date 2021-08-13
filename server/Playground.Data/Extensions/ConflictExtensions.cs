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
    public static class ConflictExtensions
    {
        static IQueryable<Conflict> Search(this IQueryable<Conflict> conflicts, string search) =>
            conflicts.Where(x => x.Id.ToString().ToLower().Contains(search.ToLower()));

        public static async Task<QueryResult<Conflict>> QueryConflicts(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        ) {
            var container = new QueryContainer<Conflict>(
                db.Conflicts,
                page, pageSize, search, sort
            );

            return await container.Query((conflicts, s) => conflicts.Search(s));
        }

        public static async Task<Conflict> GetConflict(this AppDbContext db, int id) =>
            await db.Conflicts
                .FindAsync(id);

        public static async Task AddConflict(this AppDbContext db, Conflict conflict)
        {
            if (await conflict.Validate(db))
            {
                await db.Conflicts.AddAsync(conflict);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateConflict(this AppDbContext db, Conflict conflict)
        {
            if (await conflict.Validate(db))
            {
                db.Conflicts.Update(conflict);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveConflict(this AppDbContext db, Conflict conflict)
        {
            db.Conflicts.Remove(conflict);
            await db.SaveChangesAsync();
        }

        static async Task<bool> Validate(this Conflict conflict, AppDbContext db)
        {
            // if (string.IsNullOrEmpty(conflict.Value))
            // {
            //     throw new AppException("Conflict must have a Value", ExceptionType.Validation);
            // }

            var check = await db.Conflicts
                .FirstOrDefaultAsync(x =>
                    x.Id != conflict.Id
                    // x.Value.ToLower() == conflict.Value.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{conflict.Id} is already a Conflict", ExceptionType.Validation);
            }

            return true;
        }


    }
}