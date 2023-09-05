using AppCore.Business.Services.Bases;
using AppCore.DataAccess.EntityFramework.Bases;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess;

namespace Business.Services
{
    public interface ITagService : IService<TagModel>
    {
    }

    public class TagService : ITagService
    {
        private readonly RepoBase<Tag> _tagRepo;

        public TagService(RepoBase<Tag> tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public Result Add(TagModel model)
        {
            throw new NotImplementedException();
        }

        public Result Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _tagRepo.Dispose();
        }

        public IQueryable<TagModel> Query()
        {
            return _tagRepo.Query().OrderBy(t => t.Name).Select(t => new TagModel()
            {
                Guid = t.Guid,              
                Id = t.Id,
                IsPopular = t.IsPopular,
                Name = t.Name + " " + (t.IsPopular ? "*" : "")
            });
        }

        public Result Update(TagModel model)
        {
            throw new NotImplementedException();
        }
    }
}
