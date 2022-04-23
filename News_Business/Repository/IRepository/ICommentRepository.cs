using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository.IRepository
{
    public interface ICommentRepository 
    {
        public Task<List<CommentDTO>> GetCommentsByNewsId(int NewsId);
        public Task<bool> CreateComments(CommentDTO CommentDTO);
    }
}
