﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace QLBaiDoXe.Interfaces.Interpreter
{
    internal class SeparatorExpression : AbstractExpression
    {
        public void Evaluate(DateContext context)
        {
            string expression = context.expression;
            context.expression = expression.Replace(" ", "/");
        }
    }
}
