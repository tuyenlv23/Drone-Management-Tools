using System;
using System.Linq;
using System.Windows.Forms;

namespace Drone_Management_Tools.Utilities
{
    public class TreeViewNodeSorter : System.Collections.IComparer
    {
        public TreeViewNodeSorter() { }

        public int Compare(object x, object y)
        {
            try
            {
                TreeNode tx = x as TreeNode;
                TreeNode ty = y as TreeNode;

                var _txNum = Convert.ToInt32(tx.Text.Substring(1, tx.Text.IndexOf("]") - 1));
                var _tyNum = Convert.ToInt32(ty.Text.Substring(1, ty.Text.IndexOf("]") - 1));

                return _txNum - _tyNum;
            }
            catch
            {
                return 1000;
            }
        }
    }
}
