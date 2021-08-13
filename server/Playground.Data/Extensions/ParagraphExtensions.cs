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
    public static class ParagraphExtensions
    {
        static IQueryable<Paragraph> Search(this IQueryable<Paragraph> paragraphs, string search) =>
            paragraphs.Where(x => x.Value.ToLower().Contains(search.ToLower()));

        public static async Task<QueryResult<Paragraph>> QueryParagraphs(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        ) {
            var container = new QueryContainer<Paragraph>(
                db.Paragraphs,
                page, pageSize, search, sort
            );

            return await container.Query((paragraphs, s) => paragraphs.Search(s));
        }

        public static async Task<Paragraph> GetParagraph(this AppDbContext db, int id) =>
            await db.Paragraphs
                .FindAsync(id);

        public static async Task AddParagraph(this AppDbContext db, Paragraph paragraph)
        {
            if (await paragraph.Validate(db))
            {
                await db.Paragraphs.AddAsync(paragraph);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateParagraph(this AppDbContext db, Paragraph paragraph)
        {
            if (await paragraph.Validate(db))
            {
                db.Paragraphs.Update(paragraph);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveParagraph(this AppDbContext db, Paragraph paragraph)
        {
            db.Paragraphs.Remove(paragraph);
            await db.SaveChangesAsync();
        }

        static async Task<bool> Validate(this Paragraph paragraph, AppDbContext db)
        {
            if (string.IsNullOrEmpty(paragraph.Value))
            {
                throw new AppException("Paragraph must have a Value", ExceptionType.Validation);
            }

            var check = await db.Paragraphs
                .FirstOrDefaultAsync(x =>
                    x.Id != paragraph.Id &&
                    x.Value.ToLower() == paragraph.Value.ToLower()
                );

            if (check != null)
            {
                throw new AppException($"{paragraph.Value} is already a Paragraph", ExceptionType.Validation);
            }

            return true;
        }


    }
}