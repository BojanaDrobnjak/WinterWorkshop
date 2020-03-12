using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface ITagService
    {
        Task<TagsDomainModel> GetTagByIdAsync(int id);

        Task<IEnumerable<TagsDomainModel>> GetAllAsync();

        Task<TagsDomainModel> AddTag(TagsDomainModel newTag);

        Task<TagsDomainModel> DeleteTag(int id);

        Task<TagsDomainModel> GetTagByTagName(string tagName);
    }
}
