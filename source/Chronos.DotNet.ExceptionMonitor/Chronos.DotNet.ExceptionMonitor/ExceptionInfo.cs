using System;
using System.Collections.Generic;
using Chronos.DotNet.BasicProfiler;
using Chronos.Model;

namespace Chronos.DotNet.ExceptionMonitor
{
    [Serializable]
    public sealed class ExceptionInfo : UnitBase
    {
        private IClassCollection _classes;
        private IFunctionCollection _functions;

        public ExceptionInfo(ExceptionNativeInfo queryInfo, IClassCollection classes, IFunctionCollection functions)
            : base(queryInfo)
        {
            SetDependencies(classes, functions);
        }

        private ExceptionNativeInfo ExceptionNativeInfo
        {
            get { return (ExceptionNativeInfo)NativeUnit; }
        }

        public ClassInfo ExceptionClass
        {
            get
            {
                ClassInfo exceptionClass = _classes[ExceptionNativeInfo.ClassId, BeginLifetime];
                return exceptionClass;
            }
        }

        public string Message
        {
            get { return ExceptionNativeInfo.Message; }
        }

        public IEnumerable<FunctionInfo> Stack
        {
            get
            {
                ulong[] stack = ExceptionNativeInfo.Stack;
                FunctionInfo[] functions = new FunctionInfo[stack.Length];
                for (int i = 0; i < stack.Length; i++)
                {
                    ulong functionId = stack[i];
                    functions[i] = _functions[functionId, BeginLifetime];
                }
                return functions;
            }
        }

        internal void SetDependencies(IClassCollection classes, IFunctionCollection functions)
        {
            _classes = classes;
            _functions = functions;
        }
    }
}
