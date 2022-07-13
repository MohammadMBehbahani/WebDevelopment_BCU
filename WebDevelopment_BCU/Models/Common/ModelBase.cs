using System;

namespace WebDevelopment_BCU.Models.Common
{
    public class ModelBase<T>
    {
        public T Id { get; set; }
        public DateTime InserDate { get; set; } = DateTime.Now;
    }

    public class ModelBase : ModelBase<long> { }
}
