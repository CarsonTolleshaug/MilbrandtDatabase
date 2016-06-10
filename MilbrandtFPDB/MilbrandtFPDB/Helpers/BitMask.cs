using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilbrandtFPDB
{
    public class BitMask
    {
        private List<int> _chunks;
        private const int INT_SIZE = sizeof(int) * 8; // 8 bits per byte

        public BitMask()
        {
            _chunks = new List<int>() { 0 };
        }

        public bool this[int index]
        {
            get
            {
                int i1 = index / INT_SIZE;
                int i2 = index % INT_SIZE;

                if (i1 >= _chunks.Count)
                    return false;

                int status = _chunks[i1] & (1 << i2);
                return status != 0;
            }
            set
            {
                int i1 = index / INT_SIZE;
                int i2 = index % INT_SIZE;

                while (i1 >= _chunks.Count)
                    _chunks.Add(0);

                int bitSetter = 1 << i2;

                if (value)
                {
                    // turn bit on
                    _chunks[i1] |= bitSetter;
                }
                else
                {
                    // turn bit off
                    _chunks[i1] &= ~bitSetter;
                }
            }
        }

        public int GetFirstAvailableIndex()
        {
            for (int i = 0; i < _chunks.Count; i++)
            {
                int chunk = _chunks[i];

                // we can quickly check if there are any spots available in this chunk
                // because two's complement means all 1's = -1
                if (chunk != -1)
                {
                    for (int j = 0; j < INT_SIZE; j++)
                    {
                        if ((chunk & (1 << j)) == 0)
                        {
                            return (i * INT_SIZE) + j;
                        }
                    }
                }
            }

            // every int is full, so add another one and return the first index in that chunk
            return AddChunk();
        }

        private int AddChunk()
        {
            int i = _chunks.Count;
            _chunks.Add(0);
            return i * INT_SIZE;
        }
    }
}
