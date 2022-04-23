using AutoMapper;
using News_Business.Repository.IRepository;
using News_DataLayer.Data;
using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public CommentRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> CreateComments(CommentDTO CommentDTO)
        {
            try
            {
                var Comment = _mapper.Map<CommentDTO,News_DataLayer.Models.Comment>(CommentDTO);
                _db.Comment.Add(Comment);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<CommentDTO>> GetCommentsByNewsId(int NewsId)
        {
            IQueryable<News_DataLayer.Models.Comment> Comment = _db.Comment.Where(c=>c.NewsId == NewsId);


            var CommentDTO = _mapper.Map<IQueryable<News_DataLayer.Models.Comment>, IEnumerable<CommentDTO>>(Comment);

           
            var query = CommentDTO.OrderByDescending(c => c.CreateDate).ToList();

            return query;
        }
    }
}
