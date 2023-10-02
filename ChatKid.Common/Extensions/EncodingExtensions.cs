using System.Globalization;
using System.Text;

namespace ChatKid.Common.Extensions
{
    public class BitStream : Stream
    {
        private byte[] Source { get; set; }

        /// <summary>
        /// Initialize the stream with capacity
        /// </summary>
        /// <param name="capacity">Capacity of the stream</param>
        public BitStream(int capacity)
        {
            this.Source = new byte[capacity];
        }

        /// <summary>
        /// Initialize the stream with a source byte array
        /// </summary>
        /// <param name="source"></param>
        public BitStream(byte[] source)
        {
            this.Source = source;
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Bit length of the stream
        /// </summary>
        public override long Length
        {
            get { return Source.Length * 8; }
        }

        /// <summary>
        /// Bit position of the stream
        /// </summary>
        public override long Position { get; set; }

        /// <summary>
        /// Read the stream to the buffer
        /// </summary>
        /// <param name="buffer">Buffer</param>
        /// <param name="offset">Offset bit start position of the stream</param>
        /// <param name="count">Number of bits to read</param>
        /// <returns>Number of bits read</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            // Temporary position cursor
            long tempPos = this.Position;
            tempPos += offset;

            // Buffer byte position and in-byte position
            int readPosCount = 0, readPosMod = 0;

            // Stream byte position and in-byte position
            long posCount = tempPos >> 3;
            int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

            while (tempPos < this.Position + offset + count && tempPos < this.Length)
            {
                // Copy the bit from the stream to buffer
                if ((((int)this.Source[posCount]) & (0x1 << (7 - posMod))) != 0)
                {
                    buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) | (0x1 << (7 - readPosMod)));
                }
                else
                {
                    buffer[readPosCount] = (byte)((int)(buffer[readPosCount]) & (0xffffffff - (0x1 << (7 - readPosMod))));
                }

