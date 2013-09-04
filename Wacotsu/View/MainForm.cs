using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wacotsu.View
{
    public partial class MainForm : Form
    {
        private ViewModel.LiveListViewModel viewModel;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.viewModel = new ViewModel.LiveListViewModel(this.liveListView);
            var live1 = new Model.Live { Id = "lv20", Title = "liveA", ReservedStatus = Model.ReserveStatus.NotReserved };
            for (var i = 0; i < 20; i++) {
                this.viewModel.AddLive(new Model.Live { Id = "lv20202020", Title = "生放送A", ReservedStatus = Model.ReserveStatus.NotReserved });
            }
        }

        private void liveListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // TODO: 予約・取り消し・座席を開くアクション
        }
    }
}
