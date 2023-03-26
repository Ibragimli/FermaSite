using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ferma.Service.Helper
{
    public class PagenetedList<T> : List<T>
    {
        public PagenetedList(List<T> items, int count, int pageindex, int pagesize)
        {
            this.AddRange(items);
            PageIndex = pageindex;
            TotalPages = (int)(Math.Ceiling(count / (double)pagesize));
        }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrev
        {
            get => PageIndex > 1;
        }

        public bool HasNext
        {
            get => TotalPages > PageIndex;
        }
        public static PagenetedList<T> Create(IQueryable<T> query, int pageindex, int pagesize)
        {
            var items = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            return new PagenetedList<T>(items, query.Count(), pageindex, pagesize);
        }
        public static PagenetedList<T> CreateRandom(IQueryable<T> query, int pageindex, int pagesize)
        {
            var items = new List<T>();
            if (query.Count() > pagesize)
            {
                var rand = new Random();
                var index = rand.Next(0, pagesize);
                items = query.Skip(index).Take(pagesize).ToList();
            }
            else
            {
                 items = query.Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            }

            return new PagenetedList<T>(items, query.Count(), pageindex, pagesize);
        }
    }
}
