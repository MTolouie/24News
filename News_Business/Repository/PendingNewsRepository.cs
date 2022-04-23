using AutoMapper;
using Microsoft.EntityFrameworkCore;
using News_Business.Repository.IRepository;
using News_DataLayer.Data;
using News_DataLayer.Models;
using News_Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace News_Business.Repository
{
    public class PendingNewsRepository : IPendingNewsRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public PendingNewsRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<bool> DeletePendingNews(int NewsId, string UserId)
        {
            try
            {
                var pending = await _db.PendingNews.SingleOrDefaultAsync(p => p.NewsId == NewsId && p.UserId == UserId);
                if (pending == null)
                {
                    return false;
                }
                _db.PendingNews.Remove(pending);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public PendingNewsDTO GetPendingNews(int NewsId, string UserId)
        {
            try
            {
                var pending =  _db.PendingNews.SingleOrDefault(p=>p.NewsId == NewsId && p.UserId == UserId);
                if (pending == null)
                {
                    return null;
                }
                var pendngDTO = _mapper.Map<PendingNews,PendingNewsDTO>(pending);
                return pendngDTO;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public PendingNewsDTO GetUserPendingNews(string UserId)
        {
            try
            {
                var pending = _db.PendingNews.SingleOrDefault(p=> p.UserId == UserId);
                if (pending == null)
                {
                    return null;
                }
                var pendngDTO = _mapper.Map<PendingNews, PendingNewsDTO>(pending);
                return pendngDTO;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> SendToPendingNews(PendingNewsDTO PendingDTO)
        {
            try
            {
                var pending = _mapper.Map<PendingNewsDTO,PendingNews>(PendingDTO);
                _db.PendingNews.Add(pending);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
