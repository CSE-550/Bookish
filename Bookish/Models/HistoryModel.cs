using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bookish.Models
{
    public class HistoryModel
    {
        public List<PostListModel> Posts { get; set; }
        public List<CommentModel> Comments { get; set; }

        public List<IListItem> GetItems()
        {
            List<IListItem> items = new List<IListItem>();
            items.AddRange(Posts);
            items.AddRange(Comments);
            return items.OrderByDescending(i => i.GetCreatedDate()).ToList();
        }
    }
}