                // Increment position cursors
                tempPos++;
                if (posMod == 7)
                {
                    posMod = 0;
                    posCount++;
                }
                else
                {
                    posMod++;
                }
                if (readPosMod == 7)
                {
                    readPosMod = 0;
                    readPosCount++;
                }
                else
                {
                    readPosMod++;
                }
            }
            int bits = (int)(tempPos - this.Position - offset);
            this.Position = tempPos;
            return bits;
        }

        /// <summary>
        /// Set up the stream position
        /// </summary>
        /// <param name="offset">Position</param>
        /// <param name="origin">Position origin</param>
        /// <returns>Position after setup</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case (SeekOrigin.Begin):
                    {
                        this.Position = offset;
                        break;
                    }
                case (SeekOrigin.Current):
                    {
                        this.Position += offset;
                        break;
                    }
                case (SeekOrigin.End):
                    {
                        this.Position = this.Length + offset;
                        break;
                    }
            }
            return this.Position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write from buffer to the stream
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset">Offset start bit position of buffer</param>
        /// <param name="count">Number of bits</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            // Temporary position cursor
            long tempPos = this.Position;

            // Buffer byte position and in-byte position
            int readPosCount = offset >> 3, readPosMod = offset - ((offset >> 3) << 3);

            // Stream byte position and in-byte position
            long posCount = tempPos >> 3;
            int posMod = (int)(tempPos - ((tempPos >> 3) << 3));

            while (tempPos < this.Position + count && tempPos < this.Length)
            {
                // Copy the bit from buffer to the stream
                if ((((int)buffer[readPosCount]) & (0x1 << (7 - readPosMod))) != 0)
                {
                    this.Source[posCount] = (byte)((int)(this.Source[posCount]) | (0x1 << (7 - posMod)));
                }
                else
                {
                    this.Source[posCount] = (byte)((int)(this.Source[posCount]) & (0xffffffff - (0x1 << (7 - posMod))));
                }

                // Increment position cursors
                tempPos++;
                if (posMod == 7)
                {
                    posMod = 0;
                    posCount++;
                }
                else
                {
                    posMod++;
                }
                if (readPosMod == 7)
                {
                    readPosMod = 0;
                    readPosCount++;
                }
                else
                {
                    readPosMod++;
                }
            }
            this.Position = tempPos;
        }
    }

    public static class EncodingExtensions
    {
        private const string DefaultCharacterSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private const string InvertedCharacterSet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Encode a 2-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this short original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a 4-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this int original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a 8-byte number with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this long original, bool inverted = false)
        {
            var array = BitConverter.GetBytes(original);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(array);
            }

            return array.ToBase62(inverted);
        }

        /// <summary>
        /// Encode a string with Base62
        /// </summary>
        /// <param name="original">String</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this string original, bool inverted = false)
        {
            return Encoding.UTF8.GetBytes(original).ToBase62(inverted);
        }

        /// <summary>
        /// Encode a byte array with Base62
        /// </summary>
        /// <param name="original">Byte array</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Base62 string</returns>
        public static string ToBase62(this byte[] original, bool inverted = false)
        {
            var characterSet = inverted ? InvertedCharacterSet : DefaultCharacterSet;
            var arr = Array.ConvertAll(original, t => (int)t);

            var converted = BaseConvert(arr, 256, 62);
            var builder = new StringBuilder();
            foreach (var t in converted)
            {
                builder.Append(characterSet[t]);
            }
            return builder.ToString();
        }

        /// <summary>
        /// Decode a base62-encoded string
        /// </summary>
        /// <param name="base62">Base62 string</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Byte array</returns>
        public static T FromBase62<T>(this string base62, bool inverted = false)
        {
            var array = base62.FromBase62(inverted);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.String:
                    return (T)Convert.ChangeType(Encoding.UTF8.GetString(array), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int16:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt16(array, 0), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int32:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt32(array, 0), typeof(T), CultureInfo.InvariantCulture);
                case TypeCode.Int64:
                    if (BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(array);
                    }

                    return (T)Convert.ChangeType(BitConverter.ToInt64(array, 0), typeof(T), CultureInfo.InvariantCulture);
                default:
                    throw new Exception($"Type of {typeof(T)} does not support.");
            }
        }

        /// <summary>
        /// Decode a base62-encoded string
        /// </summary>
        /// <param name="base62">Base62 string</param>
        /// <param name="inverted">Use inverted character set</param>
        /// <returns>Byte array</returns>
        public static byte[] FromBase62(this string base62, bool inverted = false)
        {
            if (string.IsNullOrWhiteSpace(base62))
            {
                throw new ArgumentNullException(nameof(base62));
            }

            var characterSet = inverted ? InvertedCharacterSet : DefaultCharacterSet;
            var arr = Array.ConvertAll(base62.ToCharArray(), characterSet.IndexOf);

            var converted = BaseConvert(arr, 62, 256);
            return Array.ConvertAll(converted, Convert.ToByte);
        }

        private static int[] BaseConvert(int[] source, int sourceBase, int targetBase)
        {
            var result = new List<int>();
            var leadingZeroCount = Math.Min(source.TakeWhile(x => x == 0).Count(), source.Length - 1);
            int count;
            while ((count = source.Length) > 0)
            {
                var quotient = new List<int>();
                var remainder = 0;
                for (var i = 0; i != count; i++)
                {
                    var accumulator = source[i] + remainder * sourceBase;
                    var digit = accumulator / targetBase;
                    remainder = accumulator % targetBase;
                    if (quotient.Count > 0 || digit > 0)
                    {
                        quotient.Add(digit);
                    }
                }

                result.Insert(0, remainder);
                source = quotient.ToArray();
            }
            result.InsertRange(0, Enumerable.Repeat(0, leadingZeroCount));
            return result.ToArray();
        }
        public static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace('+', '-').Replace('/', '_');
        }
    }
}

