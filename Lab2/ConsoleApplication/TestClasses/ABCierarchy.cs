using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.TestClasses
{
    // Classes wich are needed to check the program for a cyclic dependency

    public class A
    {
       // public A helloA;
        public C C_obj { get; set; }
        public B B_obj { get; set; }

        public A(B b_obj)
        {
            B_obj = b_obj;
        }
    }

    public class B
    {
        public C C_obj { get; set; }

        public B(C c_obj)
        {
            C_obj = c_obj;
        }
    }

    public class C
    {
        public A A_obj { get; set; }

        public C(A a_obj)
        {
            A_obj = a_obj;
        }
    }
}
