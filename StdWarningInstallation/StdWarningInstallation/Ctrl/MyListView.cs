using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace StdWarningInstallation.Ctrl
{
    /// <summary>
    /// 이 클래스는 체크박스와 더블클릭이 동시에 사용되는 경우를 위해서 만든 것으로,
    /// 반드시 첫번째 컬럼은 텍스트 없이 체크박스만 지정되어야 한다.
    /// 또한 첫번째 컬럼의 너비는 고정 너비로 변경 불가능하다.
    /// </summary>
    public class MyListView : ListView
    {
        private bool isDoubleClick = false;

        protected override void OnColumnWidthChanging(ColumnWidthChangingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                e.Cancel = true;
            }
            else
            {
                base.OnColumnWidthChanging(e);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.X <= this.Columns[0].Width)
            {
                this.isDoubleClick = false;
                return;
            }

            if (e.Clicks >= 2)
            {
                this.isDoubleClick = true;
            }
            else
            {
                this.isDoubleClick = false;
            }
        }
        protected override void OnItemCheck(ItemCheckEventArgs ice)
        {
            if (this.isDoubleClick)
            {
                if (ice.NewValue == CheckState.Checked)
                {
                    ice.NewValue = CheckState.Unchecked;
                }
                else
                {
                    ice.NewValue = CheckState.Checked;
                }
            }

            // 該当アイテムの状態が変更されるか確認
            if ((this.Items[ice.Index].Checked && ice.NewValue == CheckState.Unchecked)
            || (this.Items[ice.Index].Checked == false && ice.NewValue == CheckState.Checked))
            {
                base.OnItemCheck(ice);
            }
        }
        protected override void OnDoubleClick(EventArgs e)
        {
            System.Drawing.Point clientPos = PointToClient(Cursor.Position);
            if (clientPos.X < this.Columns[0].Width)
            {
                this.isDoubleClick = false;
                return;
            }
            this.isDoubleClick = false;

            base.OnDoubleClick(e);
        }
    }
}

