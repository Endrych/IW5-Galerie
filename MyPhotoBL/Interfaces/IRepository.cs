using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhotoBL.Interfaces
{
    interface IRepository<T>
    {
        T GetById(Guid id);
        T Insert(T item);
        void Update(T item);
        void Remove(Guid id);
    }
}
