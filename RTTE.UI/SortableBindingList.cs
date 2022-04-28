using LanguageExt;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTE.UI {
    public class SortableBindingList<T> : BindingList<T> where T : class {
        protected override bool SupportsSortingCore => true;

        private bool isSorted;
        protected override bool IsSortedCore => isSorted;

        private ListSortDirection sortDirection;
        protected override ListSortDirection SortDirectionCore => sortDirection;

        private PropertyDescriptor sortProperty;
        protected override PropertyDescriptor SortPropertyCore => sortProperty;

        public SortableBindingList() { }

        #region DONOTREMOVE
        protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction) {
            sortProperty = prop;
            sortDirection = direction;

            if (Items is not List<T> list)
                return;

            lock (this) {
                list.Sort(Compare);
            }

            isSorted = true;
            OnListChanged(new(ListChangedType.Reset, -1));
        }

        protected override void RemoveSortCore() {
            sortDirection = ListSortDirection.Ascending;
            sortProperty = null;
            isSorted = false;
        }
        #endregion

        private int Compare(T lhs, T rhs) {
            int result = OnComparison(Option<T>.Some(lhs), Option<T>.Some(rhs));
            return sortDirection != ListSortDirection.Descending ? result : -result;
        }

        private int OnComparison(Option<T> lhs, Option<T> rhs) {
            Option<object> lhsValue = lhs.Match<Option<object>>(l => sortProperty.GetValue(l), () => Option<object>.None);
            Option<object> rhsValue = rhs.Match<Option<object>>(r => sortProperty.GetValue(r), () => Option<object>.None);

            return lhsValue.Match(
                l => rhsValue.Match(r =>
                    l is IComparable comparable ?
                        comparable.CompareTo(r) :
                        !l.Equals(r) ? l.ToString().CompareTo(r.ToString()) : 0
                , 1),
                () => rhsValue.Match(_ => -1, 0)
            );
        }
    }
}
