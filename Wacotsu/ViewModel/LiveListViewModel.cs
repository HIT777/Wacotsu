using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows.Forms;

namespace Wacotsu.ViewModel
{
    internal class LiveListViewModel
    {
        /// <summary>
        /// 放送情報を表示するリストビュー
        /// </summary>
        private ListView listView;

        /// <summary>
        /// 放送情報リスト
        /// </summary>
        private ObservableCollection<Model.Live> lives;

        /// <summary>
        /// IListを放送情報に変換する関数
        /// </summary>
        private static readonly Func<System.Collections.IList, IEnumerable<Model.Live>> CastChangedCollection = (list) => list.Cast<Model.Live>();

        /// <summary>
        /// 放送情報を元にリストビューに表示するアイテムを作る関数
        /// </summary>
        private static readonly Func<Model.Live, ListViewItem> CreateListViewItem = (live) => new ListViewItem(live.Title) { Tag = live };

        /// <summary>
        /// リストビューのアイテムのうち指定した条件に一致するものを削除する
        /// </summary>
        private static readonly Action<ListView.ListViewItemCollection, Func<ListViewItem, bool>> RemoveListViewItemsBy = (items, pred) => {
            foreach (var item in items.Cast<ListViewItem>().Where(pred)) {
                items.Remove(item);
            }
        };

        internal LiveListViewModel(ListView listView)
        {
            this.listView = listView;
            this.lives = new ObservableCollection<Model.Live>();
            this.lives.CollectionChanged += lives_CollectionChanged;
        }

        internal void AddLive(Model.Live live)
        {
            lives.Add(live);
        }

        internal void RemoveLive(Model.Live live)
        {
            lives.Remove(live);
        }

        internal void ReserveSelectedLives()
        {

        }

        private void lives_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            listView.Invoke((MethodInvoker)delegate() {
                // リストビューから削除・置換された放送情報を削除する
                RemoveListViewItemsBy(listView.Items, (item) => CastChangedCollection(e.OldItems).Contains((Model.Live)item.Tag));

                // リストビューに新規追加された放送情報を追加する
                listView.Items.AddRange(CastChangedCollection(e.NewItems).Select(CreateListViewItem).ToArray());
            });
        }
    }
}
