﻿using CafeLibrary.DataAccess.Data;


namespace CafeLibrary.DataAccess.Repository.IRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository CategoryRepository { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            CategoryRepository = new CategoryRepository(_context);
        }        

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
