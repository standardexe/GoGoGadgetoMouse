using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGoGadgetoMouse {
    static class Extensions {
        public static IEnumerable<T> ToEnumerable<T>(this System.Windows.Forms.ListBox.ObjectCollection collection) {
            if (collection == null) yield break;
            foreach (var item in collection) {
                yield return (T)item;
            }
        }

        public static IEnumerable<string> ToEnumerable(this StringCollection collection) {
            if (collection == null) yield break;
            foreach (var item in collection) {
                yield return item;
            }
        }

        public static bool TryFirstOrDefault<T,U>(
            this Dictionary<T,U> source,
            Func<KeyValuePair<T,U>, bool> selector,
            out KeyValuePair<T,U> kvp) {
            
            using (var iterator = source.GetEnumerator()) {
                while (iterator.MoveNext()) {
                    if (selector(iterator.Current)) {
                        kvp = iterator.Current;
                        return true;
                    }
                }
                kvp = default;
                return false;
            }
        }

        public static Point Add(this Point pt, int dx, int dy) {
            return new Point(pt.X + dx, pt.Y + dy);
        }
    }
}
