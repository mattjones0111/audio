namespace Domain.AudioItem
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionOfCategories
    {
        readonly List<string> list;

        public CollectionOfCategories(List<string> categories)
        {
            list = categories;
        }

        public void Add(string category)
        {
            if(!list.Contains(
                category,
                StringComparer.InvariantCultureIgnoreCase))
            {
                list.Add(category);
            }
        }
    }
}
