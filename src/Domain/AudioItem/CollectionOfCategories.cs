namespace Domain.AudioItem
{
    using System.Collections.Generic;
    using System.Linq;

    public class CollectionOfCategories
    {
        readonly List<string> list;

        public CollectionOfCategories(IEnumerable<string> categories)
        {
            list = categories.ToList();
        }

        public void Add(string category)
        {
        }
    }
}