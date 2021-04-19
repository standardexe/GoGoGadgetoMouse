using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoGoGadgetoMouse {
    static class Extensions {
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
    }
}
