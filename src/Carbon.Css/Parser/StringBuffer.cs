﻿using System.Text;

namespace Carbon.Css.Parser
{
    public class StringBuffer
    {
        private readonly StringBuilder sb = new StringBuilder();

        public int Length => sb.Length;

        public void Append(char c)
        {
            sb.Append(c);
        }

        /// <summary>
        /// Extracts the buffered value and resets the buffer
        /// </summary>
        public string Extract()
        {
            var value = sb.ToString();

            Reset();

            return value;
        }

        public void Reset()
        {
            sb.Clear();
        }
    }
}