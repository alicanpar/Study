﻿using Study.DataAccess.IRepository;
using Study.DataAccess.Repository.IRepository;
using Study.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Study.DataAccess.Repository
{
    internal class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Company = new CompanyRepository(_db);
            Products = new ProductRepository(_db);
            ProductCategory = new ProductCategoryRepository(_db);
        }
        public ICompanyRepository Company { get; private set; }
        public IProductCategoryRepository ProductCategory { get; private set; }
        public IProductRepository Products { get; private set; }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
