﻿using Study.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Study.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
}
