using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperServer.Utils
{
    //값이 더 작은것이 우선순위가 높은 PQ
    public class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _datas = new List<T>();
        public int Count { get { return _datas.Count; } }

        public void Push(T data)
        {
            _datas.Add(data);

            int currentIdx = _datas.Count -1;

            while (currentIdx > 0)
            {
                int parentIdx = (currentIdx - 1) / 2;

                if (_datas[currentIdx].CompareTo(_datas[parentIdx]) >= 0)
                    break;

                T temp = _datas[currentIdx];
                _datas[currentIdx] = _datas[parentIdx];
                _datas[parentIdx] = temp;

                currentIdx = parentIdx;
            }
        }

        public T Pop()
        {
            T ret = _datas[0];

            int lastIdx = _datas.Count -1;
            _datas[0] = _datas[lastIdx];

            _datas.RemoveAt(lastIdx);
            lastIdx--;

            int currentIdx = 0;
            while (true)
            {
                int leftIdx = (currentIdx * 2) + 1;
                int rightIdx = (currentIdx * 2) + 2;

                int nextIdx = currentIdx;

                if (leftIdx <= lastIdx && _datas[nextIdx].CompareTo(_datas[leftIdx]) >= 0)
                    nextIdx = leftIdx;
                if (rightIdx <= lastIdx && _datas[nextIdx].CompareTo(_datas[rightIdx]) >= 0)
                    nextIdx = rightIdx;

                //그대로라면 (현상유지)
                if (nextIdx == currentIdx)
                    break;

                T temp = _datas[currentIdx];
                _datas[currentIdx] = _datas[nextIdx];
                _datas[nextIdx] = temp;

                currentIdx = nextIdx;
            }    

            return ret;
        }

        public T Peek()
        {
            if (_datas.Count == 0)
                return default(T);
            return _datas[0];
        }
    }
}
