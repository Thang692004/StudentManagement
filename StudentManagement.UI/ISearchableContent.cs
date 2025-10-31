using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement.UI
{
    public interface  ISearchableContent
    {
        // Nhận chuỗi tìm kiếm và thực hiện lọc hoặc tìm kiếm nội dung tương ứng
        void PerformSearch(string searchText);
    }
}
