using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ITagsRepository : IRepository<Tag> {
        Task<Tag> GetTagByTagName(string tagName);
    }
    public class TagsRepository : ITagsRepository
    {
        private CinemaContext _cinemaContext;

        public TagsRepository( CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Tag Delete(object id)
        {
            Tag existing = _cinemaContext.Tags.Find(id);
            var result = _cinemaContext.Tags.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Tag>> GetAll()
        {
            var data = await _cinemaContext.Tags.ToListAsync();

            return data;
        }

        public async Task<Tag> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Tags.FindAsync(id);

            return data;
        }

        public Tag Insert(Tag obj)
        {
            return _cinemaContext.Tags.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Tag Update(Tag obj)
        {
            var updatedEntry = _cinemaContext.Tags.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<Tag> GetTagByTagName(string tagName)
        {
            var data = _cinemaContext.Tags.Where(x => x.Name == tagName).FirstOrDefault();

            return data;
            
        }
    }
}
