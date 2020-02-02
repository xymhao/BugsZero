using System;
using System.Collections.Generic;
using System.Text;

namespace BugsZero
{
    public class ConsoleLog
    {
        public StringBuilder Builder { get; set; } = new StringBuilder();

        public void WriteLine(object des)
        {
            Console.WriteLine(des);
            Builder.Append(des);
        }

        public override string ToString()
        {
            return Builder.ToString();
        }
    }
}
