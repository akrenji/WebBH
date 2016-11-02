using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTBGD.Models
{
    public static class MyExtensions
    {
        public static IEnumerable<T> GetRandomItems<T>(this IEnumerable<T> source, Int32 count)
        {
            return source.OrderBy(s => Guid.NewGuid()).Take(count);
        }
    }
}